using UnityEngine;
using System.Collections;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {
	
	// What to chase?
	public Transform target;
	
	// How many times each second we will update our path
	public float updateRate = 2f;
	
	// Caching
	private Seeker seeker;
	private Rigidbody2D rb;
	
	//The calculated path
	public Path path;
	
	//The AI's speed per second
	public float speed = 300f;
	private float refSpeed;
	public bool isSlowed = false;
	public ForceMode2D fMode;
	public float slowCounter = 0, speedFactor = 2;
	public float timer;

	public bool pathIsEnded = false;
	
	// The max distance from the AI to a waypoint for it to continue to the next waypoint
	public float nextWaypointDistance = 3;
	
	// The waypoint we are currently moving towards
	private int currentWaypoint = 0;

	private bool searchingForPlayer = false;
	GameMaster gmRef;
	
	void Start () {
		timer = 0;
		refSpeed = speed;
		seeker = GetComponent<Seeker>();
		rb = GetComponent<Rigidbody2D>();
		
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer());
			}
			return;
		}
		// Start a new path to the target position, return the result to the OnPathComplete method
		seeker.StartPath (transform.position, target.position, OnPathComplete);
		StartCoroutine (UpdatePath ());
		gmRef = GameObject.FindWithTag ("GM").GetComponent<GameMaster> ();
	}

	void Update(){
		if (isSlowed) {
			timer+=Time.deltaTime;
			if(timer<=slowCounter){
				//speed = 0f;
				this.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
			}else{
				isSlowed = false;
				this.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
			}
		} else {
			speed = refSpeed;
			timer = 0;
		}
	}
	IEnumerator SearchForPlayer () {
		GameObject sResult = GameObject.FindGameObjectWithTag ("Player");
		if (sResult == null) {
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (SearchForPlayer());
		} else {
			target = sResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath());
			return false;
		}
	}

	IEnumerator UpdatePath () {
			if (target == null) {	
				if (!searchingForPlayer) {
					searchingForPlayer = true;
					StartCoroutine (SearchForPlayer ());
				}
				return false;
			}
		
			// Start a new path to the target position, return the result to the OnPathComplete method
			seeker.StartPath (transform.position, target.position, OnPathComplete);
		
			yield return new WaitForSeconds (1f / updateRate);
			StartCoroutine (UpdatePath ());
	}
	
	public void OnPathComplete (Path p) {
		//Debug.Log ("We got a path. Did it have an error? " + p.error);
		if (!p.error) {
			path = p;
			currentWaypoint = 0;
		}
	}
	
	void FixedUpdate () {
			if (target == null) {
				if (!searchingForPlayer) {
					searchingForPlayer = true;
					StartCoroutine (SearchForPlayer ());
				}
				return;
			}
			//TODO: Always look at player?
			if (path == null)
				return;
		
			if (currentWaypoint >= path.vectorPath.Count) {
				if (pathIsEnded)
					return;
			
				//Debug.Log ("End of path reached.");
				pathIsEnded = true;
				return;
			}
			pathIsEnded = false;
		
			//Direction to the next waypoint
			Vector3 dir = (path.vectorPath [currentWaypoint] - transform.position).normalized;
			dir *= speed * Time.fixedDeltaTime;
		
			//Move the AI
			rb.AddForce (dir, fMode);
		
			float dist = Vector2.Distance (transform.position, path.vectorPath [currentWaypoint]);
			if (dist < nextWaypointDistance) {
				currentWaypoint++;
				return;
			}
		}	
}
using UnityEngine;
using System.Collections;

public class CreepMovement : MonoBehaviour
{
	//Public variables
	public Transform[] path;
	public float moveSpeed;
	public float rotationSpeed;
	public int currentPoint;
	public int scoreValue;


	//Private variables
	//nextPoint contains the transformation information of the next point, used for looking at it, possible other implementations
	private Transform nextPoint = null;
	//to be used to find the next rotation
	private Quaternion lookRotation;
	//direction to look at the next point
	private Vector3 dir;
	private GameController gameController;
	public float health;

	//Chunks of this code adapted from the YouTube video by FlatTutorials
	//https://www.youtube.com/watch?v=fvdRKS8x0aM

	// Use this for initialization
	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindWithTag ("GameController");

		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <GameController>();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}

		//Set current point to 0
		currentPoint = 0;

		//Set health to 10
		health = 5.0f;

		//Set the first waypoint, ensure path is not 0 first
		if (path.Length > 0) {
			nextPoint = path [0];
		}

		//Set the angle the creep is looking, do not need to worry about rotating at a certain speed
		if(nextPoint != null){
			//look at the next point
			transform.LookAt(nextPoint);

			//set the direction vector here
			dir = (nextPoint.position - transform.position).normalized;

			//set the rotation needed to face the next point
			lookRotation = Quaternion.LookRotation(dir);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		//Distance between position of current creep and currentPoint
		float dist = Vector3.Distance (nextPoint.position, transform.position);

		//Move at a constant speed towards the point at a constant speed
		transform.position = Vector3.MoveTowards (transform.position, nextPoint.position, Time.deltaTime * moveSpeed);

		//rotate to face the next point
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);

		//If the distance to the current point is suffeciently close, update the point and look at the next point, checking if another point exists
		if (dist <= 0.0f) {
			//If this is the final point that it hit
			if (currentPoint == path.Length - 1) {
				//temp do nothing

				//TODO destroy the object, decrement score
				//if (gameObject != NULL) {
					Destroy (gameObject);
					gameController.updateScore (1);
				//}
			}

			//If this is not the final point update the current point and rotate the creep to face the next point
			else {
				//update current point
				currentPoint++;
				//set nextPoint to the new point
				nextPoint = path[currentPoint];

				//set the direction vector here
				dir = (nextPoint.position - transform.position).normalized;

				//set the rotation needed to face the next point
				lookRotation = Quaternion.LookRotation(dir);
			}
		}

	}

	//Take damage return true if dead
	public bool TakeDamage(float damage){
		health = health - damage;
		if (health <= 0) {
			health = 0.0f;
			Die ();
			//DestroyObject (gameObject);
			return true;
		} else
			return false;
	}

	//Die (should take a couple seconds to let the tower continue targeting it for a few frames
	public void Die() {

	}


	//Visualize the Gizmos to make the maze
	void OnDrawGizmos ()
	{
		if (path.Length > 0) {
			for (int i = 0; i < path.Length; i++) {
				if (path [i] != null) {
					Gizmos.DrawSphere (path [i].position, 0.5f);
				}
			}
		}
	}
}

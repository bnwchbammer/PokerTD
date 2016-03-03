using UnityEngine;
using System.Collections;

//Primary task, spawn objects
public class GameController : MonoBehaviour {
	//Public variables
	//creep game object
	public GameObject creep;
	//turret game object (subject to change to something else)
	public GameObject turret;
	//starting point for each creep
	public Vector3 spawnPosition;
	//number of creeps per wave
	public int creepCount;
	//wait value
	public float spawnWait;
	//score and score text
	public GUIText scoreText;
	//turret inventory (should not be more than 1)
	public int turretsAvail;

	//Private variables
	private int score;
	private int floorMask;

	void Start() {
		score = 10;
		UpdateScore ();
		StartCoroutine (SpawnCreeps());
		floorMask = LayerMask.GetMask ("Floor");
		turretsAvail = 1;
		//turret = new GameObject<TurretBehavior>();
	}

	IEnumerator SpawnCreeps() {
		//for loop to loop through and spawn creeps
		for (int i = 0; i < creepCount; i++) {
			Quaternion spawnRotation = Quaternion.identity;
			Instantiate (creep, spawnPosition, spawnRotation);
			yield return new WaitForSeconds(spawnWait);
		}
	}

	void Update() {
		//If there is a turret available display it with a lower alpha color
		if (turretsAvail > 0) {
			//Vector3 defaultStart = new Vector3 (0f, 0f, 0f);
			//Instantiate (turret, defaultStart, spawnRotation);
			HoverTurret ();
		}


		//Checker, set turrets to less than 1
		if (turretsAvail < 0) {
			Debug.Log ("Turrets available went less than 0");
			turretsAvail = 0;
		}
	}

	void FixedUpdate() {
		//If the mouse is clicked
		//TODO This currently assumes the turret was placed successfully, make sure there's a fail state
		if(Input.GetButton("Fire1") && turretsAvail > 0) {
			PlaceTurret();
			turretsAvail--;
		}
	}

	public void updateScore(int decScoreValue) {
		score -= decScoreValue;
		UpdateScore ();
	}


	void UpdateScore() {
		scoreText.text = "Score: " + score;
	}


	//Hover and place raycasts adapted from Unity Learn PlayerMovement tutorial
	//http://unity3d.com/learn/tutorials/projects/survival-shooter/player-character?playlist=17144
	void HoverTurret() {
		//Create a ray from the mouse cursor on screen in the direction of the camera
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		//Create a RaycastHit variable to store information about what was hit by the ray
		RaycastHit floorHit;

		//Perform the raycast and see if it hits something on the floor layer
		if(Physics.Raycast(camRay, out floorHit, 100f, floorMask)) {
			//Create an identity Quaternion
			//Quaternion spawnRotation = Quaternion.identity;
			//vec 3 of the point hit
			Vector3 spawnPosition = floorHit.point + new Vector3(0f,.5f,0f);
			//Instantiate the player at the point the mouse was at
			//Instantiate (turret, spawnPosition, Quaternion.identity);
		}
	}

	void PlaceTurret() {
		//Create a ray from the mouse cursor on screen in the direction of the camera
		Ray camRay = Camera.main.ScreenPointToRay (Input.mousePosition);

		//Create a RaycastHit variable to store information about what was hit by the ray
		RaycastHit floorHit;

		//Perform the raycast and see if it hits something on the floor layer
		if(Physics.Raycast(camRay, out floorHit, 100f, floorMask)) {
			//Create an identity Quaternion
			//Quaternion spawnRotation = Quaternion.identity;
			//vec 3 of the point hit
			Vector3 spawnPosition = floorHit.point + new Vector3(0f,.5f,0f);

			//Instantiate the player at the point the mouse was at
			GameObject thisTurret = (GameObject) Instantiate (turret, spawnPosition, Quaternion.identity);
			//Set the turret to its ready stance
			TurretBehavior thisTurretController = thisTurret.GetComponent<TurretBehavior> ();
			thisTurretController.active = true;
		}
	}
}

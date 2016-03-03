using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TurretBehavior : MonoBehaviour {

	public List<GameObject> enemyList = new List<GameObject>(4);
	private Vector3 enemyVec;
	private Quaternion lookRotation;
	private float nextFire;

	public float rotateSpeed;
	public float fireRate;
	public bool active;

	/*
	//Big idea of this script:
	//When adding, add to the front?
    */


	// Use this for initialization
	void Start () {
		active = false;
	}

	void OnTriggerEnter(Collider other) {
		if (other.tag == "Enemy" && active == true) {
			enemyList.Add (other.gameObject);
		} else
			return;
	}

	void OnTriggerExit(Collider other) {
		if (other.tag == "Enemy" && active == true) {
			enemyList.Remove (other.gameObject);
		} else
			return;
	}

	public void SetReady() {
		active = true;
	}


	// Update is called once per frame
	void Update () {


		if (enemyList.Count > 0) {
			//TODO if the list isn't empty look at the collided object and shoot at it




		}
	}

	void FixedUpdate() {
		if (enemyList.Count > 0) {
			//Get the vector to the enemy being shot at
			enemyVec = (enemyList [0].gameObject.transform.position - transform.position).normalized;

			//Start looking at the enemy
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation (enemyVec), Time.deltaTime * rotateSpeed);

			if (Time.time > nextFire) {
				nextFire = Time.time + fireRate;
				//Shoot bullet to cause damage
				CreepMovement enemy = enemyList [0].GetComponent<CreepMovement> ();
				if (enemy.TakeDamage (1) == true) {
					enemyList.RemoveAt (0);
				}
			}
		} else {
			//If no targets, rotate slowly
			transform.Rotate (new Vector3(0,15,0) * -Time.deltaTime);
		}
	}
}

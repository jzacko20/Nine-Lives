using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseDrone : MonoBehaviour {

	public Transform target;
	private Vector3 targetTransform;
	public float moveSpeed = 1;
	private Transform t;
	private Rigidbody rb;
	private Health health;
	private bool deleteSelf;	//if a collision was detected, needs to delete

	// Use this for initialization
	void Awake() {
		//targetTransform = new Vector3(14,11,0);
		t = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
		health = GetComponent<Health>();
		targetTransform = target.GetComponent<Transform>().position;
	}
	
	//Move to guy
	void FixedUpdate() {
		rb.MovePosition(t.position+Vector3.Normalize(target.position - t.position ) * Time.deltaTime* moveSpeed);	//set speed (floaty for some reason)
		//rb.MovePosition(t.position+(target.position - t.position) * Time.deltaTime * moveSpeed);		//move rate. 
	}

}

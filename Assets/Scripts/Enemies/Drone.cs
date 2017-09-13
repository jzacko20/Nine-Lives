using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour {

	public Vector3 targetTransform;
	public Transform target;
	public bool useObject;
	public float moveRate = 0.001f;
	private Transform t;
	private Rigidbody rb;

	// Use this for initialization
	void Awake() {
		//targetTransform = new Vector3(14,11,0);
		t = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
		if(useObject) targetTransform = target.GetComponent<Transform>().position;
	}
	
	//Move to guy
	void FixedUpdate() {
		//t.Translate(t.position - targetTransform*Time.deltaTime*moveRate);
		//rb.MovePosition(t.position + t.forward * Time.deltaTime* moveRate);
		rb.MovePosition(t.position+( target.position - t.position ) * Time.deltaTime* moveRate);

	}
}

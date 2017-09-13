using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Debris : MonoBehaviour {

	private Transform t;
	private Rigidbody rb;
	public float killY = -5;

	// Use this for initialization
	void Awake() {
		t = GetComponent<Transform>();
		rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void OnCollisionEnter(Collision other)
	{
		if (other.collider.tag  == "Floor")
		{
			int tRange = 8;
			t.position -= new Vector3(0,-0.5f,0);
			rb.AddForce(-600f,300f,300f);
			//rb.AddExplosionForce(2000,t.position + new Vector3(4,-2,0),20);
			rb.AddTorque(Random.Range(-tRange,tRange),Random.Range(-tRange,tRange),Random.Range(-tRange,tRange));
			print("OLO");
		}
	}

	private void FixedUpdate()
	{
		if (t.position.y <= killY)
		{
			Destroy(gameObject);
		}
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour {

	public float speed = 10;
	public int damageAmount = 1;
	[SerializeField] private float lifetime = 2;	//how long until it dies
	private Transform t;
	// Use this for initialization
	void Start () {
		t = GetComponent<Transform>();
		Destroy(gameObject,lifetime);
	}
	
	// Update is called once per frame
	void FixedUpdate() {
		//move forward
		t.Translate(Vector3.forward *Time.deltaTime*speed);
	}

	void OnTriggerEnter( Collider col )
	{
		Health h = col.gameObject.GetComponent<Health>();
		if (h != null)
		{
			h.Damage(damageAmount);
			Destroy(gameObject);

		}
		if (col.tag == "Floor")
		{
			Destroy(gameObject);
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	private Health myHealth;
	public bool suicideHit;	//kill self after dealing damage
	public List<GameObject> debris;
	private Transform t;
	
	// Use this for initialization
	void Awake() {
		myHealth = GetComponent<Health>();
		t = GetComponent<Transform>();
		//myHealth.OnDeath += Explode;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	// Use this for initialization
	void OnEnable()
	{
		myHealth.OnDeath += Explode;
	}

	// Update is called once per frame
	void OnDisable()
	{
		myHealth.OnDeath -= Explode;
	}
	//death
	public void Explode()
	{
		var randPosition = new Vector3();
		var randRotation = new Vector3();
		float f = 1.5f;
		if (debris != null)
		{
			foreach(GameObject i in debris)
			{
				randPosition = new Vector3(Random.Range(f,f),Random.Range(f,f),Random.Range(f,f));
				randRotation = new Vector3(Random.Range(0,360),Random.Range(0,360),Random.Range(0,360));
				t.Rotate(randRotation);
				Instantiate(i,t.position+ randPosition,t.rotation);
			}
		}
		Destroy(gameObject);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter:MonoBehaviour {
	public string button = null;	//Button that shoots
	public GameObject projectile = null;  //what gets shot
	public Transform source = null; //where bullets are created
	public float fireRate = 1;	//shots per second
	private bool fireReady = true;
	private Transform myBike;	//<><>

	// Use this for initialization
	void Start()
	{
		//myBike = source.parent;//<><>not reading the rotation correctly, using the parent instead
	}

	// Update is called once per frame
	void Update()
	{
		if(Input.GetButton(button) && fireReady)
		{
			Instantiate(projectile,source.position,source.rotation);
		
			float reloadTime = 1 / fireRate;
			fireReady = false;
			Invoke("Reload",reloadTime);
		}
	}

	// Update is called once per frame
	void Reload()
	{
		fireReady = true;
	}
}

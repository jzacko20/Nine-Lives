using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGScroller : MonoBehaviour {

	public static float scrollSpeed = 6f;
	public float range = 150f;

	private Transform t = null;

	// Update is called once per frame
	void Start()
	{
		t = GetComponent<Transform>();
	}

	// Update is called once per frame
	void Update () {
		t.position += new Vector3(-scrollSpeed,0,0);
		if(t.position.x < -range / 2)
		{
			t.position += new Vector3(range,0,0);
		}
	}
}

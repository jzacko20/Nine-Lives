using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController:MonoBehaviour {
	private Transform t = null;
	private Transform bikeTransform = null;

	[Header("Horizontal Movement...")]
	public float fAccel = 1f;   //front acceleration
	public float fSpeedMax = 3; //front max_speed
	public float bAccel = 1f;   //back acceleration
	public float bSpeedMax = 2; //back max_speed
	public float aAccel = 1f;   //air acceleration
	public float aSpeedMax = 5; //air max_speed
	public float decel = 1f;		//deceleration
	public float boundry = 12;  //how far from the origin the bike can go 
	public float boundDecel = 2f; //how hard the bike normalizes when out of bounds
	[Header("Vertical Movement...")]
	public float jumpSpeed = 5;  //vertical speed gained when jumping
	public float jumpTimeMax = 1;   //how long the player can "ascend" in seconds
	public float fallRate = 6f;	//gravity
	public float fallSpeed = 5;		//Terminal velocity
	[Header("Wheelie...")]
	public float wheelieUp = 120;    //how much wheelie you gain per second while presssing up
	public float wheelieDown = 100;    //how much wheelie you lose per second while presssing down
	public float wheelieDecay = 80; //how much wheelie you lose per second
	public float wheelieMax = 50;   //Maximum wheelie angle
	public float wheelieMin = -40;		//minimum wheelie angle (while jumping)
	[Header("In game values...")]
	[SerializeField] private float hSpeed = 0; //current horizontal speed
	[SerializeField] private float vSpeed = 0; //current vertical speed
	public float iTime = 2;
	public bool invuln = false;
	public float wheelie = 0; //the current wheelie angle
	[SerializeField] private MeshRenderer riderRenderer;
	private float jumpTime = 0; //current time spent acsending

	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	void Start()
	{
		t = GetComponent<Transform>();
		bikeTransform = t.GetChild(0);
	}

	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	void Update()
	{
		MovementUpdate();
		JumpUpdate();
		WheelieUpdate();
		if (invuln) DamageFlash();
	}

	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void MovementUpdate()
	{
		//Find current acceleration/speed values
		float myAccel = fAccel;
		float hSpeedMax = fSpeedMax;
		float hSpeedMin = -bSpeedMax;
		if(transform.position.y > 0)  //use midair values
		{
			myAccel = aAccel;
			hSpeedMax = aSpeedMax;
			hSpeedMin = -aSpeedMax;
		} else if(Input.GetAxis("Horizontal") < 0) //use reverse values
		{
			myAccel = bAccel;
		}
		//accelerate to middle of screen when on the edge
		if(t.position.x < -boundry && hSpeed < 0)
		{	//Left boundry
			hSpeed += boundDecel * Time.deltaTime;
			hSpeed = Mathf.Min(hSpeed,0);
		} else if(t.position.x > boundry+hSpeed && hSpeed > 0)
		{	//Right boundry
			hSpeed -= boundDecel * Time.deltaTime;
			hSpeed = Mathf.Max(hSpeed,0);
		} else if (Input.GetAxis("Horizontal") != 0)
		{ //Take input from player
			hSpeed += Input.GetAxis("Horizontal") * myAccel * Time.deltaTime;
		}else if (Mathf.Abs(hSpeed)  > decel * Time.deltaTime && t.position.y == 0)
		{ //Decelerate Normally (if on ground)
			hSpeed -= Mathf.Sign(hSpeed) * decel * Time.deltaTime;
		} else if (t.position.y == 0)
		{	//stop (if on ground)
			hSpeed = 0;
		}
		hSpeed = Mathf.Clamp(hSpeed,hSpeedMin,hSpeedMax);
		//t.position += new Vector3(hSpeed,0,0);
		t.Translate(0,0,hSpeed);
	}

	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void JumpUpdate()
	{
		//Find vSpeed
		if (Input.GetButton("B1") && jumpTime < jumpTimeMax)
		{//rise
			vSpeed = jumpSpeed;
			jumpTime += Time.deltaTime;
		} else
		{//fall
			vSpeed -= fallRate * Time.deltaTime;
			vSpeed = Mathf.Max(vSpeed,-fallSpeed);
			jumpTime = jumpTimeMax;
		}
		//apply speed
		t.position += new Vector3(0,vSpeed * Time.deltaTime,0);
		if (t.position.y < 0)//if touched the ground
		{
			t.position -= new Vector3(0,t.position.y,0);
			//t.Translate(hSpeed,0,0);
			jumpTime = 0;
		}
	}

	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void WheelieUpdate()
	{
		if(Input.GetAxis("Vertical") > 0) wheelie += Time.deltaTime * wheelieUp;	//wheelie up
		else if(Input.GetAxis("Vertical") < 0) wheelie -= Time.deltaTime * wheelieDown; //wheelie down
		else if(Mathf.Abs(wheelie) < wheelieDecay *Time.deltaTime) wheelie = 0; //wheelie neutral
		else wheelie -= Time.deltaTime * wheelieDecay*Mathf.Sign(wheelie);	//wheelie decay
		float myWheelieMin = 0;	//the current minimum
		if (t.position.y > 0) myWheelieMin = wheelieMin;
		wheelie = Mathf.Clamp(wheelie,myWheelieMin,wheelieMax);

		//apply transform
		//bikeTransform.rotation = Quaternion.Euler(-wheelie,90,0);
		bikeTransform.rotation = Quaternion.Euler(0,0,wheelie);

	}

	//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
	public void FireUpdate()
	{
		if(Input.GetButton("B2") ) wheelie += Time.deltaTime * wheelieUp;
		else if(Input.GetAxis("Vertical") < 0) wheelie -= Time.deltaTime * wheelieDown;
		else wheelie -= Time.deltaTime * wheelieDecay * Mathf.Sign(wheelie);
		float myWheelieMin = 0; //the current minimum
		if(t.position.y > 0) myWheelieMin = wheelieMin;
		wheelie = Mathf.Clamp(wheelie,myWheelieMin,wheelieMax);

		//apply transform
		bikeTransform.rotation = Quaternion.Euler(-wheelie,90,0);

	}


	public void OnCollisionStay( Collision collision )
	{
		print("COLLIDE");
		Enemy myEnemy = collision.gameObject.GetComponent<Enemy>();
		if(myEnemy == null) return;
		//find direction of the guy
		print("ENEMOY");
		float otherX = collision.gameObject.transform.position.x;
		float myX = t.position.x;
		Damage(Mathf.Sign(myX- otherX));
		if(myEnemy.suicideHit) myEnemy.Explode();
		
	}


	private void Damage(float knockbackDir)
	{
		//Apply Knockback
		hSpeed = 0.1f* knockbackDir;
		vSpeed = 10;
		jumpTime = jumpTimeMax;
		wheelie += 20;
		//apply damage
		if(!invuln)
		{
			invuln = true;
			Invoke("EndITime",iTime);
			//Remove health here.
		}
	}

	private void EndITime()	//when the player stops being invulnerable
	{
		invuln = false;
		riderRenderer.enabled = true;
	}

	private void DamageFlash()
	{
		riderRenderer.enabled = !riderRenderer.enabled;
	}
}

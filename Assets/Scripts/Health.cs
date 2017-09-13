using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health:MonoBehaviour {

	[HideInInspector] public int hp;
	public int hpMax = 10;
	public AudioClip damageClip = null;
	public AudioClip highDamageClip = null;
	public AudioClip dieClip = null;
	public AudioSource damageAudioSource = null;
	public delegate void HealthAction();
	public event HealthAction OnDeath;

	// Use this for initialization
	void Awake() {
		hp = hpMax;
	}

	public void Damage(int value)
	{
		hp -= value;
		if (damageClip != null && damageAudioSource != null)
		{
			AudioClip sndToPlay = damageClip;
			if(hp <= 0 && dieClip != null) sndToPlay = dieClip;
			else if(hp <= hpMax / 4 && highDamageClip != null) sndToPlay = highDamageClip;
			damageAudioSource.clip = sndToPlay;
			//play damage sound
			damageAudioSource.Play();
		}
		if(hp <= 0 && OnDeath != null)
		{
			OnDeath();
		}
	}
}

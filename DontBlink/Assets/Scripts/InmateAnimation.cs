﻿using UnityEngine;
using System.Collections;

public class InmateAnimation : MonoBehaviour {

	public float blend;

	private Animator animator;

	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
		animator.SetFloat ("Blend", blend);
		animator.speed = Random.Range (0.9f, 1.1f);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

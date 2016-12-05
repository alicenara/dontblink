using System;
using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class basicAI : MonoBehaviour {

		public NavMeshAgent agent;	//handles the level positions for movement
		public ThirdPersonCharacter character;	//handles the ai functions
		public CheckForObservers checkForObservers;
		public AudioClip walkSound;
		AudioSource audioSource;
		private bool initialized;

		public enum State {
			PATROL,
			CHASE,
			OBSERVED
		}

		public State state;
		private bool alive;

		// Varaibles for patrolling
		public GameObject[] waypoints;
		private int waypointInd = 0;
		public float patrolSpeed = 0.5f;

		//Variables for chasing
		public float chaseSpeed = 0.5f;
		public GameObject target;

		// Initialization
		void Start () {
			agent = GetComponent<NavMeshAgent> ();
			character = GetComponent<ThirdPersonCharacter> ();
			checkForObservers = GetComponent<CheckForObservers> ();
			audioSource = GetComponent <AudioSource> ();
			initialized = false;

		
			agent.updatePosition = true;
			agent.updateRotation = false;

			state = basicAI.State.PATROL;

			alive = true;

			StartCoroutine ("FSM");

		}

		// runs while ai is alive and keeps updating ai behaviour by checking current ai state
		IEnumerator FSM()
		{
			while (alive) 
			{
				if (checkForObservers.IsObserved ()) {
					state = basicAI.State.OBSERVED;
				} 
				switch (state) 
				{
				case State.OBSERVED:
					Freeze ();
					break;
				case State.PATROL:
					Patrol ();
					break;
				case State.CHASE:
					Chase ();
					break;
				}

				yield return null;
			}
		}

		// runs if player location is unknown and ai not observed
		void Patrol()
		{
			agent.speed = patrolSpeed;
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
			if (audioSource.volume < 1.0f) {
				audioSource.volume += Time.deltaTime;
			}
		}

		// runs if play location is known and ai not observed
		void Chase()
		{
			if (!checkForObservers.IsObserved()){
				agent.speed = chaseSpeed;
				agent.SetDestination (target.transform.position);
				character.Move(agent.desiredVelocity, false, false);
				if (!audioSource.isPlaying) {
					audioSource.Play();
				}
				if (audioSource.volume < 1.0f) {
					audioSource.volume += Time.deltaTime;
				}
			}
		}

		// runs if ai is observed
		void Freeze()
		{
			agent.speed = 0;
			agent.SetDestination (character.transform.position);
			character.Move(agent.desiredVelocity, false, false);
			if (audioSource.volume > 0.0f) {
				audioSource.volume -= 2.0f * Time.deltaTime;
			} else {
				audioSource.Stop ();
			}
		}

		// runs if player is within ai awareness radius
		// has no effect if ai is observed
		void OnTriggerStay (Collider coll)
		{
			if (coll.tag == "Player")
			{
				state = basicAI.State.CHASE;
				target = coll.gameObject;
			}
			
		}

		// Checks if player is touching the ai
		void OnTriggerEnter (Collider coll)
		{
			if (coll.tag == "Player")
			{
				if (initialized) {
					if (coll is CapsuleCollider) {
						//Enter code for game over here
						Debug.Log ("You are Dead");

					}
				} else {
					initialized = true;
				}
			}

		}


	}
}


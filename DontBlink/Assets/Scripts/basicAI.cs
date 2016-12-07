using System;
using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class basicAI : MonoBehaviour {

		NavMeshAgent agent;	//handles the level positions for movement
		ThirdPersonCharacter character;	//handles the ai functions
		CheckForObservers checkForObservers;
		AudioSource audioSource;
		bool initialized;
		float patrolTick = 0.0f;


		public enum State {
			PATROL,
			CHASE,
			OBSERVED
		}

		public State state;
		private bool alive;
		public bool playerAlive = true;

		// Varaibles for patrolling
		public GameObject[] waypoints;
		int waypointInd = 0;
		public float patrolSpeed = 0.5f;

		//Variables for chasing
		public float chaseSpeed = 0.5f;
		Vector3 target;

		// Initialization
		void Start () {
			agent = GetComponent<NavMeshAgent> ();
			character = GetComponent<ThirdPersonCharacter> ();
			checkForObservers = GetComponent<CheckForObservers> ();
			audioSource = GetComponent <AudioSource> ();
			audioSource.pitch = UnityEngine.Random.Range (0.8f, 1.2f);
			initialized = false;

		
			agent.updatePosition = true;
			agent.updateRotation = false;

			state = State.PATROL;

			alive = true;
			playerAlive = true;

			StartCoroutine ("FSM");

		}

		// runs while ai is alive and keeps updating ai behaviour by checking current ai state
		IEnumerator FSM() {
			while (alive) {
				if (checkForObservers.IsObserved ()) {
					state = State.OBSERVED;
				} else if (state == State.OBSERVED) {
					state = State.PATROL;
				}
				switch (state) {
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
		void Patrol() {
			patrolTick += Time.deltaTime;
			if (patrolTick > 5.0f) {
				patrolTick = 0.0f;
				float angle = UnityEngine.Random.Range (0.0f, 2 * Mathf.PI);
				Vector3 direction = new Vector3 (Mathf.Cos(angle), 0.0f, Mathf.Sin(angle));
				agent.SetDestination(transform.position + direction * 10.0f);
			}
			agent.speed = patrolSpeed;
			character.Move(agent.desiredVelocity, false, false);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
			if (audioSource.volume < 1.0f) {
				audioSource.volume += Time.deltaTime;
			}
		}

		// runs if play location is known and ai not observed
		void Chase() {
			agent.speed = chaseSpeed;
			agent.SetDestination (target);
			character.Move(agent.desiredVelocity, false, false);
			if (!audioSource.isPlaying) {
				audioSource.Play();
			}
			if (audioSource.volume < 1.0f) {
				audioSource.volume += Time.deltaTime;
			}
		}

		// runs if ai is observed
		void Freeze() {
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
		void OnTriggerStay (Collider coll) {
			if (coll.tag == "Player") {
				state = State.CHASE;
				target = coll.gameObject.transform.position;
			} else if (coll.tag == "Safe Player") {
				state = State.PATROL;
			}
		}

		// Checks if player is touching the ai
		void OnCollisionEnter (Collision coll) {

			if (coll.collider.tag == "Player") {
				

					if (coll.collider is CapsuleCollider) {
						//Enter code for game over here
						Debug.Log ("You are Dead");
						playerAlive = false;
					}
				 
			}

		}

		void OnTriggerExit (Collider coll) {
			if (coll.tag == "Player") {
				state = State.PATROL;
			}
		}
	}
}


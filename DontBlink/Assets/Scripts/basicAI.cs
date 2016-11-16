using System;
using UnityEngine;
using System.Collections;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class basicAI : MonoBehaviour {

		public NavMeshAgent agent;
		public ThirdPersonCharacter character;
		public CheckForObservers checkForObservers;

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
		public float chaseSpeed = 1f;
		public GameObject target;

		// Use this for initialization
		void Start () {
			agent = GetComponent<NavMeshAgent> ();
			character = GetComponent<ThirdPersonCharacter> ();
			checkForObservers = GetComponent<CheckForObservers> ();

			agent.updatePosition = true;
			agent.updateRotation = false;

			state = basicAI.State.PATROL;

			alive = true;

			StartCoroutine ("FSM");

		}

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

		void Patrol()
		{
			agent.speed = patrolSpeed;

		}

		void Chase()
		{
			if (!checkForObservers.IsObserved()){
				agent.speed = chaseSpeed;
				agent.SetDestination (target.transform.position);
				character.Move(agent.desiredVelocity, false, false);
			}
		}

		void Freeze()
		{
			agent.speed = 0;
			agent.SetDestination (character.transform.position);
			character.Move(agent.desiredVelocity, false, false);

		}

		void OnTriggerStay (Collider coll)
		{
			if (coll.tag == "Player")
			{
				state = basicAI.State.CHASE;
				target = coll.gameObject;
			}
			
		}


	}
}


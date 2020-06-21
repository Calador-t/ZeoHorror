using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AI;

class AIMovement : MonoBehaviour
{
	private NavMeshAgent navAgent;
	public GameObject WaypointRoot;
	
	private Waypoint currentDestination;
	private Waypoint lastDestination;

	private bool waitForContinue;
	private float remainingPauseTime;


	private void Start()
	{
		navAgent = gameObject.GetComponent<NavMeshAgent>();
		if ( WaypointRoot == null ) Debug.LogError( $"No Waypoint Root for {gameObject.name} attached" );
		currentDestination = GetNearWaypoint();
	}

	private void Update()
	{
		//if ( waitForContinue )
		//{
		//	float pausedTime = Time.deltaTime;
		//	remainingPauseTime -= pausedTime;
		//	if ( remainingPauseTime <= 0 )
		//	{
		//		waitForContinue = false;

		//		navAgent.SetDestination( currentDestination.GetRndPosition() );
		//	}
		//}

		if ( !navAgent.pathPending && navAgent.remainingDistance < .5f )
		{
			var tempCurrent = currentDestination;
			currentDestination = currentDestination.GetNextWaypoint(lastDestination);
			navAgent.SetDestination( currentDestination.transform.position );
			lastDestination = tempCurrent;
			//remainingPauseTime = currentDestination.GetPauseTime();
			//if(remainingPauseTime > 0 )
			//{
			//	waitForContinue = true;
			//}
		}
	}


	private Waypoint GetNearWaypoint()
	{
		float distance = float.MaxValue;
		Waypoint nearest = null;

		foreach ( var waypoint in WaypointRoot.GetComponentsInChildren<Waypoint>() )
		{
			float newDistance;
			if ( (newDistance = Vector3.Distance( transform.position, waypoint.transform.position )) < distance )
			{
				distance = newDistance;
				nearest = waypoint;
			}
		}
		return nearest;
	}



}


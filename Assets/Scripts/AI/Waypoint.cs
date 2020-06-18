using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
	[HideInInspector]
	public List<Waypoint> NextWaypoints = new List<Waypoint>(0);

	[HideInInspector]
	public int AdjacentWaypointCount = 0;

	[Range(0,10)]
	public float WaypointWidth = .5f;

	public bool AllowGoBack = false;
	public bool PauseSpot = false;
	public float MinPauseTime = 0f;
	public float MaxPauseTime = 2f;


	/// <summary>
	/// Returns a random position inside the waypoint
	/// </summary>
	/// <returns></returns>
	public Vector3 GetRndPosition()
	{
		Vector3 minBound = transform.position + transform.right * WaypointWidth / 2;
		Vector3 maxBound = transform.position + transform.right * WaypointWidth / 2;

		return Vector3.Lerp( minBound, maxBound, Random.Range( 0f, 1f ) );
	}

	public void RemoveWaypointFromReference(Waypoint waypoint )
	{
		NextWaypoints.Remove( waypoint );
	}

	public void Resize()
	{
		while(AdjacentWaypointCount > NextWaypoints.Count )
		{
			NextWaypoints.Add( null );
		}
		while(AdjacentWaypointCount < NextWaypoints.Count )
		{
			NextWaypoints.RemoveAt( NextWaypoints.Count - 1 );
		}
	}
}

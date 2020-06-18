#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
class WaypointVisualizer
{

	[DrawGizmo( GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable )]
	public static void OnDrawSceneGizmo( Waypoint waypoint, GizmoType gizmoType )
	{
		if ( ( gizmoType & GizmoType.Selected ) != 0 )
		{
			Gizmos.color = Color.blue;
		}
		else
		{
			Gizmos.color = Color.blue * .5f;
		}
		Gizmos.DrawSphere( waypoint.transform.position, .1f );

		Handles.color = Color.blue;
		Handles.DrawWireDisc( waypoint.transform.position, waypoint.transform.up, waypoint.WaypointWidth / 2f );

		Gizmos.color = Color.blue;

		foreach ( var item in waypoint.NextWaypoints )
		{
			if ( item == null ) continue;

			Transform current = waypoint.transform;
			Transform target = item.transform;

			current.LookAt( target );
			target.LookAt( current );

			Vector3 offset = waypoint.transform.right * -waypoint.WaypointWidth / 2f;
			Vector3 offsetTarget = (item.transform.right * -item.WaypointWidth / 2f);

			Gizmos.DrawLine( waypoint.transform.position - offset, item.transform.position + offsetTarget );
			Gizmos.DrawLine( waypoint.transform.position + offset, item.transform.position - offsetTarget );


			Handles.color = Color.green;

			float distance = Vector3.Distance( current.position, target.position );
			Handles.ArrowHandleCap( 0, ( current.position + target.position ) / 2, current.rotation, distance/5, EventType.Repaint );
		}
	}


}


#endif

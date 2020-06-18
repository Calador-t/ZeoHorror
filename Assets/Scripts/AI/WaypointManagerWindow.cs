#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

class WaypointManagerWindow : EditorWindow
{
	[MenuItem( "Tools/Waypoint Editor" )]
	public static void Open()
	{
		GetWindow<WaypointManagerWindow>();
	}

	public Transform WaypointRoot;
	public List<Waypoint> Waypoints;

	public Dictionary<Waypoint, bool> WaypointInfoExpanded = new Dictionary<Waypoint, bool>();
	public Vector2 ScrollPos;
	public bool ShowDeleteButton;

	private void OnGUI()
	{
		SerializedObject obj = new SerializedObject( this );

		if ( WaypointRoot == null )
		{
			WaypointRoot = GameObject.FindGameObjectWithTag( "WaypointRoot" ).transform;
		}

		UpdateWaypoints();

		EditorGUILayout.PropertyField( obj.FindProperty( "WaypointRoot" ) );
		ShowDeleteButton = EditorGUILayout.Toggle( "Show delete button", ShowDeleteButton );

		if ( WaypointRoot == null )
		{
			EditorGUILayout.HelpBox( "No WaypointRoot tagged object loaded", MessageType.Warning );
		}
		else
		{
			EditorGUILayout.BeginVertical();
			ScrollPos = EditorGUILayout.BeginScrollView( ScrollPos );

			foreach ( var waypoint in Waypoints )
			{
				WaypointInfoExpanded[waypoint] = EditorGUILayout.BeginFoldoutHeaderGroup( WaypointInfoExpanded[waypoint], waypoint.gameObject.name );

				if ( WaypointInfoExpanded[waypoint] )
				{
					
					waypoint.name = EditorGUILayout.TextField( "Waypoint Name", waypoint.name );
					waypoint.WaypointWidth = EditorGUILayout.Slider( "Waypoint Width", waypoint.WaypointWidth, 0, 10 );
					waypoint.AllowGoBack = EditorGUILayout.Toggle( "Allow go back", waypoint.AllowGoBack );
					waypoint.PauseSpot = EditorGUILayout.Toggle( "Pause here", waypoint.PauseSpot );
					if ( waypoint.PauseSpot )
					{
						waypoint.MinPauseTime = EditorGUILayout.FloatField( "Min pause time (in sec)", waypoint.MinPauseTime );
						waypoint.MaxPauseTime = EditorGUILayout.FloatField( "Max pause time (in sec)", waypoint.MaxPauseTime );
						if(waypoint.MinPauseTime < 0 )
						{
							waypoint.MinPauseTime = 0;
						}
						if(waypoint.MinPauseTime > waypoint.MaxPauseTime )
						{
							waypoint.MaxPauseTime = waypoint.MinPauseTime;
						}
					}

					waypoint.AdjacentWaypointCount = EditorGUILayout.IntField( "Next Waypoints count", waypoint.AdjacentWaypointCount );

					waypoint.Resize();

					for ( int i = 0; i < waypoint.AdjacentWaypointCount; i++ )
					{
						waypoint.NextWaypoints[i] = ( Waypoint )EditorGUILayout.ObjectField( $"Next Waypoint {( i + 1 )}", waypoint.NextWaypoints[i], typeof( Waypoint ), true );
					}

					if ( ShowDeleteButton && GUILayout.Button( "Delete Waypoint" ) )
						DeleteWaypoint( waypoint );
				}
				EditorGUILayout.EndFoldoutHeaderGroup();

			}

			EditorGUILayout.EndScrollView();
			EditorGUILayout.EndVertical();

			EditorGUILayout.BeginVertical( "box" );
			DrawButtons();
			EditorGUILayout.EndVertical();
		}
		obj.ApplyModifiedProperties();
	}


	private void DrawButtons()
	{
		if ( GUILayout.Button( "Create Waypoint" ) )
		{
			CreateWaypoint();
		}
	}

	private void DeleteWaypoint( Waypoint waypoint )
	{
		foreach ( var item in Waypoints )
		{
			item.RemoveWaypointFromReference( waypoint );
		}
		DestroyImmediate( waypoint.gameObject );
	}

	private void UpdateWaypoints()
	{
		Waypoints = new List<Waypoint>( WaypointRoot.childCount );

		for ( int i = 0; i < WaypointRoot.childCount; i++ )
		{
			Waypoints.Add( WaypointRoot.GetChild( i ).GetComponent<Waypoint>() );
		}

		foreach ( var item in Waypoints )
		{
			if ( !WaypointInfoExpanded.ContainsKey( item ) )
				WaypointInfoExpanded.Add( item, false );
		}

	}
	private void CreateWaypoint()
	{
		GameObject waypointObject = new GameObject( "Waypoint " + WaypointRoot.childCount, typeof( Waypoint ) );
		waypointObject.transform.SetParent( WaypointRoot, false );
	}
}
#endif
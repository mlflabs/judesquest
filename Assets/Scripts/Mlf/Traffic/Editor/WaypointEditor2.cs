using System.Collections.Generic;
using Mlf.Traffic;
using UnityEditor;
using UnityEngine;



[CustomEditor(typeof(Waypoint))]
public class WaypointEditor2 : Editor
{
    public static Stack<Waypoint> calculatedPath = null;


    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();


        EditorGUILayout.HelpBox("First add the neighbour waypoint into Add Link Waipoint Field, then click button", MessageType.Info);

        if (GUILayout.Button("Add Waypoint to Neighbours"))
        {
            if (Selection.activeGameObject != null &&
               Selection.activeGameObject.GetComponent<Waypoint>())
            {
                Waypoint start = Selection.activeGameObject.GetComponent<Waypoint>();

                if (start.AddLinkWaypoint == null)
                {
                    Debug.LogWarning("No waypoint selected");
                }

                Waypoint neighbour = start.AddLinkWaypoint.GetComponent<Waypoint>();
                if (neighbour == null)
                {
                    Debug.LogWarning("Can only link with a waypoint object");
                }

                start.AddLink(neighbour);

                if (start.Bidirectional)
                    neighbour.AddLink(start);
                start.AddLinkWaypoint = null;
            }
        }

        if (calculatedPath == null)
        {
            if (GUILayout.Button("Calculate Path"))
            {
                EditorGUILayout.HelpBox("First add the neighbour waypoint into Add Link Waipoint Field, then click button to calculate fastest path",
                    MessageType.Info);
                if (Selection.activeGameObject != null &&
                  Selection.activeGameObject.GetComponent<Waypoint>())
                {
                    Waypoint start = Selection.activeGameObject.GetComponent<Waypoint>();
                    Waypoint end = start.AddLinkWaypoint.GetComponent<Waypoint>();

                    if (start == null || end == null) return;

                    calculatedPath = UtilsPath.FindPathToTarget(start, end);
                    Debug.Log("Path Calculated Number of nodes: " + calculatedPath.Count);
                }
            }
        }
        else
        {
            if (GUILayout.Button("Clear Calculated Path"))
            {
                calculatedPath = null;
            }
        }



    }



}




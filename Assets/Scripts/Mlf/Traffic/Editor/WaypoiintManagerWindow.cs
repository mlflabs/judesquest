using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

namespace Mlf.Traffic
{
    public class WaypoiintManagerWindow : EditorWindow
    {

        [MenuItem("Tools/Waypoint Editor")]
        public static void Open()
        {
            GetWindow<WaypoiintManagerWindow>();
        }

        public Transform waypointRoot;

        public int duplicates;

        private void OnGUI()
        {
            SerializedObject obj = new SerializedObject(this);
            EditorGUILayout.PropertyField(obj.FindProperty("waypointRoot"));
            //EditorGUILayout.PropertyField(obj);
            //EditorGUILayout.PropertyField()
            //EditorGUILayout obj = new SerializedObject(this);

            if (waypointRoot == null)
            {
                EditorGUILayout.HelpBox("Root transform must be selected. Please assign a root transform",
                     MessageType.Warning);
            }
            else
            {
                EditorGUILayout.BeginVertical("box");
                DwawButtons();
            }

            if (duplicates > 0)
            {
                EditorGUILayout.HelpBox("Found waypoints overlaping: " + duplicates,
                     MessageType.Warning);
            }
            obj.ApplyModifiedProperties();
        }

        private void DwawButtons()
        {
            if (GUILayout.Button("Create Waypoint"))
            {
                CreateWaypoint();
            }

            if (GUILayout.Button("Merge Overlaping Waypoints"))
            {
                DestroyDuplicates();
            }

            /*
            if (Selection.activeGameObject != null &&
                Selection.activeGameObject.GetComponent<Waypoint>())
            {
                if (GUILayout.Button("Create Waypoint Before"))
                {
                    //CreteWaypointBefore();
                }
                if (GUILayout.Button("Create Waypoint After"))
                {
                    //CreateWaypointAfter();
                }
                if (GUILayout.Button("Remove Waypoint"))
                {
                    //RemoveWaypoint();
                }
            }
            */

        }

        void CreateWaypoint()
        {

            GameObject waypointObject = new GameObject("Waypoint " + waypointRoot.childCount,
                typeof(Waypoint));

            waypointObject.transform.SetParent(waypointRoot, false);

            Waypoint waypoint = waypointObject.GetComponent<Waypoint>();

            if (Selection.activeGameObject != null &&
                Selection.activeGameObject.GetComponent<Waypoint>())
            {
                Waypoint prev = Selection.activeGameObject.GetComponent<Waypoint>();
                if (prev == null)
                {
                    Selection.activeGameObject = waypoint.gameObject;
                    return;
                }

                waypoint.transform.position = prev.transform.position;
                waypoint.transform.forward = prev.transform.forward;

                prev.AddLink(waypoint);
                waypoint.AddLink(prev);

            }

            Selection.activeGameObject = waypoint.gameObject;
            CheckNumOfDuplicatesMessage();
        }

        void DestroyDuplicates()
        {
            Waypoint[] points = (Waypoint[])GameObject.FindObjectsOfType(typeof(Waypoint));
            List<Waypoint> PointsToDestroy = new List<Waypoint>();
            Debug.Log("Start scanning waypints, total: " + points.Length);
            for (var i = 0; i < points.Length; i++)
            {
                for (var x = i + 1; x < points.Length; x++)
                {
                    //Debug.Log("X: " + x + " I: " + i);
                    //make sure we are not in destroy list if so just skip it
                    if (PointsToDestroy.Contains(points[x])) continue;

                    if (Vector3.Distance(points[i].transform.position,
                                         points[x].transform.position) < .001f)
                    {


                        // we have a duplicate
                        PointsToDestroy.Add(points[x]);
                        Debug.Log("Found duplicate: " + points[x].name);


                        //merge the neighbour links
                        for (int y = 0; y < points[x].neighbours.Count; y++)
                        {
                            Debug.Log(points[x].name);

                            if (points[x].neighbours[y].neighbour == null) continue;
                            Debug.Log("Neighbour: " + points[x].neighbours[y].neighbour.name);
                            points[i].AddLink(points[x].neighbours[y].neighbour);
                            Debug.Log("======");
                        }

                        Debug.Log("Checking if any other wp points to here");
                        //also make sure no wp link is pointing to this one
                        for (var z = 0; z < points.Length; z++)
                        {
                            for (var zz = 0; zz < points[z].neighbours.Count; zz++)
                            {
                                //see if any links point to this one, if so change the reference to first duplicate
                                if (points[z].neighbours[zz].neighbour == points[x])
                                {
                                    Debug.Log("Found non direct link: " + points[z].neighbours[zz].neighbour.name);
                                    //make sure this is not a point thats ment for desturction
                                    if (PointsToDestroy.Contains(points[z].neighbours[zz].neighbour))
                                        continue;

                                    //add this reference to first duplicate
                                    points[i].AddLink(points[z].neighbours[zz].neighbour);
                                }
                            }
                        }
                    }
                }
            }

            Debug.Log("Total duplicats found: " + PointsToDestroy.Count);
            //destroy the duplicates
            for (var ii = 0; ii < PointsToDestroy.Count; ii++)
            {
                DestroyImmediate(PointsToDestroy[ii]);
            }


            //lets go one more time through it all, and remove any links with no neighbour

            RemoveEmptyLinks();
            CheckNumOfDuplicatesMessage();

        }

        void RemoveEmptyLinks()
        {
            Waypoint[] points = (Waypoint[])GameObject.FindObjectsOfType(typeof(Waypoint));
            for (var i = 0; i < points.Length; i++)
            {
                for (var x = 0; x < points[i].neighbours.Count; x++)
                {
                    if (points[i].neighbours[x].neighbour == null)
                    {
                        points[i].neighbours.RemoveAt(x);
                    }
                }
            }
        }

        void CheckNumOfDuplicatesMessage()
        {
            Waypoint[] points = (Waypoint[])GameObject.FindObjectsOfType(typeof(Waypoint));
            int duplicates = 0;
            for (var i = 0; i < points.Length; i++)
            {
                for (var x = i + 1; x < points.Length; x++)
                {
                    if (Vector3.Distance(points[i].transform.position,
                                         points[x].transform.position) < .001f)
                    {
                        duplicates++;
                    }
                }
            }
            this.duplicates = duplicates;

        }
    }
}

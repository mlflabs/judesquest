using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Mlf.Traffic;

namespace Mlf.Traffic
{
    [InitializeOnLoad()]
    public class WaypointEditor : MonoBehaviour
    {





        [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
        public static void OnDrawSceneGizmo(Waypoint waypoint, GizmoType gizmoType)
        {
            if ((gizmoType & GizmoType.Selected) != 0)
            {
                Gizmos.color = Color.red;
            }
            else
            {
                Gizmos.color = Color.yellow;
            }

            Gizmos.DrawSphere(waypoint.transform.position, 0.25f);


            if (waypoint.neighbours == null) return;

            Gizmos.color = Color.blue;

            foreach (var link in waypoint.neighbours)
            {
                if (link != null && link.neighbour != null)
                {
                    Gizmos.DrawLine(waypoint.transform.position,
                        link.neighbour.transform.position);
                    //draw arrow

                    UtilGizmos.DrawArrowForGizmo(waypoint.transform.position,
                        (link.neighbour.transform.position - waypoint.transform.position).normalized,
                        0.1f, 20, 0.5f);
                    //Debug.Log(waypoint.name);
                    UtilGizmos.DrawArrowForGizmo(waypoint.transform.position,
                        (link.neighbour.transform.position - waypoint.transform.position).normalized,
                        0.1f, 20, 1f);

                }
            }



            if (WaypointEditor2.calculatedPath != null)
            {
                var arrayPath = WaypointEditor2.calculatedPath.ToArray();
                for (int i = 0; i < arrayPath.Length; i++)
                {
                    Gizmos.color = Color.green;
                    var pos = arrayPath[i].transform.position;
                    pos.y += 0.5f;
                    Gizmos.DrawSphere(pos, 0.08f);

                    //do we draw path
                    if (i + 1 < arrayPath.Length)
                    {
                        Gizmos.DrawLine(arrayPath[i].transform.position,
                            arrayPath[i + 1].transform.position);

                        UtilGizmos.DrawArrowForGizmo(arrayPath[i].transform.position,
                            (arrayPath[i + 1].transform.position - arrayPath[i].transform.position).normalized,
                            0.1f, 20, 0.5f);
                        UtilGizmos.DrawArrowForGizmo(arrayPath[i].transform.position,
                            (arrayPath[i + 1].transform.position - arrayPath[i].transform.position).normalized,
                            0.1f, 20, 1f);
                    }
                }
            }

            /*
            Gizmos.color = Color.white;
            Gizmos.DrawLine(waypoint.transform.position +
                (waypoint.transform.right * waypoint.width / 2f),
                waypoint.transform.position -
                (waypoint.transform.right * waypoint.width / 2f));

            if (waypoint.prevousWaypoint != null)
            {
                Gizmos.color = Color.red;
                Vector3 offset = waypoint.transform.right * waypoint.width / 2f;
                Vector3 offsetTo = waypoint.prevousWaypoint.transform.right
                    * waypoint.prevousWaypoint.width / 2f;

                Gizmos.DrawLine(waypoint.transform.position + offset,
                    waypoint.prevousWaypoint.transform.position + offsetTo);

            }

            if (waypoint.nextWaypoint != null)
            {
                Gizmos.color = Color.green;
                Vector3 offset = waypoint.transform.right * -waypoint.nextWaypoint.width / 2f;
                Vector3 offsetTo = waypoint.nextWaypoint.transform.right
                    * -waypoint.nextWaypoint.width / 2f;

                Gizmos.DrawLine(waypoint.transform.position + offset,
                    waypoint.nextWaypoint.transform.position + offsetTo);
            }

            */
        }

    }

}


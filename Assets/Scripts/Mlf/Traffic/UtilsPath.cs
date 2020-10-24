using UnityEngine;
using System.Collections;
using System.Collections.Generic;


namespace Mlf.Traffic
{
    public class UtilsPath
    {


        public static Waypoint FindClosestWaypoint(Vector3 target, List<Waypoint> wpList)
        {
            Waypoint closest = null;
            float closestDist = Mathf.Infinity;

            foreach (var point in wpList)
            {
                var dist = (point.transform.position - target).magnitude;
                //Debug.LogWarning(target);
                //Debug.Log(point.name + " == " + dist + "::: " + point.transform.position);
                if (dist < closestDist)
                {
                    closest = point;
                    closestDist = dist;
                }
            }

            if (closest != null)
            {
                return closest.GetComponent<Waypoint>();
            }
            return null;
        }

        public static Stack<Waypoint> FindPathToTarget(Waypoint startPoint, Waypoint endPoint)
        {

            //Debug.LogWarning("Finding path between:: " + startPoint.name + " and " + endPoint.name);
            SortedList<float, Waypoint> openList = new SortedList<float, Waypoint>();
            List<Waypoint> closedList = new List<Waypoint>();


            startPoint.previous = null;
            openList.Add(0, startPoint);



            Waypoint currentPoint = null;
            float distance = 0;
            float fulldistance = 0;

            while (openList.Count > 0)
            {
                currentPoint = openList.Values[0];

                if (currentPoint == endPoint)
                {
                    //Debug.Log("Found Path:: " + endPoint.name);
                    return CalculatePathStack(currentPoint);
                }


                distance = openList.Keys[0];// total distance from start to this node

                openList.RemoveAt(0);
                closedList.Add(currentPoint);

                for (int i = 0; i < currentPoint.neighbours.Count; i++)
                {
                    if (closedList.Contains(currentPoint.neighbours[i].neighbour) ||
                        openList.ContainsValue(currentPoint.neighbours[i].neighbour)) continue;


                    currentPoint.neighbours[i].neighbour.previous = currentPoint;

                    fulldistance = currentPoint.neighbours[i].GetDistance() +
                                    distance +
                                    (endPoint.transform.position -
                                     currentPoint.neighbours[i].neighbour.transform.position).magnitude;

                    if (openList.ContainsKey(fulldistance))
                    {
                        //if for some reason we have 2 exact distance waypoints
                        while (openList.ContainsKey(fulldistance))
                        {
                            fulldistance += 0.01f;
                        }
                    }

                    openList.Add(fulldistance, currentPoint.neighbours[i].neighbour);
                }
            }


            //Debug.Log("Found Partial Path: " + endPoint.name);

            return CalculatePartialPathStack(currentPoint, endPoint);
        }




        public static Stack<Waypoint> FindPathToTarget(Vector3 startPosition,
            Vector3 endPosition, List<Waypoint> wpList)
        {
            var startPoint = FindClosestWaypoint(startPosition, wpList);
            var endPoint = FindClosestWaypoint(endPosition, wpList);

            //if we can't find waypoint stop
            if (startPoint == null || endPoint == null)
            {
                Debug.LogWarning("Coudn't find a path between two endpoints:: "
                    + startPoint.name + ", " + endPoint.name);

                return null;
            }

            return FindPathToTarget(startPoint, endPoint);
        }



        private static Stack<Waypoint> CalculatePathStack(Waypoint point)
        {
            Stack<Waypoint> path = new Stack<Waypoint>();

            while (point.previous != null)
            {
                path.Push(point);
                point = point.previous;
            }
            path.Push(point); // add the last one

            return path;
        }

        private static Stack<Waypoint> CalculatePartialPathStack(Waypoint point, Waypoint endpoint)
        {

            //now see if any other node is closer than this one
            float closestDist = float.PositiveInfinity;
            Waypoint closestPoint = null;
            Waypoint currentPoint = point;
            var dist = float.PositiveInfinity;
            while (currentPoint.previous != null)
            {
                dist = (currentPoint.transform.position - endpoint.transform.position).magnitude;
                if (dist < closestDist)
                {
                    closestDist = dist;
                    closestPoint = currentPoint;
                }
                currentPoint = currentPoint.previous;
            }
            //also check the last point
            dist = (currentPoint.transform.position - endpoint.transform.position).magnitude;
            if (dist < closestDist)
            {
                closestPoint = currentPoint;
            }



            //we have the closest point, just add path to that one

            Stack<Waypoint> path = new Stack<Waypoint>();
            // go back to the closes point
            while (point != closestPoint)
            {
                point = point.previous;
            }
            // then from the closes point calculate path to begging
            while (point.previous != null)
            {
                path.Push(point);
                point = point.previous;
            }
            path.Push(point); // add the last one

            //Debug.Log("Waypoint Count:: " + path.ToArray().Length);
            return path;
        }







    }

}

using UnityEngine;
using System.Collections.Generic;
using System;

namespace Mlf.Traffic
{
    public class PathManager : MonoBehaviour
    {
        public static PathManager instance;

        [SerializeField] private List<Waypoint> _waypoints = new List<Waypoint>();

        [SerializeField]
        private Dictionary<WaypointTypes, List<Waypoint>> _waypointTypeDict =
            new Dictionary<WaypointTypes, List<Waypoint>>();

        void Awake()
        {

            if (instance == null)
            {

                instance = this;
                //DontDestroyOnLoad(this.gameObject);

                //Rest of your Awake code

            }
            else
            {
                Destroy(this);
            }

            //initialize our dict
            foreach (WaypointTypes type in (WaypointTypes[])Enum.GetValues(typeof(WaypointTypes)))
            {
                _waypointTypeDict[type] = new List<Waypoint>();
            }
        }




        public Waypoint FindClosestWaypoint(Vector3 pos)
        {

            return UtilsPath.FindClosestWaypoint(pos, _waypoints);
        }



        public Stack<Waypoint> GetRandomLocationPath(Vector3 pos)
        {
            Waypoint wp = FindClosestWaypoint(pos);
            //Debug.Log(wp.name);
            return GetRandomLocationPath(wp);
        }

        public Stack<Waypoint> GetRandomLocationPath(Waypoint wp)
        {
            return UtilsPath.FindPathToTarget(wp, GetRandomPoint());
        }

        public Waypoint GetRandomPoint()
        {
            Debug.Log(_waypointTypeDict[WaypointTypes.destination].Count);
            return _waypointTypeDict[WaypointTypes.destination]
                [UnityEngine.Random.Range(0, _waypointTypeDict[WaypointTypes.destination].Count)];
        }





        public Stack<Waypoint> FindPathToTarget(Vector3 startPosition, Vector3 endPosition)
        {
            var startPoint = FindClosestWaypoint(startPosition);
            var endPoint = FindClosestWaypoint(endPosition);

            //if we can't find waypoint stop
            if (startPoint == null || endPoint == null)
            {
                Debug.LogWarning("Coudn't find a path between two endpoints:: "
                    + startPoint.name + ", " + endPoint.name);

                return null;
            }

            return FindPathToTarget(startPoint, endPoint);
        }


        public Stack<Waypoint> FindPathToTarget(Waypoint startPoint, Waypoint endPoint)
        {
            return UtilsPath.FindPathToTarget(startPoint, endPoint);
        }









        public void AddWaypoint(Waypoint wp)
        {
            if (_waypoints.Contains(wp)) return;

            _waypoints.Add(wp);
            _waypointTypeDict[wp.type].Add(wp);
        }

        public void RemoveWaypoint(Waypoint wp)
        {
            _waypoints.Remove(wp);
            _waypointTypeDict[wp.type].Remove(wp);
        }



    }

}

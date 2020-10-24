using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



namespace Mlf.Traffic
{
    [Serializable]
    public class WaypointLink
    {
        public Waypoint start;
        public Waypoint neighbour;
        private float distance = 0;




        public WaypointLink(Waypoint start, Waypoint neighbour)
        {
            this.start = start;
            this.neighbour = neighbour;
            GetDistance();
        }

        public float GetDistance()
        {
            if (distance != 0) return distance;

            if (start == null || neighbour == null) return 0;

            distance = (start.transform.position - neighbour.transform.position).magnitude;
            return distance;
        }

    }
}


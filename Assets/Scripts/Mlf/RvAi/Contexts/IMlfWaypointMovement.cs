// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System.Collections.Generic;
using Mlf.Traffic;
using UnityEngine;

namespace Mlf.RvAi.Contexts
{
    public interface IMlfWaypointMovement
    {

        /// <summary>
        /// 
        /// </summary>
        bool AtDestination { get; }

        /// <summary>
        /// Sets current destination and automatically starts moving towards it
        /// Should be set to Vector3.zero after arriving
        /// </summary>
        Waypoint Destination { get; set; }

        Vector3 Velocity { get; }

        /// <summary>
        /// Current position
        /// </summary>
        Vector3 Position { get; }

        Quaternion Rotation { get; }

        //Waypoint Functions
        void LoadNewPath(Stack<Waypoint> path);


        //Dialouge Target
        Vector3 target { get; set; }


        void LoadNextWaypoint();
    }
}
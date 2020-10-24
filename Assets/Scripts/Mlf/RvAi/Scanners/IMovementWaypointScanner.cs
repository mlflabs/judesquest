// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System.Collections.Generic;
using Mlf.Traffic;
using UnityEngine;

namespace Mlf.RvAi.Scanners
{
    public interface IMovementWaypointScanner
    {
        #region Public methods

        Stack<Waypoint> FindWaypointTarget(Vector3 _position, float _range);
        Stack<Waypoint> FindWaypointTarget(Waypoint _position, float _range);

        #endregion
    }
}
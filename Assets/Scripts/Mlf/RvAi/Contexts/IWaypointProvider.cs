// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System.Collections.Generic;
using Mlf.Traffic;
using UnityEngine;

namespace Mlf.RvAi.Contexts
{
    public interface IWaypointProvider
    {
        #region Properties

        //Waypoint GetWaypointPosition(int _id);
        int WaypointsCount { get; }

        #endregion
    }
}
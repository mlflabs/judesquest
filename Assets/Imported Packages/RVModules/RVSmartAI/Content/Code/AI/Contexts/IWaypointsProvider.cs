// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System.Collections.Generic;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Contexts
{
    public interface IWaypointsProvider
    {
        #region Properties

        Vector3 GetWaypointPosition(int _id);
        int WaypointsCount { get; }

        #endregion
    }
}
// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System.Collections.Generic;
using RVModules.RVSmartAI.Content.Code.AI.Contexts;
using RVModules.RVSmartAI.Content.Code.Movements;
using RVModules.RVSmartAI.Content.Code.Scanners;
using RVModules.RVSmartAI.GraphElements;
using RVModules.RVUtilities;

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks.TasksParams
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AiAgentBaseTaskParams<T> : AiTaskParams<T>
    {
        #region Fields

        protected IMovement movement;
        protected IMovementScanner movementScanner;
        protected IEnvironmentScanner environmentScanner;

        private INearbyObjectsProvider nearbyObjectsProvider;
        private IMoveTargetProvider moveTargetProvider;
        private IWaypointsProvider waypointsProvider;

        #endregion

        #region Properties

        protected Transform MoveTarget => moveTargetProvider.FollowTarget;
        protected ListNonAlloc<Object> NearbyObjects => nearbyObjectsProvider.NearbyObjects;

        #endregion

        #region Not public methods

        protected override void OnContextUpdated()
        {
            movement = (Context as IMovementProvider)?.Movement;
            movementScanner = (Context as IMovementScannerProvider)?.MovementScanner;
            environmentScanner = (Context as IEnvironmentScannerProvider)?.EnvironmentScanner;
            moveTargetProvider = Context as IMoveTargetProvider;
            nearbyObjectsProvider = Context as INearbyObjectsProvider;
            waypointsProvider = Context as IWaypointsProvider;
        }

        #endregion
    }
}
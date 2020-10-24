// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.RvAi.Components;
using Mlf.RvAi.Contexts;
using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    /// <summary>
    /// Returns set score if AIAgent is at destination or if it isn't, depending on 'not' field
    /// </summary>
    public class MlfAtDestinationScorer : AiScorer
    {
        #region Fields

        [SmartAiExposeField]
        public bool trueIfNotAtDestination;


        protected IMlfWaypointMovement movement;
        #endregion

        #region Public methods

        public override float Score(float _deltaTime)
        {
            //Debug.Log("At Destination Scorer");

            if (!trueIfNotAtDestination && movement.AtDestination) return score;
            if (trueIfNotAtDestination && !movement.AtDestination) return score;

            return 0;
        }

        #endregion

        #region Not public methods

        protected override void OnContextUpdated()
        {
            movement = (Context as IMlfWaypointMovementProvider)?.Movement;
            //movementScanner = (Context as IMovementScannerProvider)?.MovementScanner;
            //environmentScanner = (Context as IEnvironmentScannerProvider)?.EnvironmentScanner;
            //moveTargetProvider = Context as IMoveTargetProvider;
            //nearbyObjectsProvider = Context as INearbyObjectsProvider;
            //waypointsProvider = Context as IWaypointsProvider;
        }

        #endregion
    }
}
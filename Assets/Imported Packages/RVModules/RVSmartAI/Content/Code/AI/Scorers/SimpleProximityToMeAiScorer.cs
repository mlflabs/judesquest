// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVUtilities.Extensions;

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public abstract class SimpleProximityToMeAiScorer : AiAgentBaseScorer
    {
        #region Properties

        protected abstract Vector3 PositionToMeasure { get; }

        #endregion

        #region Public methods

        public override float Score(float _deltaTime)
        {
            return PositionToMeasure.ManhattanDistance2d(movement.Position) * score;
        }

        #endregion
    }
}
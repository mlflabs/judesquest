// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVUtilities.Extensions;

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers.ScorersParams
{
    public abstract class SimpleProximityAiScorerParams : AiAgentScorerParams<Vector3>
    {
        #region Properties

        protected abstract Vector3 PositionToMeasure { get; }

        #endregion

        #region Public methods

        protected override float Score(Vector3 _parameter)
        {
            return PositionToMeasure.ManhattanDistance2d(_parameter) * score;
        }

        #endregion
    }
}
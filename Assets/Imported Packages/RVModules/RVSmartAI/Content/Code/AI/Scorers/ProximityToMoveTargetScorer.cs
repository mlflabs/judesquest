// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class ProximityToMoveTargetScorer : ProximityToMeAiScorer
    {
        #region Properties

        public override Vector3 PositionToMeasure => MoveTarget.position;

        #endregion
    }
}
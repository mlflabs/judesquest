﻿// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers.ScorersParams
{
    public class SimpleProximityToMoveTargetAiScorerParams : SimpleProximityAiScorerParams
    {
        #region Properties

        protected override Vector3 PositionToMeasure => MoveTarget.position;

        #endregion
    }
}
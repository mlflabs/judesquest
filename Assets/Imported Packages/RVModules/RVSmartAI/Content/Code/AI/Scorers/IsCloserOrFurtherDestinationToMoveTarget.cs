// Created by Ronis Vision. All rights reserved
// 10.07.2020.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class IsCloserOrFurtherDestinationToMoveTarget : IsCloserOrFurtherThanAiScorer
    {
        #region Properties

        protected override Vector3 PositionToMeasure => MoveTarget.position;
        protected override Vector3 SecondPositionToMeasure => movement.Destination;

        #endregion
    }
}
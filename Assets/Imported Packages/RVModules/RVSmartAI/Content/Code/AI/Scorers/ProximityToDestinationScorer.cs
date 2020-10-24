// Created by Ronis Vision. All rights reserved
// 03.06.2020.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class ProximityToDestinationScorer : ProximityToMeAiScorer
    {
        public override Vector3 PositionToMeasure
        {
            get
            {
                if (movement.AtDestination) return movement.Position;
                return movement.Destination;
            }
        }
    }
}
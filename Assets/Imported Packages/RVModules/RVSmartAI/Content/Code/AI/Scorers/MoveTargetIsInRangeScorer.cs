// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class MoveTargetIsInRangeScorer : AiAgentBaseScorer
    {
        #region Public methods

        public override float Score(float _deltaTime)
        {
            foreach (var cNearbyObject in NearbyObjects.Array)
            {
                var comp = cNearbyObject as Component;
                if (comp == null) continue;

                if (comp.transform == MoveTarget)
                    return score;
            }

            return 0;
        }

        #endregion
    }
}
// Created by Ronis Vision. All rights reserved
// 23.08.2019.

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class MoveTargetIsNullScorer : AiAgentBaseScorer
    {
        #region Fields

        [SmartAiExposeField]
        public bool not;

        #endregion

        #region Public methods

        public override float Score(float _deltaTime)
        {
            if (not && MoveTarget != null) return score;
            if (!not && MoveTarget == null) return score;
            return 0;
        }

        #endregion
    }
}
// Created by Ronis Vision. All rights reserved
// 23.08.2019.

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    /// <summary>
    /// Returns set score if AIAgent is at destination or if it isn't, depending on 'not' field
    /// </summary>
    public class AtDestinationScorer : AiAgentBaseScorer
    {
        #region Fields

        [SmartAiExposeField]
        public bool not;

        #endregion

        #region Public methods

        public override float Score(float _deltaTime)
        {
            if (!not && movement.AtDestination) return score;
            if (not && !movement.AtDestination) return score;

            return 0;
        }

        #endregion
    }
}
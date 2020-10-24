// Created by Ronis Vision. All rights reserved
// 23.08.2019.

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    /// <summary>
    /// 
    /// </summary>
    public class IsMovingAiScorer : AiAgentBaseScorer
    {
        #region Fields

        [SmartAiExposeField]
        public float scoreNotMoving;

        #endregion

        #region Public methods

        public override float Score(float _deltaTime) => movement.Velocity.magnitude > .15f ? score : scoreNotMoving;

        #endregion
    }
}
// Created by Ronis Vision. All rights reserved
// 23.08.2019.

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks
{
    public class MoveToMoveTarget : AiAgentBaseTask
    {
        #region Public methods

        protected override void Execute(float _deltaTime)
        {
            movement.Destination = MoveTarget.position;
        }

        #endregion
    }
}
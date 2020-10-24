// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVSmartAI.Content.Code.Movements;

namespace RVModules.RVSmartAI.Content.Code.AI.Contexts
{
    public interface IMovementProvider
    {
        #region Properties

        IMovement Movement { get; }

        #endregion
    }
}
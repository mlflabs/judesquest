// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVSmartAI.Content.Code.Scanners;

namespace RVModules.RVSmartAI.Content.Code.AI.Contexts
{
    public interface IMovementScannerProvider
    {
        #region Properties

        IMovementScanner MovementScanner { get; }

        #endregion
    }
}
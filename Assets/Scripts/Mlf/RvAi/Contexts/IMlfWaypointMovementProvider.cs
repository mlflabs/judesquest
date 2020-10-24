// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using RVModules.RVSmartAI.Content.Code.Movements;

namespace Mlf.RvAi.Contexts
{
    public interface IMlfWaypointMovementProvider
    {
        IMlfWaypointMovement Movement { get; }
    }
}
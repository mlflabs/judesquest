// Created by Ronis Vision. All rights reserved
// 11.07.2020.

using RVModules.RVSmartAI.Content.Code.AI.Contexts;
using UnityEngine;

namespace RVModules.RVSmartAI.DataProviderPrototype
{
    public class MyPositionProvider : PositionProvider
    {
        public override Vector3 GetData(IContext _context) => Context<IMovementProvider>(_context).Movement.Position;
    }

    public class DestinationProvider : PositionProvider
    {
        public override Vector3 GetData(IContext _context) => Context<IMovementProvider>(_context).Movement.Destination;
    }

    public class MoveTargetPositionProvider : PositionProvider
    {
        public override Vector3 GetData(IContext _context) => Context<IMoveTargetProvider>(_context).FollowTarget.position;
    }
}
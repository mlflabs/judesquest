// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.Characters;
using Mlf.RvAi.Contexts;
using Mlf.Traffic;
using RVModules.RVSmartAI;
using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace Mlf.RvAi.Tasks
{
    public class MlfTaskGetNewDestinationFromScheduleEvent : AiTask
    {

        protected IMlfWaypointMovement movement;
        protected CharacterContext characterContext;



        protected override void Execute(float _deltaTime)
        {

            Debug.Log("New Schedule Destination:: " + characterContext);
            if (movement == null)
                Debug.LogWarning("Movement is null");
            if (movement == null) return;

            movement.LoadNewPath(UtilsPath.FindPathToTarget(
                characterContext.getCurrentLocation(),
                characterContext.getNextEventLocation()));

            characterContext.actions.Peek().state = ActionState.InProgress;
        }



        protected override void OnContextUpdated()
        {

            var test = (Context as IMlfWaypointMovementProvider);
            movement = (Context as IMlfWaypointMovementProvider)?.Movement;
            characterContext = (Context as ICharacterContextProvider)?.characterContext;
        }
    }


}
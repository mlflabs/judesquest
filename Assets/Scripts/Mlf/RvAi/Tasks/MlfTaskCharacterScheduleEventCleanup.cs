// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.City;
using Mlf.Characters;
using Mlf.RvAi.Contexts;
using Mlf.Traffic;
using RVModules.RVSmartAI;
using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace Mlf.RvAi.Tasks
{
    public class MlfTaskCharacterScheduleEventCleanup : AiTask
    {

        protected IMlfWaypointMovement movement;
        protected CharacterContext characterContext;



        protected override void Execute(float _deltaTime)
        {

            Debug.Log("Schedule Event Cleanup:: " + characterContext);

            if (characterContext == null) return;

            //remove the last action, see if we have anything else
            var state = characterContext.actions.Dequeue();

            characterContext.currentLocation = state.action.destination;


            if (characterContext.actions.Count > 0)
                return;

            //if we are here, that means we are at destination, and we
            //have no more actions left, remove from board;
            CityManager.instance.destroyCharacter(characterContext);

        }



        protected override void OnContextUpdated()
        {
            movement = (Context as IMlfWaypointMovementProvider)?.Movement;
            characterContext = (Context as ICharacterContextProvider)?.characterContext;
        }
    }


}
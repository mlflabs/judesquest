// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.RvAi.Contexts;
using Mlf.Traffic;
using RVModules.RVSmartAI;
using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace Mlf.RvAi.Tasks
{
    public class MlfWaypointLoadNextDestinationWaypoint : AiTask
    {




        protected IMlfWaypointMovement movement;



        protected override void Execute(float _deltaTime)
        {
            Debug.Log("Excecuing GoToNextWaypoint");
            movement.LoadNextWaypoint();
        }


        protected override void OnContextUpdated()
        {
            movement = (Context as IMlfWaypointMovementProvider)?.Movement;
        }
    }


}
// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.RvAi.Contexts;
using Mlf.Traffic;
using RVModules.RVSmartAI;
using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace Mlf.RvAi.Tasks
{
    public class MlfGoToNextWaypoint : AiTask
    {
        #region Fields

        [SmartAiExposeField("Loop means after reaching last WP it move to 0.")]
        public bool loop = true;

        [SmartAiExposeField("Select random waypoint")]
        public bool selectRandomWaypoint = false;

        [SmartAiExposeField("Just for info")]
        public int currentWaypoint;

        private bool goingUp = true;

        #endregion

        #region Context Properties
        protected IMlfWaypointMovement movement;

        #endregion

        #region Public methods

        protected override void Execute(float _deltaTime)
        {
            Debug.Log("Excecuing GoToNextWaypoint");
            movement.LoadNewPath(PathManager.instance.GetRandomLocationPath(movement.Position));


            /*
            if (waypointsProvider.WaypointsCount == 0) return;

            if (selectRandomWaypoint)
            {
                currentWaypoint = Random.Range(0, waypointsProvider.WaypointsCount);
                movement.Destination = waypointsProvider.GetWaypointPosition(currentWaypoint);
                return;
            }

            if (loop)
            {
                if (currentWaypoint >= waypointsProvider.WaypointsCount)
                    currentWaypoint = 0;
            }
            else
            {
                if (currentWaypoint >= waypointsProvider.WaypointsCount)
                {
                    goingUp = false;
                    currentWaypoint = waypointsProvider.WaypointsCount - 2;
                }

                if (currentWaypoint <= 0 && !goingUp)
                {
                    goingUp = true;
                    currentWaypoint = 0;
                }
            }

            movement.Destination = waypointsProvider.GetWaypointPosition(currentWaypoint);

            if (loop) currentWaypoint++;
            else
            {
                if (goingUp) currentWaypoint++;
                else currentWaypoint--;
            }*/
        }

        #endregion

        protected override void OnContextUpdated()
        {
            movement = (Context as IMlfWaypointMovementProvider)?.Movement;
        }
    }


}
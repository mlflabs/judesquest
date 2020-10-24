// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.RvAi.Contexts;
using Mlf.Traffic;
using RVModules.RVSmartAI;
using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace Mlf.RvAi.Tasks
{
    public class MlfDialogueFacePlayerTask : AiTask
    {
        #region Fields

        #endregion

        #region Context Properties
        protected IMlfWaypointMovement movement;

        #endregion

        #region Public methods

        protected override void Execute(float _deltaTime)
        {

        }

        #endregion

        protected override void OnContextUpdated()
        {
            movement = (Context as IMlfWaypointMovementProvider)?.Movement;
        }




    }


}
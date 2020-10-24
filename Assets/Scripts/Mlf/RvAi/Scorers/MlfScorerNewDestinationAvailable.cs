// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.City;
using Mlf.Characters;
using Mlf.RvAi.Components;
using Mlf.RvAi.Contexts;
using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    /// <summary>
    /// Returns set score if AIAgent is at destination or if it isn't, depending on 'not' field
    /// </summary>
    public class MlfScorerNewDestinationAvailable : AiScorer
    {

        protected CharacterContext characterContext;




        public override float Score(float _deltaTime)
        {
            //Debug.Log("Destination Available:: " + characterContext);
            if (!CityManager.instance.canAddCharacterToStage())
                return 0;//no room, wait untill some characters are removed

            if (characterContext == null) return 0;


            if (characterContext.actions.Peek().state == ActionState.Added)
                return score;

            return 0;
        }



        #region Not public methods

        protected override void OnContextUpdated()
        {
            characterContext = (Context as ICharacterContextProvider)?.characterContext;

            //Debug.Log("ONUPDATE:::::DestinaitonAVAILABLE:: " + characterContext);
            //Debug.Log(Context);
            //movementScanner = (Context as IMovementScannerProvider)?.MovementScanner;
            //environmentScanner = (Context as IEnvironmentScannerProvider)?.EnvironmentScanner;
            //moveTargetProvider = Context as IMoveTargetProvider;
            //nearbyObjectsProvider = Context as INearbyObjectsProvider;
            //waypointsProvider = Context as IWaypointsProvider;
        }

        #endregion
    }
}
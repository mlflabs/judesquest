// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.Characters;
using Mlf.Dialogue;
using Mlf.RvAi.Components;
using Mlf.RvAi.Contexts;
using RVModules.RVSmartAI.GraphElements;
using UnityEngine;

namespace Mlf.RvAi.Scorers
{
    /// <summary>
    /// Returns set score if AIAgent is at destination or if it isn't, depending on 'not' field
    /// </summary>
    public class MlfDialogueStateScorer : AiScorer
    {
        #region Fields

        protected DialogueNPCCmp dialogue;
        #endregion

        #region Public methods

        public override float Score(float _deltaTime)
        {
            if (dialogue.inDialogue) return score;

            return 0;
        }

        #endregion

        #region Not public methods

        protected override void OnContextUpdated()
        {
            dialogue = (Context as IMlfDialogueCmpProvider)?.DialogueCmp;
        }

        #endregion
    }
}
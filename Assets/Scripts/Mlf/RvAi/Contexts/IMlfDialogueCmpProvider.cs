// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using Mlf.Dialogue;
using Mlf.RvAi.Components;
using RVModules.RVSmartAI.Content.Code.Movements;

namespace Mlf.RvAi.Contexts
{
    public interface IMlfDialogueCmpProvider
    {
        #region Properties

        DialogueNPCCmp DialogueCmp { get; }

        #endregion
    }
}
// Created by Ronis Vision. All rights reserved
// 31.12.2019.

namespace RVModules.RVSmartAI.Content.Code.AI.Tasks
{
    public class SetString : SetProperty
    {
        #region Fields

        [SmartAiExposeField]
        public string value;

        #endregion

        #region Properties

        protected override object Value => value;

        #endregion
    }
}
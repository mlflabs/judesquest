// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System;

namespace RVModules.RVSmartAI
{
    public class SmartAiExposeField : Attribute
    {
        #region Fields

        public string description;

        #endregion

        public SmartAiExposeField()
        {
        }

        public SmartAiExposeField(string _description)
        {
            description = _description;
        }
    }
}
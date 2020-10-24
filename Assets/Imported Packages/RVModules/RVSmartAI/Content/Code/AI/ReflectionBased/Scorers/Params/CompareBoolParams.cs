// Created by Ronis Vision. All rights reserved
// 01.01.2020.

using System;
using RVModules.RVSmartAI.RpgPack;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class CompareBoolParams : GetPropertyParams<object>
    {
        [SmartAiExposeField]
        public bool value;

        [SmartAiExposeField]
        public float scoreFalse;

        protected override float Score(object _parameter)
        {
            var v = GetPropertyValue(_parameter);
            if (v == null) return 0;
            return (bool)v == value ? score : scoreFalse;
        }
    }
}
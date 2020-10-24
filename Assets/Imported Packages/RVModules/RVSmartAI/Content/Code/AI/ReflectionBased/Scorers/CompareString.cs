// Created by Ronis Vision. All rights reserved
// 01.01.2020.

using System;
using RVModules.RVSmartAI.RpgPack;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Scorers
{
    public class CompareString : GetProperty
    {
        [SmartAiExposeField]
        public string value;

        [SmartAiExposeField]
        public float scoreFalse;

        public override float Score(float _deltaTime)
        {
            string s = GetPropertyValue.ToString();

            return s == value ? score : scoreFalse;
        }
    }
}
// Created by Ronis Vision. All rights reserved
// 11.07.2020.

using RVModules.RVSmartAI.Content.Code.AI.Scorers;
using UnityEngine;

namespace RVModules.RVSmartAI.DataProviderPrototype
{
    public class IsCloserOrFurtherThanAiScorerDataProviders : IsCloserOrFurtherThanAiScorer
    {
        [SmartAiExposeField]
        public PositionProvider firstPosProvider;
        
        [SmartAiExposeField]
        public PositionProvider secondPosProvider;   
        
        //[SmartAiExposeField]
        //public DataProvider<Vector3> thirdPosProvider;

        protected override Vector3 SecondPositionToMeasure => secondPosProvider.GetData(Context);
        protected override Vector3 PositionToMeasure => firstPosProvider.GetData(Context);
    }
}
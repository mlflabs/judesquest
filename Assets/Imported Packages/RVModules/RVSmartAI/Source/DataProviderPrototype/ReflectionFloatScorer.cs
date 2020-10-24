// Created by Ronis Vision. All rights reserved
// 11.07.2020.

using RVModules.RVSmartAI.GraphElements;

namespace RVModules.RVSmartAI.DataProviderPrototype
{
    public class ReflectionFloatScorer: AiScorer
    {
        [SmartAiExposeField]
        public FloatProviderReflection scoreProvider;
        
        public override float Score(float _deltaTime) => scoreProvider.GetData(Context);
    }
}
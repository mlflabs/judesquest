// Created by Ronis Vision. All rights reserved
// 26.03.2019.

using System;
using RVModules.RVSmartAI.GraphElements;

namespace RVModules.RVSmartAI.Editor.SelectWindows
{
    public class SelectScorerWindow : SelectWindowBase<AiScorer>
    {
        protected override string Title => "Select AiScorer";
        //protected override Type GetWindowType() => GetType();
    }
    
//    public class SelectScorerParamsWindow : SelectWindowBase<AiScorerParams>
//    {
//        protected override string Title => "Select scorer";
//    }
}
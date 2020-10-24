// Created by Ronis Vision. All rights reserved
// 05.01.2020.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RVModules.RVSmartAI.Content.Code;
using UnityEditor;
using UnityEngine;

namespace RVModules.RVSmartAI.GraphElements
{
    /// <summary>
    /// Base class for AiTask
    /// </summary>
    public abstract class AiTask : AiGraphElement
    {
        #region Not public methods

        protected abstract void Execute(float _deltaTime);

        internal void Exec(float _deltaTime) => Execute(_deltaTime);
        
        /// <summary>
        /// for debugging only
        /// </summary>
        public float lastScore;

        protected float Score(float _deltaTime)
        {
            float score = 0;
            for (var i = 0; i < taskScorers.Count; i++)
            {
                var scorer = taskScorers[i];
                if (scorer == null || !scorer.Enabled) continue;
                var s = scorer.lastScore = scorer.Score(_deltaTime);
                switch (scorer.scorerType)
                {
                    case ScorerType.Add:
                        score += s;
                        break;
                    case ScorerType.Subtract:
                        score -= s;
                        break;
                    case ScorerType.Multiply:
                        score *= s;
                        break;
                    case ScorerType.Divide:
                        score /= s;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            lastScore = score;
            return score;
        }

        #endregion
        
        #region Fields

        public List<AiScorer> taskScorers = new List<AiScorer>();

        #endregion

        #region Properties

        public override IList ChildGraphElements => taskScorers;

        #endregion

        public override Type[] GetAssignableSubElementTypes() => new[] {typeof(AiScorer)};

        public override IAiGraphElement[] GetChildGraphElements()
        {
            var list = new List<IAiGraphElement>() {this};
            list.AddRange(taskScorers);
            return list.ToArray();
        }

        public override IAiGraphElement[] GetAllGraphElements()
        {
            var list = new List<IAiGraphElement>() {this};
            foreach (var aiGraphElement in taskScorers)
            {
                list.AddRange(aiGraphElement.GetAllGraphElements());
            }

            return list.ToArray();
        }

        public override void RemoveNulls()
        {
            base.RemoveNulls();
            taskScorers = taskScorers.Where(_scorer => _scorer != null).ToList();
        }

        public override void AssignSubSelement(IAiGraphElement _aiGraphElement)
        {
            var s = _aiGraphElement as AiScorer;
            if (s == null)
            {
                Debug.LogError("Fail!");
                base.AssignSubSelement(_aiGraphElement);
                return;
            }

            s.AiGraph = AiGraph;
            taskScorers.Add(s);
            base.AssignSubSelement(_aiGraphElement);
        }

#if UNITY_EDITOR
        private void Awake() => Reset();

        private void Reset()
        {
            if (string.IsNullOrEmpty(Name)) Name = ObjectNames.NicifyVariableName(ToString());
            // remove null entries
            taskScorers = taskScorers.Where(_s => _s != null).ToList();
        }
#endif
    }
}
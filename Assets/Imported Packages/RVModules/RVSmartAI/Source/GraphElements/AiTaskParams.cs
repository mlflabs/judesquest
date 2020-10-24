// Created by Ronis Vision. All rights reserved
// 05.01.2020.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using RVModules.RVSmartAI.Content.Code;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RVModules.RVSmartAI.GraphElements
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AiTaskParams<T> : AiTask, IAiTaskParams
    {
        #region Fields

        public List<AiGraphElement> scorers = new List<AiGraphElement>();

        [SmartAiExposeField]
        public bool debugValues;

        #endregion

        #region Properties

        public override IList ChildGraphElements => scorers;

        #endregion

        protected AiTaskParams() => scorers = scorers.Where(_s => _s != null).ToList();

        #region Public methods
        
        public override void RemoveNulls()
        {
            base.RemoveNulls();
            scorers = scorers.Where(_scorer => _scorer != null).ToList();
        }

        public override Type[] GetAssignableSubElementTypes() => new[] {typeof(IAiScorer), typeof(AiScorerParams<T>)};

        public override IAiGraphElement[] GetChildGraphElements()
        {
            var list = new List<IAiGraphElement>() {this};
            list.AddRange(scorers);
            return list.ToArray();
        }

        public override IAiGraphElement[] GetAllGraphElements()
        {
            var list = new List<IAiGraphElement>() {this};
            foreach (var aiGraphElement in scorers)
            {
                if (aiGraphElement == null) continue;
                list.AddRange(aiGraphElement.GetAllGraphElements());
            }

            return list.ToArray();
        }

        public override void AssignSubSelement(IAiGraphElement _aiGraphElement)
        {
            var s = _aiGraphElement as AiGraphElement;
            if (s == null)
            {
                Debug.LogError("Fail!");
                base.AssignSubSelement(_aiGraphElement);
                return;
            }

            s.AiGraph = AiGraph;
            scorers.Add(s);
            base.AssignSubSelement(_aiGraphElement);
        }

        #endregion

        #region Not public methods

        List<object> IAiTaskParams.GetScorers() => scorers.Cast<object>().ToList();

        void IAiTaskParams.SetScorers(List<object> _scorers) => scorers = _scorers.Cast<AiGraphElement>().ToList();

        /// <summary>
        /// Returns best match using added scorers
        /// </summary>
        /// <param name="_parameters"></param>
        /// <returns></returns>
        protected virtual T GetBest(T[] _parameters)
        {
            var highestScore = float.MinValue;
            T highestParam = default;

            for (var index = 0; index < _parameters.Length; index++)
            {
                var parameter = _parameters[index];
                if (parameter == null) continue;
                var unityObject = parameter as Object;
                if ((object)unityObject != null)
                {
                    if (unityObject == null) continue;
                    //Debug.Log("hehe");
                }

                float score = 0;
#if UNITY_EDITOR
                for (var i = 0; i < scorers.Count; i++)
                {
                    var scorer = scorers[i] as IAiScorer;
                    if (scorer == null || !scorer.Enabled) continue;
                    var s = scorer.Score_(parameter);
                    switch (scorer.ScorerType)
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

                if (debugValues) AiGraph.AddDebugValue(parameter, score);
#else
                for (var i = 0; i < scorers.Count; i++)
                {
                    var scorer = scorers[i] as IAiScorer;
                    if (scorer == null || !scorer.Enabled) continue;
                    var s = scorer.Score_(parameter);
                    switch (scorer.ScorerType)
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
#endif

                if (!(score > highestScore)) continue;
                highestParam = parameter;
                highestScore = score;
            }

            return highestParam;
        }

        #endregion
    }
}
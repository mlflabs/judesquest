// Created by Ronis Vision. All rights reserved
// 16.01.2020.

using System.Linq;
using RVLoadBalancer;
using UnityEngine;

namespace RVModules.RVSmartAI
{
    /// <summary>
    /// AI component that is added to gameObject that needs AI, provides reference to AiGraph
    /// </summary>
    public class Ai : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        private AiGraph aiGraph;

        [SerializeField]
        private AiGraph[] secondaryGraphs;

        [SerializeField]
        private AiGraph[] instantiatedSecondaryGraphs;

        /// <summary>
        /// Assign any Unity.Object that implements IContextProvider
        /// </summary>
        [SerializeField]
        private Object contextProvider;

        // debug info

        [SerializeField]
        private int graphStepsPerUpdate = 9999;

        [SerializeField]
        private bool dontHideInHierarchy;

        [SerializeField]
        private int updateFrequency = 2;

        #endregion

        #region Properties

        /// <summary>
        /// Can be safely changed at runtime
        /// </summary>
        public int UpdateFrequency
        {
            get => updateFrequency;
            set
            {
                if (!Application.isPlaying)
                {
                    Debug.LogError("Can't change update frequency in edit mode!");
                    return;
                }

                updateFrequency = value;
                if (!enabled || !IsInitialized) return;
                if (LoadBalancerSingleton.Instance == null) return;
                // new lb version handes re-registering automatically
                //LoadBalancerSingleton.Instance.Unregister(this, Tick);
                RegisterGraphUpdateLoop();
            }
        }

        private void RegisterGraphUpdateLoop()
        {
            if (!Application.isPlaying) return;

            LoadBalancerSingleton.Instance.Register(this, Tick, updateFrequency);
            aiGraph.UpdateFrequency = updateFrequency;
        }

        public AiGraph AiGraph
        {
            get
            {
                if (Application.isPlaying && !IsInitialized)
                {
                    Debug.LogError("Accessing AiGraph before it's instantation is not allowed to avoid accidental change of AiGraph prefab data!");
                    return null;
                }

                return aiGraph;
            }
        }

        /// <summary>
        /// Returns copy of instantiated secondary graphs
        /// </summary>
        public AiGraph[] SecondaryGraphs => instantiatedSecondaryGraphs.ToArray();

        public string CurrentNode { get; private set; }

        public string LastUtility { get; private set; }

        public string LastTask { get; private set; }

        public bool IsInitialized { get; protected set; }

        #endregion

        #region Not public methods

        protected virtual void Awake()
        {
            if (aiGraph == null)
            {
                Debug.LogError($"You need to assign {typeof(AiGraph)}!", this);
                return;
            }

            if (contextProvider as IContextProvider == null)
            {
                Debug.LogError("Coudln't get IContextProvider from assigned contexProvider object!", this);
                return;
            }

            // Create own copy of Ai graphs
            aiGraph = AiGraph.CreateAndInitializeGraph(dontHideInHierarchy, graphStepsPerUpdate, gameObject.name, aiGraph);
            instantiatedSecondaryGraphs = new AiGraph[secondaryGraphs.Length];
            for (var i = 0; i < secondaryGraphs.Length; i++)
            {
                var secondaryGraph = secondaryGraphs[i];
                instantiatedSecondaryGraphs[i] = AiGraph.CreateAndInitializeGraph(dontHideInHierarchy, graphStepsPerUpdate, gameObject.name, secondaryGraph);
            }

            Initialized();
        }

        protected virtual void Start()
        {
            if (!IsInitialized) return;
            aiGraph.UpdateContext((contextProvider as IContextProvider)?.GetContext());
            foreach (var secondaryGraph in instantiatedSecondaryGraphs)
                secondaryGraph.UpdateContext((contextProvider as IContextProvider)?.GetContext());
            LoadBalancerSingleton.Instance.Register(this, Tick, updateFrequency);
        }

        protected virtual void Tick(float _deltaTime)
        {
            if (aiGraph != null) aiGraph.UpdateGraph(_deltaTime);

            foreach (var secondaryGraph in instantiatedSecondaryGraphs)
            {
                if (secondaryGraph == null) continue;
                secondaryGraph.UpdateGraph(_deltaTime);
            }
#if UNITY_EDITOR
            if (aiGraph == null) return;
            // debug info
            if (aiGraph.lastNode != null) CurrentNode = aiGraph.lastNode.Name;
            if (aiGraph.lastAiUtility != null) LastUtility = aiGraph.lastAiUtility.Name;
            if (aiGraph.lastTask != null) LastTask = aiGraph.lastTask.Name;
#endif
        }

        protected void Initialized() => IsInitialized = true;

        protected virtual void OnEnable()
        {
            if (!IsInitialized) return;
            RegisterGraphUpdateLoop();
        }

        protected virtual void OnDisable()
        {
            if (LoadBalancerSingleton.Instance == null || !IsInitialized) return;
            LoadBalancerSingleton.Instance.Unregister(this, Tick);
        }

        protected virtual void OnDestroy()
        {
            if (!Application.isPlaying || !IsInitialized) return;

            if (aiGraph != null) Destroy(aiGraph.gameObject);
            foreach (var instantiatedSecondaryGraph in instantiatedSecondaryGraphs)
                if (instantiatedSecondaryGraph != null)
                    Destroy(instantiatedSecondaryGraph);
        }

        #endregion
    }
}
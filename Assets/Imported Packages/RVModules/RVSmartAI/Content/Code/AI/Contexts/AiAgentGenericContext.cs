// Created by Ronis Vision. All rights reserved
// 23.08.2019.

using System.Collections.Generic;
using RVModules.RVSmartAI.Content.Code.Movements;
using RVModules.RVSmartAI.Content.Code.Scanners;
using RVModules.RVUtilities;
using UnityEngine;

namespace RVModules.RVSmartAI.Content.Code.AI.Contexts
{
    /// <summary>
    /// Generic context providing the most common/useful members for ai
    /// </summary>
    public class AiAgentGenericContext : MonoBehaviour, IContext, IContextProvider, IMovementProvider, IMovementScannerProvider, IEnvironmentScannerProvider,
        IMoveTargetProvider, IWaypointsProvider, INearbyObjectsProvider, IJobHandlerProvider
    {
        #region Fields

        [SerializeField]
        protected Transform moveTarget;

        [SerializeField]
        protected List<Transform> waypoints;

        [SerializeField]
        protected ListNonAlloc<Object> nearbyObjects = new ListNonAlloc<Object>();

        [SerializeField]
        protected TaskHandler aiJobHandler;

        #endregion

        #region Properties

        public IMovement Movement { get; private set; }

        public IMovementScanner MovementScanner { get; private set; }

        public IEnvironmentScanner EnvironmentScanner { get; private set; }

        public Transform FollowTarget
        {
            get => moveTarget;
            set => moveTarget = value;
        }

        public ListNonAlloc<Object> NearbyObjects => nearbyObjects;

        #endregion

        #region Public methods

        /// <summary>
        /// Finds references and creates TaskHandler
        /// </summary>
        public virtual void Awake()
        {
            Movement = GetComponent<IMovement>();
            EnvironmentScanner = GetComponent<IEnvironmentScanner>();
            MovementScanner = GetComponent<IMovementScanner>();
            CreateAiJobHandler();
        }

        /// <summary>
        /// Override for custom initial task handler settings
        /// </summary>
        protected virtual void CreateAiJobHandler() => aiJobHandler = new TaskHandler();

        // IContextProvider implementation
        public virtual IContext GetContext() => this;

        #endregion

        public Vector3 GetWaypointPosition(int _id) => waypoints[_id].position;
        public int WaypointsCount => waypoints.Count;
        public TaskHandler AiJobHandler => aiJobHandler;
    }
}
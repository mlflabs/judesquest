// Created by Ronis Vision. All rights reserved
// 27.10.2019.

using RVModules.RVUtilities.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace RVModules.RVSmartAI.Content.Code.Movements
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class UnityNavMeshMovement : MonoBehaviour, IMovement
    {
        #region Fields

        [SerializeField]
        private bool reserveDestinationPosition = true;

        [SerializeField]
        [HideInInspector]
        private NavMeshAgent agent;

        [SerializeField]
        private GameObject destPosBlocker;

        [SerializeField]
        private int destinationBlockLayer = 9;

        // serialized for debugging only
        [SerializeField]
        private Vector3 destination;

        // cached trasform access
        private new Transform transform;

        #endregion

        #region Properties

        public Vector3 Velocity => agent.velocity;
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;

        public bool AtDestination => !agent.hasPath || transform.position.ManhattanDistance2d(Destination) < .2f || agent.destination == Vector3.zero;

        public Vector3 Destination
        {
            get => destination;
            set
            {
                agent.destination = value;
                destination = agent.destination;
                if (destination.ManhattanDistance2d(transform.position) < .1f)
                    agent.isStopped = true;
                else
                    agent.isStopped = false;

                if (ReserveDestinationPosition)
                    destPosBlocker.transform.position = destination;
            }
        }

        /// <summary>
        /// Create 'blocker' object with collider that is set to destination position
        /// to avoid many agents trying to go to the same position
        /// </summary>
        public bool ReserveDestinationPosition
        {
            get => reserveDestinationPosition;
            set
            {
                if (value && destPosBlocker == null) CreateDestinationBlocker();
                if (!value && destPosBlocker != null) Destroy(destPosBlocker);
                reserveDestinationPosition = value;
            }
        }

        public int DestinationBlockLayer
        {
            get => destinationBlockLayer;
            set
            {
                destinationBlockLayer = value;
                if (destPosBlocker != null) destPosBlocker.layer = value;
            }
        }

        #endregion

        #region Not public methods

        /// <summary>
        /// Removes destination position blocker
        /// </summary>
        protected virtual void OnDestroy()
        {
            if (destPosBlocker == null) return;
            Destroy(destPosBlocker);
        }

        protected virtual void Awake()
        {
            transform = base.transform;
            agent = GetComponent<NavMeshAgent>();
            agent.avoidancePriority = Random.Range(0, 100);
            //agent.updateRotation = false;
//            agent.velocity = destination;
//            agent.angularSpeed = destPosBlockerLayer;

            if (!ReserveDestinationPosition) return;
            CreateDestinationBlocker();
        }

        protected virtual void CreateDestinationBlocker()
        {
            if (destPosBlocker != null) return;
            destPosBlocker = new GameObject(name + " destination blocker");
            var coll = destPosBlocker.AddComponent<SphereCollider>();
            coll.isTrigger = true;
            DestinationBlockLayer = destinationBlockLayer;
        }

        #endregion
    }
}
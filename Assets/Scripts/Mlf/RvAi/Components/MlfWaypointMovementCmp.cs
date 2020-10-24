using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using Mlf.Traffic;
using Mlf.RvAi.Contexts;

namespace Mlf.RvAi.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class MlfWaypointMovementCmp : MonoBehaviour, IMlfWaypointMovement
    {
        public float pathBuffer = 0.5f;
        public float rotationSpeed = 3f;
        public float rotationAngleBuffer = 0f;
        [SerializeField] private Vector3 _target;

        NavMeshAgent agent;
        [SerializeField] private Stack<Waypoint> _path = null;

        public bool useWaypointLoop = false;
        [SerializeField] private Waypoint[] waypointLoop;
        private int currentWaypointLoopIndex = 0;

        Waypoint _currentWaypoint = null;

        Waypoint _destination = null;



        public Waypoint Destination
        {
            get => _destination;
            set
            {
                _path = PathManager.instance.GetRandomLocationPath(transform.position);
            }
        }

        public bool AtDestination => _path == null || _path.Count == 0;
        public Vector3 Velocity => agent.velocity;
        public Vector3 Position => transform.position;
        public Quaternion Rotation => transform.rotation;


        public Vector3 target
        {
            get => _target; set
            {
                _target = value;
                if (value == Vector3.zero)
                {
                    //see if we where mid point destination
                    agent.isStopped = false;
                }
                else
                {
                    transform.LookAt(value);
                    agent.isStopped = true;
                }

            }
        }





        //public float targetBuffer = 1f; 

        void Start()
        {

            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
            target = Vector3.zero;
        }

        void Update()
        {
            if (target != Vector3.zero)
            {
                transform.LookAt(target);
                return;
            }
            else if (AtDestination)
            {
                return;
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance < pathBuffer)
                    GotoNextPoint();
            }

        }

        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (_path.Count == 0)
            {
                _path = null;
                return;
            }

            _currentWaypoint = _path.Pop();
            agent.destination = _currentWaypoint.transform.position;

        }


        public void LoadNewPath(Stack<Waypoint> path)
        {
            _path = path;
        }


        public void LoadNextWaypoint()
        {
            //if loop, use loop points, if not, find random point
            if (useWaypointLoop && waypointLoop.Length > 0)
            {
                currentWaypointLoopIndex++;
                if (currentWaypointLoopIndex < waypointLoop.Length)
                {
                    LoadNewPath(PathManager.instance.FindPathToTarget(
                        waypointLoop[currentWaypointLoopIndex - 1],
                        waypointLoop[currentWaypointLoopIndex]
                    ));
                }
                else
                {
                    currentWaypointLoopIndex = 0;
                    LoadNewPath(PathManager.instance.FindPathToTarget(
                        waypointLoop[waypointLoop.Length - 1],
                        waypointLoop[currentWaypointLoopIndex]
                    ));
                }



            }
            else
            {
                LoadNewPath(PathManager.instance.GetRandomLocationPath(this.transform.position));
            }
        }


        /*
         * 
        private void RotateTowards(Vector3 target)
        {
            agent.updateRotation = false;
            Vector3 direction = (target - transform.position).normalized;
            Debug.Log(direction);
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                lookRotation, Time.deltaTime * rotationSpeed);
        }

        private void FaceTarget(Vector3 destination)
        {

            Vector3 lookPos = destination - transform.position;
            lookPos.y = 0;
            Quaternion rotation = Quaternion.LookRotation(lookPos);
            transform.rotation = Quaternion.Slerp(transform.rotation,
                rotation, rotationSpeed);

            if (Vector3.Angle(transform.forward, transform.position - target)
                < rotationAngleBuffer)
            {
                Debug.Log("Changing facing target...." +
                    Vector3.Angle(transform.forward, transform.position - target));
                facingTarget = true;
            }
        }

        */




    }
}

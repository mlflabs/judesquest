using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Mlf.Traffic
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PathAgent : MonoBehaviour
    {
        public float pathBuffer = 0.5f;

        NavMeshAgent agent;
        [SerializeField] Stack<Waypoint> path = null;
        Waypoint currentDestinationPoint = null;


        void Start()
        {
            agent = GetComponent<NavMeshAgent>();
            agent.autoBraking = false;
        }

        void Update()
        {



            if (path == null || path.Count == 0)
            {
                calculateNewPath();
                return;
            }
            else
            {
                if (!agent.pathPending && agent.remainingDistance < 0.5f)
                    GotoNextPoint();
            }

        }

        void GotoNextPoint()
        {
            // Returns if no points have been set up
            if (path.Count == 0)
            {
                calculateNewPath();
                return;
            }

            currentDestinationPoint = path.Pop();
            agent.destination = currentDestinationPoint.transform.position;


        }


        void calculateNewPath()
        {
            path = PathManager.instance.GetRandomLocationPath(transform.position);
        }

    }

}

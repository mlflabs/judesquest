using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Mlf.Traffic
{
    public class WaypointNavigator : MonoBehaviour
    {


        public Waypoint currentWaypoint;
        public float speed = 2;
        public float turnSpeed = 3;
        public float bufferSpace = 0.2f;

        private NavMeshAgent agent;
        private CharacterController controller;


        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            agent = GetComponent<NavMeshAgent>();
        }

        private void Start()
        {
            agent.SetDestination(currentWaypoint.GetPosition());
        }



        private void Update()
        {

            // have we reached destination
            if (agent.remainingDistance < bufferSpace)
            {
                //currentWaypoint = currentWaypoint.nextWaypoint;
                agent.SetDestination(currentWaypoint.GetPosition());
            }


            return;
        }

        /*

        public Transform _playerTrans;
        public float _speed = 2;
        public float _turnSpeed = 3;

        private NavMeshAgent _agent;
        private Vector3 _desVelocity;
        private CharacterController _charControl;

        void Start()
        {

            this._agent = this.gameObject.GetComponent<NavMeshAgent>();
            this._charControl = this.gameObject.GetComponent<CharacterController>();

            this._agent.destination = this._playerTrans.position;

            return;
        }

        void Update()
        {

            Vector3 lookPos;
            Quaternion targetRot;

            this._agent.destination = this._playerTrans.position;
            this._desVelocity = this._agent.desiredVelocity;

            this._agent.updatePosition = false;
            this._agent.updateRotation = false;

            lookPos = this._playerTrans.position - this.transform.position;
            lookPos.y = 0;
            targetRot = Quaternion.LookRotation(lookPos);
            this.transform.rotation = Quaternion.Slerp(transform.rotation, targetRot,
        Time.deltaTime * this._turnSpeed);

            this._charControl.Move(this._desVelocity.normalized * this._speed * Time.deltaTime);

            this._agent.velocity = this._charControl.velocity;

            return;
        }

        */
    }
}


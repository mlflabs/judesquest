using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Mlf.Controller
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class AnimationAgent : MonoBehaviour
    {
        NavMeshAgent nav;
        Animator anim;

        [SerializeField] private float moveing;


        void Start()
        {
            nav = GetComponent<NavMeshAgent>();
            nav.autoBraking = false;
            anim = GetComponent<Animator>();
        }

        void Update()
        {
            if (nav.velocity != Vector3.zero)
            {
                anim.SetFloat("Speed", 1);
                moveing = 1f;
            }
            else
            {
                anim.SetFloat("Speed", 0);
                moveing = 0f;
            }


        }

    }

}

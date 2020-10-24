using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;

namespace Mlf.RvAi.Components
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class MlfAnimatorCmp : MonoBehaviour
    {
        NavMeshAgent nav;
        Animator anim;


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
            }
            else
            {
                anim.SetFloat("Speed", 0);
            }


        }

    }

}

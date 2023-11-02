using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class transition2Behaviour : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    //override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // Altered from https://www.youtube.com/watch?v=xr8UPU_X7A8&t=1405s
    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (LunalaController.instance.isAttacking) {
            LunalaController.instance.anim.SetTrigger("hit3");
            Vector3 crossPos = GameObject.Find("Crosshair").transform.position;
            Vector3 moveDir = (crossPos - LunalaController.instance.transform.position).normalized;
            LunalaController.instance.rb.velocity = moveDir * LunalaController.instance.moveSpeed;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LunalaController.instance.isAttacking = false;
    }


    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}

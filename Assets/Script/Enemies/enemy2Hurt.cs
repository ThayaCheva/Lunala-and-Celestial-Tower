using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy2Hurt : StateMachineBehaviour
{
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameObject enemySprite= animator.gameObject;
       Transform parent= enemySprite.transform.parent;
       GameObject enemy= parent.gameObject;
       var EnemyController= enemy.GetComponent<Enemy2Controller>();
       EnemyController.isStun=true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

     override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       GameObject enemySprite= animator.gameObject;
       Transform parent= enemySprite.transform.parent;
       GameObject enemy= parent.gameObject;
       var EnemyController= parent.GetComponent<Enemy2Controller>();
       EnemyController.isStun=false;

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

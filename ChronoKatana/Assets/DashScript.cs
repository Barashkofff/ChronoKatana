using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class DashScript : StateMachineBehaviour
{
    [SerializeField] private float trans;
    Collider2D collider;
    Transform legs;
    Transform torso;
    SpriteRenderer sprite1;
    SpriteRenderer sprite2;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        collider = PlayerController.instance.GetComponent<Collider2D>();
        torso = collider.transform;
        legs = PlayerController.instance.GetLegsTransform();
        sprite1 = collider.GetComponent<SpriteRenderer>();
        sprite2 = legs.GetComponent<SpriteRenderer>();

        collider.offset += new Vector2(0, 0.125f);
        legs.position += new Vector3(0, 0.125f, 0);
        torso.position += new Vector3(0, -0.125f, 0);

        if (PlayerController.instance.GetCoolDash())
        {
            sprite1.color = new Color(1, 1, 1, trans);
            sprite2.color = new Color(1, 1, 1, trans);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        collider.offset += new Vector2(0, -0.125f);
        legs.position += new Vector3(0, -0.125f, 0);
        torso.position += new Vector3(0, 0.125f, 0);

        if (PlayerController.instance.GetCoolDash())
        {
            sprite1.color = new Color(1, 1, 1, 1);
            sprite2.color = new Color(1, 1, 1, 1);
        }
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

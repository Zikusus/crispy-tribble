using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBool : StateMachineBehaviour
{
    public string isInteractingBool;
    public bool isInteractingStatus = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state

    //Every time you perform an animation like falling or landing, when the animation is done the OnStateEnter function will be called to reset the isInteracting bool so we would be able to move again
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
       animator.SetBool(isInteractingBool, isInteractingStatus);
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    PlayerInputActions playerInputActions;
    PlayerAnimator playerAnimator;
    PlayerMovement playerMovement;
    public GameObject camMain;

    public Vector2 mouseInput;
    public Vector2 movementInput;
    public float scrollInput;
    public bool walkInput;
    public bool runInput;
    public bool jumpInput;
    public bool attackInput;
    public bool pickUpInput;
    public bool dropItemInput;

    public bool pauseInput;

    private void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAnimator = GetComponent<PlayerAnimator>();
    }

    private void OnEnable()
    {
        if (playerInputActions == null)
        {
            playerInputActions = new PlayerInputActions();

            playerInputActions.Movement.View.performed += ctx => mouseInput = ctx.ReadValue<Vector2>(); // += context => ctx. is the callback fuction syntax in c#
            playerInputActions.Movement.Scroll.performed += ctx => scrollInput = ctx.ReadValue<float>();
            playerInputActions.Movement.Movement.performed += ctx => movementInput = ctx.ReadValue<Vector2>();

            playerInputActions.Actions.Walk.performed += ctx => walkInput = true;
            playerInputActions.Actions.Walk.canceled += ctx => walkInput = false;
            playerInputActions.Actions.Run.performed += ctx => runInput = true;
            playerInputActions.Actions.Run.canceled += ctx => runInput = false;
            playerInputActions.Actions.Jump.performed += ctx => jumpInput = true;
            playerInputActions.Actions.Jump.canceled += ctx => jumpInput = false;

            playerInputActions.Actions.Attack.performed += ctx => attackInput = true;
            playerInputActions.Actions.Attack.canceled += ctx => attackInput = false;

            playerInputActions.Actions.PickUp.performed += ctx => pickUpInput = true;
            playerInputActions.Actions.PickUp.canceled += ctx => pickUpInput = false;
            playerInputActions.Actions.DropItem.performed += ctx => dropItemInput = true;
            playerInputActions.Actions.DropItem.canceled += ctx => dropItemInput = false;
        }

        playerInputActions.Enable();
    }

    public void HandleInput()
    {
        HandleJumpInput();
        HandleAttackInput();
    }

    private void OnDisable()
    {
        playerInputActions.Disable();
    }

    //Why tf I can't put this function in PlayerMovement???
    public void HandleJumpInput()
    {
        if (jumpInput)
        {
            jumpInput = false;
            playerMovement.HandleJump();
        }
    }

    public void HandleAttackInput()
    {
        if(attackInput)
        {
            attackInput = false;
            playerMovement.HandleAttack(Random.Range(1,3));
        }
    }
}
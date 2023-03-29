using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandling : MonoBehaviour
{
    PlayerMovement playerMovement;
    Camera cameraScript;
    InputManager inputManager;
    Animator animator;

    public bool isInteracting;

    private void Awake()
    {
        animator= GetComponent<Animator>();
        playerMovement = GetComponent<PlayerMovement>();
        cameraScript = GameObject.FindGameObjectWithTag("CameraCenter").GetComponent<Camera>();
        inputManager = GetComponent<InputManager>();
    }

    private void Update()
    {
        inputManager.HandleInput();
        //playerMovement.HandleAll();
       // playerMovement.HandleRotation();
       // cameraScript.FollowPlayer();
    }

    private void FixedUpdate()
    {
        playerMovement.HandleAll();
       // playerMovement.HandleMovement();
    }

    private void LateUpdate()
    {
        cameraScript.FollowPlayer();
        cameraScript.HandleCameraMovement();
        isInteracting = animator.GetBool("isInteracting");
        playerMovement.isJumping = animator.GetBool("isJumping");
        animator.SetBool("isGrounded", playerMovement.isGrounded);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    InputManager inputManager;
    Rigidbody playerRb;
    PlayerAnimator playerAnimator;
    PlayerHandling playerHandling;
    SurvivalSystem survivalSystem;
    
    public GameObject mainCamera;

    [Header("Movement settings")]
    public Vector3 facingDirection;
    public float rotationSpeed = 10;
    public float movementSpeed=3;

    [Header("Turning")]
    public float slerpSpeed = 0.1f;
    public Vector3 right = Vector3.right;
    public Vector3 forward = Vector3.forward;
    float rightDirection;
    float forwardDirection;

    [Header("FallingAndLanding")]
    public bool isGrounded;
    public float inAirTimer;
    public float leapingVelocity;
    public float fallingSpeed;
    public LayerMask groundLayer;
    public float raycastOriginOffSet;
    float state = 1;
    public float fallingMovSpeed = 1;

    [Header("Jumping")]
    public bool isJumping;
    public float jumpHeight = 3;
    public float gravityIntensity = -15;

    public void Awake()
    {
        groundLayer = 1 << LayerMask.NameToLayer("Ground");

        isGrounded = true;

        playerHandling= GetComponent<PlayerHandling>();
        inputManager= GetComponent<InputManager>();
        playerRb= GetComponent<Rigidbody>();
        playerAnimator= GetComponent<PlayerAnimator>();
        survivalSystem = GetComponent<SurvivalSystem>();
    }

    public void HandleAll()
    {
        HandleLandAndFall();
        HandleRotation();
        HandleMovement();
        //Lock player from movement if he's interacting=
        if (playerHandling.isInteracting)
            return;
        if (isJumping)
            return;
        HandleTurningAnimation();
        HandleWalkTrotRun();
    }


    //Apply for non-root motion animations
    public void HandleMovement()
    {
        facingDirection = mainCamera.transform.forward * inputManager.movementInput.y + mainCamera.transform.right * inputManager.movementInput.x;
        facingDirection.y = 0;
        facingDirection.Normalize();
        facingDirection *= movementSpeed;
        //Debug.Log(facingDirection);
        //playerRb.velocity = facingDirection;
        //playerRb.AddForce(facingDirection,ForceMode.Impulse);
    }
    /*public void HandleRotation()//try using camera center instead of the main camera
    {
        Vector3 facingDir = mainCamera.transform.forward * inputManager.movementInput.y + mainCamera.transform.right * inputManager.movementInput.x;
        facingDir.y = 0;
        facingDir.Normalize();
       // facingDir.y = 0;

        if(facingDir == Vector3.zero)
        {
            facingDir = Vector3.forward;
        }

        Quaternion lookTowards = Quaternion.LookRotation(facingDir);
        Quaternion rotateTowards = Quaternion.Slerp(transform.rotation, lookTowards, rotationSpeed * Time.deltaTime);
        transform.rotation = rotateTowards;
    }*/

    public void HandleRotation()
    {
        Vector3 movementInput = mainCamera.transform.forward * inputManager.movementInput.y + mainCamera.transform.right * inputManager.movementInput.x;
        movementInput.y = 0;
        if (movementInput.magnitude > 1)
            movementInput.Normalize();


        if (movementInput.magnitude > 0)
        {
            Quaternion rotation = Quaternion.LookRotation(movementInput);
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * rotationSpeed);
           // Debug.Log(rotation);
        }
    }

    public void HandleTurningAnimation()
    {
        right = Vector3.Slerp(right, transform.right, slerpSpeed);
        forward = Vector3.Slerp(forward, transform.forward, slerpSpeed);

        //Used Vector3.Slerp for right and forward vectors to smoothly rotate towards the transform.right and transform.foward vector
        //I do it smoothly so I can get the dot product between transform.right * forward and transform.forward * right
        //Acces https://www.falstad.com/dotproduct/ for a visualization
        forwardDirection = Vector3.Dot(transform.right, forward);
        rightDirection = Vector3.Dot(transform.forward, right);//
        //Debug.Log(forwardDirection + "  " + rightDirection);

        
        Debug.DrawRay(transform.position, transform.right*2,Color.red);
        Debug.DrawRay(transform.position, transform.forward*2, Color.red);

        Debug.DrawRay(transform.position, right* 2, Color.magenta);
        Debug.DrawRay(transform.position, forward * 2, Color.magenta);
        


        //movementInput.x and .y !=0 so It won't play the animation if you don't rotate
        //Mathf.Abs(forwardDirection/rightDirection) so the animation will stop when you fully rotate towards the forward/right direction
        if ((inputManager.movementInput.x != 0 || inputManager.movementInput.y != 0) && (Mathf.Abs(forwardDirection) > 0.1f) && (Mathf.Abs(rightDirection) > 0.1f))
        {
            //So the animation would play instantly, later on ADD MORE RIGHTDIRECTION CHECK VALUES TO MAKE IT REALISTIC
            if (rightDirection > 0)
            {
                if (rightDirection < 0.2)
                {
                    rightDirection = 0.5f;
                }
                else
                {
                    rightDirection = 1;
                }
            }
            else
            {
                if (rightDirection > -0.2)
                {
                    rightDirection = -0.5f;
                }
                else
                {
                    rightDirection = -1;
                }
            }
        }
        else
            rightDirection = 0;
    }

    public void HandleWalkTrotRun()
    {
        state = 1;
        if (inputManager.movementInput.y != 0 || inputManager.movementInput.x != 0)
        {
            if (inputManager.walkInput)
            {
                state = 0.25f;
            }
            else if(inputManager.runInput)
            {
                state = 2;
                if(survivalSystem.currentStamina<=0)
                {
                    state = 1; //go back to walking state if the stamina is depleted
                }
            }
        }

        //So the character won't slow down while pressing both w/s and a/d keys
        float vertical = Mathf.Clamp01(Mathf.Abs(inputManager.movementInput.x) + Mathf.Abs(inputManager.movementInput.y))*state;

        if (playerAnimator!= null)
        {
            playerAnimator.UpdateAnimationValues(rightDirection, vertical);
        }
    }

    public void HandleLandAndFall()
    {
        RaycastHit hit;
        Vector3 raycastOrigin = transform.position;
        raycastOrigin.y += raycastOriginOffSet; //So the raycast won't start below the players feet or below the ground

        if(!isGrounded && !isJumping)
        {
            //If the player is not grounded and is not interacting then we'll make him interact
            if(!playerHandling.isInteracting)
            {
                playerAnimator.PlayDesiredAnimation("Fox_Fall_Low", true);
            }
            inAirTimer += Time.deltaTime; //The more the player is in the air, the more faster he'll fall
            Vector3 fallingMovement = new Vector3(facingDirection.x*fallingMovSpeed,facingDirection.y*fallingMovSpeed, facingDirection.z*fallingMovSpeed);
            playerRb.AddForce(fallingMovement, ForceMode.Impulse); //Let the player move while falling
            playerRb.AddForce(transform.forward * leapingVelocity);
            playerRb.AddForce(Vector3.down * fallingSpeed * inAirTimer);

            if(inAirTimer>0.5f)
            {
                playerAnimator.PlayDesiredAnimation("Fox_Fall_High", true);
            }
        }

        if(Physics.SphereCast(raycastOrigin, 0.2f, Vector3.down, out hit,0.5f, groundLayer))
        {
            if(!isGrounded&&playerHandling.isInteracting)
            {
                playerAnimator.PlayDesiredAnimation("Fox_Fall_SmallRecover", true);
            }

            inAirTimer = 0;
            isGrounded= true;
           // playerHandling.isInteracting = false; // That's what resetbool does
        }
        else
        {
            isGrounded= false;
        }
    }

    public void HandleJump()
    {
        if(isGrounded)
        {
            float heightboost = 1;
            if(state <= 1)
            {
                playerAnimator.animator.SetBool("isJumping", true);
                playerAnimator.PlayDesiredAnimation("Fox_Jump_Forward", false); //False because the player isn't interacting and we want him to be able to move
            }
            else
            {
                playerAnimator.animator.SetBool("isJumping", true);
                playerAnimator.PlayDesiredAnimation("Fox_Jump_Run", false); //False because the player isn't interacting and we want him to be able to move
                heightboost = 1.7f;
            }
            float jumpVelocity = Mathf.Sqrt(-2 * gravityIntensity * jumpHeight * heightboost);
            Vector3 playerVelocity = facingDirection;
            playerVelocity.y = jumpVelocity;
            playerRb.velocity = playerVelocity;
        }
    }

    public void HandleAttack(int attacktype)
    {
        if (isGrounded)
        {
            if(attacktype == 1)
            {
                playerAnimator.PlayDesiredAnimation("Fox_Attack_Bite_Right", true);
            }
            else if(attacktype == 2)
            {
                playerAnimator.PlayDesiredAnimation("Fox_Attack_Bite_Left", true);
            }
            else if(attacktype == 3)
            {
                playerAnimator.PlayDesiredAnimation("Fox_Attack_Claws", true);
            }
            else
            {
                playerAnimator.PlayDesiredAnimation("Fox_Attack_Jump", true);
            }
        }
    }
}
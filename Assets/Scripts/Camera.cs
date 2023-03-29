using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    InputManager inputManager;

    public GameObject behindCamera;
    public GameObject character;
    public GameObject cameraCenter;
    public Camera cam;
    public Transform playerTransform;

    [Header("Settings")]
    public float yOffSet = 0;
    public float xOffSet = 0;
    public float minRotationLimit = 0;
    public float maxRotationLimit = 45;
    public float sensitivity = 3;
    public float distCameraCollisionFromPlayer = -1;
    public Vector3 cameraFollowVelocity = Vector3.zero;
    public float cameraSmoothFollowSpeed = 0.3f;

    [Header("Scroll Settings")]
    public float scrollSensitivity = 1;
    public float scrollDamp = 1;
    public float scrollSpeed = 0.3f;

    [Header("Zoom Settings")]
    public float zoomMin = 3;
    public float zoomMax = 15;
    public float zoomDefault = 10;
    public float zoomDistance;

    [Header("Collision Settings")]
    public float collisionSensitivity = 1;
    public float collisionDamp = 1;
    public Vector3 currentVelocity = Vector3.zero;
    public int ignoreCollision;


    private RaycastHit _camHit;
    private Vector3 _camDist;


    private void Awake()
    {
        ignoreCollision = 1 << LayerMask.NameToLayer("Ground");

        Cursor.visible = false;

        inputManager = FindObjectOfType<InputManager>();
        playerTransform = FindObjectOfType<PlayerHandling>().transform;

        _camDist = cam.transform.localPosition;
        _camDist.z = zoomDefault;
        zoomDistance = zoomDefault;
    }

    public void HandleCameraMovement()
    {
        HandleCamereMovement();
        HandleCameraRotation();
        HandleCameraZoom();
        HandleCameraCollisions();
    }

    //Use this function instead of directly using Mathf.Clamp to clamp eulerAngles as it only accepts possitive values between 0 and 360
    public float ClampAngle(float angle, float min, float max) {
        if (angle>180) angle -= 360;
        angle = Mathf.Clamp(angle, min, max); //If the angle is between 90 and 270
        if (angle < 0) angle += 360;  // If the angle is negative
        return angle;
    }

    public void HandleCamereMovement()
    {
        //Make the camera move based on the cameraCenter position  
        cameraCenter.transform.position = new Vector3(character.transform.position.x + xOffSet, character.transform.position.y + yOffSet, cameraCenter.transform.position.z);
    }

    public void HandleCameraRotation()
    {
        //Make the camera rotate based on the mouse input
        Quaternion rotation = Quaternion.Euler(
            ClampAngle(cameraCenter.transform.rotation.eulerAngles.x - inputManager.mouseInput.y * sensitivity,minRotationLimit,maxRotationLimit), //Restring the x rotation on the camera, FIXED, yey
            cameraCenter.transform.rotation.eulerAngles.y + inputManager.mouseInput.x * sensitivity,
            cameraCenter.transform.rotation.eulerAngles.z);

        //Debug.Log(cameraCenter.transform.rotation.eulerAngles.x);
        cameraCenter.transform.rotation = rotation;
    }

    public void HandleCameraZoom()
    {
        //Make the camera zoom in or out based on the mouse wheel input
        if (inputManager.scrollInput != 0)
        {
            float scrollAmount = inputManager.scrollInput * scrollSensitivity;
            scrollAmount *= zoomDistance; //The farther away, the faster
            zoomDistance += -scrollAmount; // - because when you scroll back, it gives you negative values and when you scroll forward, positive
            zoomDistance = Mathf.Clamp(zoomDistance, zoomMin, zoomMax);

        }

        //To make sure you don't change the camera zoom every frame if you don't need to
        if (_camDist.z != -zoomDistance)
        {
            _camDist.z = Mathf.Lerp(_camDist.z, -zoomDistance, Time.deltaTime * scrollDamp);
        }

        cam.transform.localPosition = _camDist;
    }

    public void FollowPlayer() // FIX THIS and Don't forget to use interpolate on the character(fox)
    {
        Vector3 followPlayer = Vector3.SmoothDamp(cameraCenter.transform.position, playerTransform.position, ref cameraFollowVelocity, cameraSmoothFollowSpeed);
       cameraCenter.transform.position = followPlayer;
    }

    public void HandleCameraCollisions()
    {
        behindCamera.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, cam.transform.localPosition.z - collisionSensitivity);

        if(Physics.Linecast(cameraCenter.transform.position,behindCamera.transform.position, out _camHit,ignoreCollision)) //Check if the raycast from the player to the camera hits anything
        {
            //Move the camera to the raycast hit location
            cam.transform.position = Vector3.SmoothDamp(cam.transform.position,_camHit.point,ref currentVelocity,collisionDamp); //FIX THE SMOOTH DAMP

            Vector3 localPosition = new Vector3(cam.transform.localPosition.x,cam.transform.localPosition.y,cam.transform.localPosition.z + collisionSensitivity); //So it won't be directly in the object it collides with
                                                                                                                                                                   //but just a little in front
            cam.transform.localPosition = localPosition;
        }
        Debug.DrawRay(cameraCenter.transform.position, behindCamera.transform.position, Color.green);

        //Make sure the camera won't clip through the player
        if(cam.transform.localPosition.z > distCameraCollisionFromPlayer)
        {
            cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, distCameraCollisionFromPlayer);
        }
    }

}

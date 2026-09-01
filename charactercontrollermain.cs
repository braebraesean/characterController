using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterControllerMain : MonoBehaviour
{
    

   
    // characterController is a reference to the CharacterController component attached to the player character
    private CharacterController characterController;
    // speed is a constant that will be used to control the speed of the player character
    public float speed =.1f;
    // gravity is a constant that will be used to simulate the effect of gravity on the player character
    public float gravity = 9.81f;
    // DEBUG is a variable that will be used to enable or disable debug mode
    
    // camera is a reference to the main camera in the scene
    private Camera camera;
    // Tracks current camera angle (0 = straight ahead)
    private float verticalAngle = 20f;  
    // Limit: camera can look max 90° up or down
    public float maxVerticalAngle = 89f;  
    // sets the rotation speed of the camera
    public float mouseSensitivity = 100f;
    int DEBUG;

    // Start is called before the first frame update
    void Start()
    {
        //define camera and player character controller
        characterController = GetComponent<CharacterController>();
        camera = Camera.main;
        // lock camera to the player
        camera.transform.SetParent(transform);
        // rotates camera to look at player
        camera.transform.Rotate(verticalAngle,0,0, Space.Self);

        
        // Lock cursor to game window
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;  // Optional: hide the cursor
    }

    // Update is called once per frame
    void Update()
    {
        // get input from the player
        float HorizontalX = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float HorizontalZ = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // calculate the vertical speed of the player character based on whether they are grounded or not
        float ySpeed = characterController.isGrounded ? 0 : -gravity;
        
        // Get camera's forward and right directions
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;

        // Remove Y component (keep movement horizontal)
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        
        // Calculate movement relative to camera
        Vector3 moveDirection = (cameraForward * HorizontalZ + cameraRight * HorizontalX).normalized;

        // multiply movement by speed
        moveDirection *= speed;
        mouseX *= mouseSensitivity * 50;
        mouseY *= mouseSensitivity * 50;
        
        // create a new vector3 to represent the vertical movement of the player character
        Vector3 VerticalMovement = new Vector3(0, ySpeed, 0);
        
        // move the player character in the direction of the movement vector
        characterController.Move(moveDirection + VerticalMovement * Time.deltaTime);
        
        // checks what the angle of the camera would be vertically if rotated this frame
        float newAngle = verticalAngle - mouseY * Time.deltaTime;

        // uses that new angle and previous angle to calcculate how much the camera would rotate
        float deltaRotation = newAngle - verticalAngle;

        // calculates how much further the camera could rotate without going over max
        float maxDelta = maxVerticalAngle - verticalAngle;  
        float minDelta = -maxVerticalAngle - verticalAngle;

        // clamps down the rotation to not go over +-90
        deltaRotation = Mathf.Clamp(deltaRotation, minDelta, maxDelta);

        
        // if clamped angle is different from current angle, rotates camera vertically
        if (deltaRotation != 0)
        {
            camera.transform.RotateAround(transform.position, transform.right, deltaRotation);
            verticalAngle += deltaRotation;
        }
        // rotate the player horozontally around the y axis
        transform.Rotate(0, mouseX * Time.deltaTime, 0);

        // check if the player has pressed the L key to enable debug mode
        DEBUG = Input.GetKeyDown(KeyCode.L) ? 1 : 0;
        if (DEBUG == 1) debug(moveDirection); 
    } 
    // debug function to print out the current state of the player character
    private void debug(Vector3 moveDirection)
    {
        Debug.Log("CharacterControllerMain: " + characterController.isGrounded);
        Debug.Log(moveDirection);
    }

}

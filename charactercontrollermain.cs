using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMain : MonoBehaviour
{
    

   
    private Rigidbody Player;

    //model variables
    GameObject Model;
    private Quaternion modelRotation;
    // movement variables
    public float speed = 40f;
    public float gravity = 9.81f;
    public float friction = 1.8f; 
    public float acceleration = 0.5f;

    // camera variables
    private Camera camera;
    private Vector3 cameraDirection;
    private float verticalAngle = 0f;  
    private float horizontalAngle = 0f;
    private float maxVerticalAngle = 90f;  
    public float mouseSensitivity = 100f;
    public float cameraAngleSet = 20f;
    public float cameraDistance = 10F;

    // debug variables
    int DEBUG;
    private Vector3 currentVelocity = Vector3.zero;
    private bool IsInput;
    private bool IsSlowingDown;

    void Start(){
            // define camera, player character, playermodel, and the cursor, then locks them together.

            // camera
            camera = Camera.main;
            camera.transform.SetParent(transform);
            verticalAngle += cameraAngleSet;

            // model
            Model = GameObject.FindWithTag("Player");

            //player
            Player = Model.GetComponent<Rigidbody>();
            
            // cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;  // Optional: hide the cursor    
        }

    void FixedUpdate(){
        //handles player movement
        PlayerMovement();
     } 

    void Update(){
        //handles the camera
        CameraMovement();
            // check if the player has pressed the L key to enable debug mode
        DEBUG = Input.GetKeyDown(KeyCode.L) ? 1 : 0;
         if (DEBUG == 1) debug(); 
    }

    private bool IsGrounded()
    {
        return Physics.Raycast(Model.transform.position, Vector3.down, 0.5f);
    }

    void CameraMovement(){
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 50;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 50;

        // Update angles based on mouse input
        horizontalAngle += mouseX * Time.deltaTime;
        verticalAngle -= mouseY * Time.deltaTime;
        verticalAngle = Mathf.Clamp(verticalAngle, -maxVerticalAngle, maxVerticalAngle);

        // Convert angles to direction
        float x = Mathf.Sin(horizontalAngle * Mathf.Deg2Rad) * Mathf.Cos(verticalAngle * Mathf.Deg2Rad);
        float y = Mathf.Sin(verticalAngle * Mathf.Deg2Rad);
        float z = -Mathf.Cos(horizontalAngle * Mathf.Deg2Rad) * Mathf.Cos(verticalAngle * Mathf.Deg2Rad);

        cameraDirection = new Vector3(-x, y, z);

        // Positions, and rotates camera
        camera.transform.position = Model.transform.position + (cameraDirection * cameraDistance);
        camera.transform.LookAt(Model.transform.position);
    }

    public void PlayerMovement(){
        // get input
        float HorizontalX = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float HorizontalZ = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        float ySpeed = IsGrounded() ? 0 : Player.linearVelocity.y - (gravity * Time.deltaTime);
        Vector3 VerticalMovement = new Vector3(0, ySpeed, 0);
        IsInput = HorizontalX != 0 || HorizontalZ != 0;
        Vector3 cameraForward = camera.transform.forward;
        Vector3 cameraRight = camera.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward.Normalize();
        cameraRight.Normalize();
        Vector3 input = (cameraForward * HorizontalZ + cameraRight * HorizontalX);
        Vector3 desiredMove = desiredMove = input.normalized * speed;
        

         // Apply friction when there is no input
        if (!IsInput)
        {
            IsSlowingDown = true;
            currentVelocity = Vector3.Lerp(currentVelocity, Vector3.zero, friction * Time.deltaTime);
        }
        
        if (IsInput)
        {
            IsSlowingDown = false;
            currentVelocity = Vector3.Lerp(currentVelocity, desiredMove, acceleration * Time.deltaTime);
        }
        
        // move the player character in the direction of the movement vector
        Player.linearVelocity = (currentVelocity + VerticalMovement);

    }
    
    private void debug()
    {
        Debug.Log("Camera Verticle rotation: " + verticalAngle);
        Debug.Log("Player is giving input: " + IsInput);
        Debug.Log("Player Friction: " + IsSlowingDown); 
        Debug.Log("Player velocity: " + currentVelocity); 
        Debug.Log("player is grounded: " + IsGrounded());
    }
}

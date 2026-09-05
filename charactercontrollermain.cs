using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerMain : MonoBehaviour
{
    

   
    private Rigidbody Player;
    GameObject Model;

    // movement variables
    public float speed = 40f;
    public float gravity = 9.81f;
    public float friction = 1.8f; 
    public float acceleration = 0.5f;

    // camera variables
    private Camera camera;
    private float verticalAngle = 20f;  
    public float maxVerticalAngle = 89f;  
    public float mouseSensitivity = 100f;

    // debug variables
    int DEBUG;
    private Vector3 currentVelocity = Vector3.zero;
    private bool IsInput;
    private bool IsSlowingDown;

    void Start(){
            //define camerac player character, and playermodel. and locks them together
            Player = GetComponent<Rigidbody>();
            camera = Camera.main;
            camera.transform.SetParent(transform);
            camera.transform.Rotate(verticalAngle,0,0, Space.Self);
            Model = GameObject.FindWithTag("Player");
            Model.transform.SetParent(transform);
            // Lock cursor to game window
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
    

    public void CameraMovement(){
        // Gets mouse input from user
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        // adjust mouse speed for sensitivity setting
        mouseX *= mouseSensitivity * 50;
        mouseY *= mouseSensitivity * 50;

        // checks what the angle of the camera would be vertically if rotated this frame based on mouse movement, 
        // and clamps it to under +-MaxVerticleMovement
        float newAngle = verticalAngle - mouseY * Time.deltaTime;
        float deltaRotation = newAngle - verticalAngle;
        float maxDelta = maxVerticalAngle - verticalAngle;  
        float minDelta = -maxVerticalAngle - verticalAngle;
        deltaRotation = Mathf.Clamp(deltaRotation, minDelta, maxDelta);

        
        // if clamped angle is different from current angle, rotates camera vertically
        if (deltaRotation != 0)
        {
            camera.transform.RotateAround(transform.position, transform.right, deltaRotation);
            verticalAngle += deltaRotation;
        }
        // rotate the player horozontally around the y axis
        transform.Rotate(0, mouseX * Time.deltaTime, 0);
    }


    public void PlayerMovement(){
        // get input
        float HorizontalX = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float HorizontalZ = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        float ySpeed = Player.linearVelocity.y - (gravity * Time.deltaTime);
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
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterControllerMain : MonoBehaviour
{
    

   
    // characterController is a reference to the CharacterController component attached to the player character
    private CharacterController characterController;
    // camera is a reference to the main camera in the scene
    private Camera camera;
    // speed is a constant that will be used to control the speed of the player character
    public float speed =.1f;
    // gravity is a constant that will be used to simulate the effect of gravity on the player character
    public float gravity = 9.81f;
    // DEBUG is a variable that will be used to enable or disable debug mode
    int DEBUG;

    // Start is called before the first frame update
    void Start()
    {
        //define camera and player character controller
        characterController = GetComponent<CharacterController>();
        camera = Camera.main;
        // lock camera to the player
        camera.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {
        // get input from the player
        float Horizontal = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float Vertical = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;

        // calculate the vertical speed of the player character based on whether they are grounded or not
        float ySpeed = characterController.isGrounded ? 0 : -gravity;
        
        // create a new vector3 to represent the movement direction of the player character
        Vector3 moveDirection = new Vector3(Horizontal, 0, Vertical).normalized;

        // multiply movement by speed
        moveDirection *= speed;

        // create a new vector3 to represent the vertical movement of the player character
        Vector3 VerticalMovement = new Vector3(0, ySpeed, 0);
        
        // move the player character in the direction of the movement vector
        characterController.Move(moveDirection + VerticalMovement * Time.deltaTime);
        
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

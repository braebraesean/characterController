using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterControllerMain : MonoBehaviour
{
    int DEBUG = 0;
    private void debug(Vector3 moveDirection)
    {
        Debug.Log("CharacterControllerMain: " + characterController.isGrounded);
        Debug.Log(moveDirection);
    }

    private CharacterController characterController;
    private Camera camera;
    public float speed =.1f;
    public float gravity = 9.81f;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        camera = Camera.main;
        // lock camera to the player
        camera.transform.SetParent(transform);
    }

    // Update is called once per frame
    void Update()
    {

        float Horizontal = Input.GetKey(KeyCode.D) ? 1 : Input.GetKey(KeyCode.A) ? -1 : 0;
        float Vertical = Input.GetKey(KeyCode.W) ? 1 : Input.GetKey(KeyCode.S) ? -1 : 0;
        float ySpeed = characterController.isGrounded ? 0 : -gravity;
        // multiply movement by speed
        

        Vector3 moveDirection = new Vector3(Horizontal, 0, Vertical).normalized;
        moveDirection *= speed;

        Vector3 VerticalMovement = new Vector3(0, ySpeed, 0);
        characterController.Move(moveDirection + VerticalMovement * Time.deltaTime);
        
        DEBUG = Input.GetKeyDown(KeyCode.L) ? 1 : 0;
        if (DEBUG == 1) debug(moveDirection); 
    }
}

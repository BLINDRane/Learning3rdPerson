using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocomotion : MonoBehaviour
{
    private InputManager input;
    private CharacterController controller;
    private AnimatorHandler animatorHandler;

    public Transform camParent;
    public Transform cam;

    public float speed = 5f;
    public float rotationSpeed = 10f;


    void Start()
    {
        input = InputManager.instance;
        controller = GetComponent<CharacterController>();
        animatorHandler = GetComponent<AnimatorHandler>();

        animatorHandler.Initialize();
    }

    void FixedUpdate()
    {
        HandleMovement(Time.deltaTime);

        if (animatorHandler.canRotate)
        {
            HandleRotation(Time.deltaTime);
        }

        animatorHandler.UpdateAnimatorValues(input.moveAmount, 0);
    }

    private void HandleMovement(float delta)
    {
        //This vector stores a direction to move towards 
        Vector3 movement = (input.move.x * camParent.right) + (input.move.y * camParent.forward);

        controller.Move(movement * speed * delta);
    }

    private void HandleRotation(float delta)
    {
        //the direction we want to be facing
        Vector3 targetDir;

        //dunno
        float moveOverride = input.move.magnitude;

        //first, turn the character forwards or backwards depending on vertical input
        targetDir = cam.forward * input.move.y;
        //next, alter the direction based on horizontal input
        targetDir += cam.right * input.move.x;

        targetDir.Normalize();
        
        //zero out the y direction, we dont want to generate a point up in the air
        targetDir.y = 0;

        //If the target direction is zeroed out, just look forward. 
        if(targetDir == Vector3.zero)
        {
            targetDir = transform.forward;
        }

        //make a rotation that looks towards the target direction vector.
        Quaternion tr = Quaternion.LookRotation(targetDir);

        //interpolate between the current rotation and target rotation
        transform.rotation = Quaternion.Slerp(transform.rotation, tr, rotationSpeed * delta);
    }
}

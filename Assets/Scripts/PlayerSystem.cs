using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    public bool isGravityEnabled = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 onMoveInput(
        float horizontalInput,
        float verticalInput,
        bool isGrounded = true
    )
    {
        //return new Vector3(horizontal,0f,vertical);
        var forwardDirection = transform.forward * verticalInput;
        var rightDirection = transform.right * horizontalInput;

        var verticalSpeed = 0f;
        if(isGravityEnabled && !isGrounded) {
            verticalSpeed = -9.87f;
        }
        var topDirection = transform.up * verticalSpeed;

        return  forwardDirection + rightDirection + topDirection;
    }

    public RotationState onRotateInput(
        float horizontalInput,
        float verticalInput,
        float mouseSensitivity = 1f
    ) {
        return new RotationState(
            new Vector3(verticalInput * mouseSensitivity, 0f, 0f),
            new Vector3(0f, horizontalInput * mouseSensitivity, 0f)
        );
    }
}

public struct RotationState {
    public Vector3 cameraRotation;
    public Vector3 bodyRotation;

    public RotationState(
        Vector3 cameraRotation,
        Vector3 bodyRotation
    ) {
        this.cameraRotation = cameraRotation;
        this.bodyRotation = bodyRotation;
    }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystemShell : MonoBehaviour
{
    public Player player;

    public InputSystem inputSystem;
    private Transform cameraHolder;
    private CharacterController characterController;
    private PlayerSystem playerSystem;
    private bool isGravityEnabled = true;

    void Awake()
    {
        playerSystem = new PlayerSystem();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraHolder = player.cameraHolder;
        characterController = player.characterController;

        setInputSystem(GetComponent<RealInputSystem>());
    }

    public void setInputSystem(InputSystem inputSystem)
    {
        if (this.inputSystem != null)
        {
            this.inputSystem.unsubscribe();
        }
        this.inputSystem = inputSystem;
        this.inputSystem.subscribe(handleInputActions);
    }

    void handleInputActions(InputAction action)
    {
        switch (action)
        {
            case InputAction.Climb a:
                climb(a);
                break;
            case InputAction.Grab a:
                grab(a);
                break;
            case InputAction.Move a:
                move(a);
                break;
            case InputAction.Rotate a:
                rotate(a);
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Move();
        //Rotate();
    }

    void move(InputAction.Move action)
    {
        // float horizontalMove = Input.GetAxis("Horizontal");
        // float verticalMove = Input.GetAxis("Vertical");

        update(
            action: new PlayerAction.OnMoveInput(
                horizontalInput: action.horizontal,
                verticalInput: action.vertical,
                isGrounded: characterController.isGrounded
            )
        );
    }

    void rotate(InputAction.Rotate action)
    {
        // float horizontalRotation = Input.GetAxis("Mouse X");
        // float verticalRotation = Input.GetAxis("Mouse Y");

        update(
            action: new PlayerAction.onRotateInput(
                horizontalInput: action.horizontal,
                verticalInput: action.vertical,
                mouseSensitivity: 1f,
                cameraUpLimit: -50f,
                cameraDownLimit: 50f
            )
        );
    }

    void climb(InputAction.Climb action) { }

    void grab(InputAction.Grab action) { }

    global::PlayerState getCurrentState()
    {
        return new global::PlayerState(
            rotationState: new RotationState(
                cameraRotation: cameraHolder.eulerAngles,
                bodyRotation: Vector3.zero,
                forward: player.transform.forward,
                right: player.transform.right,
                up: player.transform.up
            ),
            translationState: new TranslationState(bodyMovement: Vector3.zero),
            isGravityEnabled: isGravityEnabled
        );
    }

    void update(PlayerAction action)
    {
        var result = playerSystem.functionalCore(getCurrentState(), action);

        applyNewState(result.state);
    }

    void applyNewState(global::PlayerState state)
    {
        characterController.Move(3 * Time.deltaTime * state.translationState.bodyMovement);

        var rotationState = state.rotationState;
        player.transform.Rotate(
            rotationState.bodyRotation.x,
            rotationState.bodyRotation.y,
            rotationState.bodyRotation.z
        );

        cameraHolder.eulerAngles = rotationState.cameraRotation;
    }
}

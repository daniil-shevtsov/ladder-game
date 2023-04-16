using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystemShell : MonoBehaviour
{
    public Player player;
    private Transform cameraHolder;
    private CharacterController characterController;
    private PlayerSystem playerSystem;

    void Awake()
    {
        playerSystem = GetComponent<PlayerSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {
        cameraHolder = player.cameraHolder;
        characterController = player.characterController;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        update(
            action: new PlayerAction.OnMoveInput(
                horizontalInput: horizontalMove,
                verticalInput: verticalMove,
                isGrounded: characterController.isGrounded
            )
        );
    }

    void Rotate()
    {
        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");

        update(
            action: new PlayerAction.onRotateInput(
                horizontalInput: horizontalRotation,
                verticalInput: verticalRotation,
                mouseSensitivity: 1f,
                cameraUpLimit: -50f,
                cameraDownLimit: 50f
            )
        );
    }

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
            translationState: new TranslationState(bodyMovement: Vector3.zero)
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

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSystem : MonoBehaviour
{
    public bool isGravityEnabled = true;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update() { }

    public Vector3 onMoveInput(float horizontalInput, float verticalInput, bool isGrounded = true)
    {
        //return new Vector3(horizontal,0f,vertical);
        var forwardDirection = transform.forward * verticalInput;
        var rightDirection = transform.right * horizontalInput;

        var verticalSpeed = 0f;
        if (isGravityEnabled && !isGrounded)
        {
            verticalSpeed = -9.87f;
        }
        var topDirection = transform.up * verticalSpeed;

        return forwardDirection + rightDirection + topDirection;
    }

    public FunctionalCoreResult functionalCore(PlayerState state, PlayerAction action)
    {
        return action switch
        {
            PlayerAction.Init => new FunctionalCoreResult(state, null),
            PlayerAction.OnMoveInput a
                => new FunctionalCoreResult(
                    state.copy(
                        translationState: new TranslationState(
                            bodyMovement: onMoveInput(a.horizontalInput, a.verticalInput)
                        )
                    ),
                    null
                ),
            _ => new FunctionalCoreResult(state, null)
        };
    }

    public RotationState onRotateInput(
        float horizontalInput,
        float verticalInput,
        float mouseSensitivity = 1f,
        float cameraUpLimit = -50f,
        float cameraDownLimit = 50f,
        Vector3? currentCameraRotation = null
    )
    {
        if (currentCameraRotation == null)
        {
            currentCameraRotation = Vector3.zero;
        }

        var bodyRotation = new Vector3(0f, horizontalInput * mouseSensitivity, 0f);

        var cameraRotation =
            currentCameraRotation.Value
            + new Vector3(-verticalInput * mouseSensitivity, bodyRotation.y, 0f);

        if (cameraRotation.x > 180)
        {
            cameraRotation.x -= 360;
        }

        var clampedCameraRotation = new Vector3(
            Mathf.Clamp(cameraRotation.x, cameraUpLimit, cameraDownLimit),
            cameraRotation.y,
            cameraRotation.z
        );

        return new RotationState(clampedCameraRotation, bodyRotation);
    }
}

public struct RotationState
{
    public Vector3 cameraRotation;
    public Vector3 bodyRotation;

    public RotationState(Vector3 cameraRotation, Vector3 bodyRotation)
    {
        this.cameraRotation = cameraRotation;
        this.bodyRotation = bodyRotation;
    }
}

public struct TranslationState
{
    public Vector3 bodyMovement;

    public TranslationState(Vector3 bodyMovement)
    {
        this.bodyMovement = bodyMovement;
    }
}

public struct PlayerState
{
    public RotationState rotationState;
    public TranslationState translationState;

    public PlayerState(RotationState rotationState, TranslationState translationState)
    {
        this.rotationState = rotationState;
        this.translationState = translationState;
    }

    public PlayerState copy(
        RotationState? rotationState = null,
        TranslationState? translationState = null
    )
    {
        return new PlayerState(
            rotationState: rotationState ?? this.rotationState,
            translationState: translationState ?? this.translationState
        );
    }
}

public interface PlayerAction
{
    struct Init : PlayerAction { };

    struct OnMoveInput : PlayerAction
    {
        public float verticalInput;
        public float horizontalInput;

        public OnMoveInput(float verticalInput, float horizontalInput)
        {
            this.verticalInput = verticalInput;
            this.horizontalInput = horizontalInput;
        }
    }
}

public interface PlayerEvent { }

public struct FunctionalCoreResult
{
    public PlayerState state;
    public PlayerEvent? playerEvent;

    public FunctionalCoreResult(PlayerState state, PlayerEvent? playerEvent)
    {
        this.state = state;
        this.playerEvent = playerEvent;
    }
}

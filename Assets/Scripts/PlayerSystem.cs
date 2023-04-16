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

    private Vector3 onMoveInput(PlayerState state, PlayerAction.OnMoveInput action)
    {
        //return new Vector3(horizontal,0f,vertical);
        var forwardDirection = state.rotationState.forward * action.verticalInput;
        var rightDirection = state.rotationState.right * action.horizontalInput;

        var verticalSpeed = 0f;
        if (isGravityEnabled && !action.isGrounded)
        {
            verticalSpeed = -9.87f;
        }
        var topDirection = state.rotationState.up * verticalSpeed;

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
                        translationState: new TranslationState(bodyMovement: onMoveInput(state, a))
                    ),
                    null
                ),
            PlayerAction.onRotateInput a
                => new FunctionalCoreResult(
                    state.copy(
                        rotationState: onRotateInput(
                            state,
                            a.horizontalInput,
                            a.verticalInput,
                            a.mouseSensitivity,
                            a.cameraUpLimit,
                            a.cameraDownLimit,
                            state.rotationState.cameraRotation
                        )
                    ),
                    null
                ),
            _ => new FunctionalCoreResult(state, null)
        };
    }

    private RotationState onRotateInput(
        PlayerState state,
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

        return new RotationState(
            clampedCameraRotation,
            bodyRotation,
            state.rotationState.forward,
            state.rotationState.right,
            state.rotationState.up
        );
    }
}

public struct RotationState
{
    public Vector3 cameraRotation;
    public Vector3 bodyRotation;

    public Vector3 forward;
    public Vector3 right;
    public Vector3 up;

    public RotationState(
        Vector3 cameraRotation,
        Vector3 bodyRotation,
        Vector3 forward,
        Vector3 right,
        Vector3 up
    )
    {
        this.cameraRotation = cameraRotation;
        this.bodyRotation = bodyRotation;
        this.forward = forward;
        this.right = right;
        this.up = up;
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
        public bool isGrounded;

        public OnMoveInput(float verticalInput, float horizontalInput, bool isGrounded)
        {
            this.verticalInput = verticalInput;
            this.horizontalInput = horizontalInput;
            this.isGrounded = isGrounded;
        }
    }

    struct onRotateInput : PlayerAction
    {
        public float horizontalInput;
        public float verticalInput;
        public float mouseSensitivity;
        public float cameraUpLimit;
        public float cameraDownLimit;

        public onRotateInput(
            float horizontalInput,
            float verticalInput,
            float mouseSensitivity,
            float cameraUpLimit,
            float cameraDownLimit
        )
        {
            this.horizontalInput = horizontalInput;
            this.verticalInput = verticalInput;
            this.mouseSensitivity = mouseSensitivity;
            this.cameraUpLimit = cameraUpLimit;
            this.cameraDownLimit = cameraDownLimit;
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

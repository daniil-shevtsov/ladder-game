using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class PlayerSystemTest : MonoBehaviour
{
    private PlayerSystem playerSystem;

    [OneTimeSetUp]
    public void Setup()
    {
        playerSystem = new PlayerSystem();
    }

    [Test]
    public void ShouldStayWhenZeroInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 0f, verticalInput: 0f)
        );
        AssertEqual(Vector3.zero, result.state.translationState.bodyMovement);
    }

    private PlayerState playerState(
        RotationState? rotationState = null,
        TranslationState? translationState = null
    )
    {
        return new PlayerState(
            rotationState: rotationState ?? rotationStateStub(),
            translationState: translationState ?? new TranslationState()
        );
    }

    private RotationState rotationStateStub(
        Vector3? cameraRotation = null,
        Vector3? bodyRotation = null
    )
    {
        return new RotationState(
            cameraRotation: cameraRotation ?? Vector3.zero,
            bodyRotation: bodyRotation ?? Vector3.zero,
            forward: new Vector3(0f, 0f, 1f),
            right: new Vector3(1f, 0f, 0f),
            up: new Vector3(0f, 1f, 0f)
        );
    }

    private TranslationState translationStateStub(Vector3? bodyMovement = null)
    {
        return new TranslationState(bodyMovement: bodyMovement ?? Vector3.zero);
    }

    private PlayerAction.OnMoveInput onMoveInput(
        float horizontalInput = 0f,
        float verticalInput = 0f,
        bool isGrounded = true
    )
    {
        return new PlayerAction.OnMoveInput(
            horizontalInput: horizontalInput,
            verticalInput: verticalInput,
            isGrounded: isGrounded
        );
    }

    private PlayerAction.onRotateInput onRotateInput(
        float horizontalInput = 0f,
        float verticalInput = 0f,
        float mouseSensitivity = 1f,
        float cameraUpLimit = -360f,
        float cameraDownLimit = 360f
    )
    {
        return new PlayerAction.onRotateInput(
            horizontalInput: horizontalInput,
            verticalInput: verticalInput,
            mouseSensitivity: mouseSensitivity,
            cameraDownLimit: cameraDownLimit,
            cameraUpLimit: cameraUpLimit
        );
    }

    private void AssertEqual(float expected, float actual, float error = 0.1f)
    {
        Assert.AreEqual(expected, actual, error);
    }

    private void AssertEqual(Vector3 expected, Vector3 actual, float error = 0.001f)
    {
        Assert.AreEqual(expected.x, actual.x, "x not the same");
        Assert.AreEqual(expected.y, actual.y, "y not the same");
        Assert.AreEqual(expected.z, actual.z, "z not the same");
    }
}

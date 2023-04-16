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

    [Test]
    public void ShouldMoveForwardWhenHasVerticalInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 0f, verticalInput: 1f)
        );
        AssertEqual(1f, result.state.translationState.bodyMovement.z);
    }

    [Test]
    public void ShouldMoveRightWhenHasHorizontalInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 1f, verticalInput: 1f)
        );
        AssertEqual(1f, result.state.translationState.bodyMovement.x);
    }

    [Test]
    public void ShouldStayWhenOnGroundInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 0f, verticalInput: 1f)
        );
        AssertEqual(0f, result.state.translationState.bodyMovement.y);
    }

    [Test]
    public void ShouldApplyGravityInTheAirInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 0f, verticalInput: 0f, isGrounded: false)
        );
        AssertEqual(-9.87f, result.state.translationState.bodyMovement.y);
    }

    [Test]
    public void ShouldNotApplyGravityWhenGravityDisabled()
    {
        var result = playerSystem.functionalCore(
            playerState(isGravityEnabled: false),
            onMoveInput(horizontalInput: 0f, verticalInput: 0f, isGrounded: false)
        );
        AssertEqual(0f, result.state.translationState.bodyMovement.y);
    }

    [Test]
    public void ShouldHasZeroRotationWithZeroInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onRotateInput(horizontalInput: 0f, verticalInput: 0f)
        );
        Assert.AreEqual(Vector3.zero, result.state.rotationState.bodyRotation);
    }

    [Test]
    public void ShouldRotateBodyWhenHorizontalInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onRotateInput(horizontalInput: 1f, verticalInput: 0f)
        );
        Assert.AreEqual(new Vector3(0f, 1f, 0f), result.state.rotationState.bodyRotation);
    }

    [Test]
    public void ShouldRotateCameraWhenVerticalInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onRotateInput(horizontalInput: 0f, verticalInput: 1f)
        );
        Assert.AreEqual(new Vector3(-1f, 0f, 0f), result.state.rotationState.cameraRotation);
    }

    [Test]
    public void ShouldApplyMouseSensitivityToBodyRotation()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onRotateInput(horizontalInput: 1f, verticalInput: 0f, mouseSensitivity: 2)
        );
        Assert.AreEqual(new Vector3(0f, 2f, 0f), result.state.rotationState.bodyRotation);
    }

    [Test]
    public void ShouldApplyMouseSensitivityToCameraRotation()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onRotateInput(horizontalInput: 0f, verticalInput: 1f, mouseSensitivity: 2)
        );
        Assert.AreEqual(new Vector3(-2f, 0f, 0f), result.state.rotationState.cameraRotation);
    }

    [Test]
    public void ShouldStopAtUpLimitOfCameraRotation()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onRotateInput(horizontalInput: 0f, verticalInput: 51f, cameraUpLimit: -50f)
        );
        Assert.AreEqual(new Vector3(-50f, 0f, 0f), result.state.rotationState.cameraRotation);
    }

    [Test]
    public void ShouldStopAtDownLimitOfCameraRotation()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onRotateInput(horizontalInput: 0f, verticalInput: -51f, cameraDownLimit: 50f)
        );
        Assert.AreEqual(new Vector3(50f, 0f, 0f), result.state.rotationState.cameraRotation);
    }

    [Test]
    public void ShouldRotateCameraWithTheBody()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onRotateInput(horizontalInput: 10f, verticalInput: 0f)
        );
        Assert.AreEqual(new Vector3(0f, 10f, 0f), result.state.rotationState.cameraRotation);
    }

    [Test]
    public void ShouldApplyRotationToCurrentCameraRotation()
    {
        var result = playerSystem.functionalCore(
            playerState(rotationState: rotationStateStub(cameraRotation: new Vector3(20f, 0f, 0f))),
            onRotateInput(horizontalInput: 0f, verticalInput: -10f)
        );
        Assert.AreEqual(new Vector3(30f, 0f, 0f), result.state.rotationState.cameraRotation);
    }

    [Test]
    public void ShouldClampResultingCameraRotation()
    {
        var result = playerSystem.functionalCore(
            playerState(rotationState: rotationStateStub(cameraRotation: new Vector3(40f, 0f, 0f))),
            onRotateInput(horizontalInput: 0f, verticalInput: -11f, cameraDownLimit: 50f)
        );
        Assert.AreEqual(new Vector3(50f, 0f, 0f), result.state.rotationState.cameraRotation);
    }

    [Test]
    public void ShouldCreateInitialState()
    {
        var initialState = playerState();
        var result = playerSystem.functionalCore(initialState, new PlayerAction.Init());
        Assert.AreEqual(initialState, result.state);
    }

    private PlayerState playerState(
        RotationState? rotationState = null,
        TranslationState? translationState = null,
        bool? isGravityEnabled = null
    )
    {
        return new PlayerState(
            rotationState: rotationState ?? rotationStateStub(),
            translationState: translationState ?? new TranslationState(),
            isGravityEnabled: isGravityEnabled ?? true
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

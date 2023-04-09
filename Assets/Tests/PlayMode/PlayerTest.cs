using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest : MonoBehaviour
{
    // private ButtonController controller;
    // private TMPro.TMP_Text result;
    private GameObject player;
    private PlayerSystem playerSystem;

    [OneTimeSetUp]
    public void Setup()
    {
        player = MonoBehaviour.Instantiate(
            Resources.Load<GameObject>("Prefabs/Body"),
            new Vector3(0.0f, 0.0f, 1.0f),
            Quaternion.identity
        );
        playerSystem = player.GetComponent<PlayerSystem>();
    }

    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator ShouldStayWhenZeroInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 0f, verticalInput: 0f)
        );
        AssertEqual(Vector3.zero, result.state.translationState.bodyMovement);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldMoveForwardWhenHasVerticalInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 0f, verticalInput: 1f)
        );
        AssertEqual(1f, result.state.translationState.bodyMovement.z);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldMoveRightWhenHasHorizontalInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 1f, verticalInput: 1f)
        );
        AssertEqual(1f, result.state.translationState.bodyMovement.x);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldStayWhenOnGroundInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 0f, verticalInput: 1f)
        );
        AssertEqual(0f, result.state.translationState.bodyMovement.y);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldApplyGravityInTheAirInput()
    {
        var result = playerSystem.functionalCore(
            playerState(),
            onMoveInput(horizontalInput: 0f, verticalInput: 0f, isGrounded: false)
        );
        AssertEqual(-9.87f, result.state.translationState.bodyMovement.y);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldHasZeroRotationWithZeroInput()
    {
        var rotationState = playerSystem.onRotateInput(0f, 0f);
        Assert.AreEqual(Vector3.zero, rotationState.bodyRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldRotateBodyWhenHorizontalInput()
    {
        var rotationState = playerSystem.onRotateInput(1f, 0f);
        Assert.AreEqual(new Vector3(0f, 1f, 0f), rotationState.bodyRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldRotateCameraWhenVerticalInput()
    {
        var rotationState = playerSystem.onRotateInput(0f, 1f);
        Assert.AreEqual(new Vector3(-1f, 0f, 0f), rotationState.cameraRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldApplyMouseSensitivityToBodyRotation()
    {
        var rotationState = playerSystem.onRotateInput(1f, 0f, 2f);
        Assert.AreEqual(new Vector3(0f, 2f, 0f), rotationState.bodyRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldApplyMouseSensitivityToCameraRotation()
    {
        var rotationState = playerSystem.onRotateInput(0f, 1f, 2f);
        Assert.AreEqual(new Vector3(-2f, 0f, 0f), rotationState.cameraRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldStopAtUpLimitOfCameraRotation()
    {
        var rotationState = playerSystem.onRotateInput(0f, 51f, cameraUpLimit: -50f);
        Assert.AreEqual(new Vector3(-50f, 0f, 0f), rotationState.cameraRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldStopAtDownLimitOfCameraRotation()
    {
        var rotationState = playerSystem.onRotateInput(0f, -51f, cameraDownLimit: 50f);
        Assert.AreEqual(new Vector3(50f, 0f, 0f), rotationState.cameraRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldRotateCameraWithTheBody()
    {
        var rotationState = playerSystem.onRotateInput(10f, 0f);
        Assert.AreEqual(new Vector3(00f, 10f, 0f), rotationState.cameraRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldApplyRotationToCurrentCameraRotation()
    {
        var rotationState = playerSystem.onRotateInput(
            0f,
            -10f,
            currentCameraRotation: new Vector3(20f, 0f, 0f)
        );
        Assert.AreEqual(new Vector3(30f, 0f, 0f), rotationState.cameraRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldClampResultingCameraRotation()
    {
        var rotationState = playerSystem.onRotateInput(
            0f,
            -11f,
            currentCameraRotation: new Vector3(40f, 0f, 0f)
        );
        Assert.AreEqual(new Vector3(50f, 0f, 0f), rotationState.cameraRotation);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldCreateInitialState()
    {
        var initialState = playerState();
        var result = playerSystem.functionalCore(initialState, new PlayerAction.Init());
        Assert.AreEqual(initialState, result.state);
        yield return new WaitForSeconds(0.1f);
    }

    private PlayerState playerState()
    {
        return new PlayerState();
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

    private void AssertEqual(float expected, float actual, float error = 0.001f)
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

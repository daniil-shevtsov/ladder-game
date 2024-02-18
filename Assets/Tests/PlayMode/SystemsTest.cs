using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SystemsTest : MonoBehaviour
{
    private GameObject systemsObject;
    private PlayerSystemShell systems;

    private FakeInputSystem inputSystem = new FakeInputSystem();

    [OneTimeSetUp]
    public void Setup()
    {
        systemsObject = MonoBehaviour.Instantiate(
            Resources.Load<GameObject>("Prefabs/Systems"),
            new Vector3(0.0f, 0.0f, 0.0f),
            Quaternion.identity
        );
        systems = systemsObject.GetComponent<PlayerSystemShell>();
        systems.setInputSystem(inputSystem);
    }

    [UnityTest]
    public IEnumerator ShouldNotCrash()
    {
        yield return new WaitForSeconds(0.0f);
    }

    [UnityTest]
    public IEnumerator ShouldMoveOnMoveInput()
    {
        inputSystem.emulateInput(new InputAction.Move(horizontal: 0f, vertical: 2f));
        AssertEqual(new Vector3(0f, 0f, 1f), systems.player.transform.position);

        // inputSystem.emulateInput(new InputAction.Move(horizontal: 1f, vertical: 0f));
        // AssertEqual(new Vector3(0f, 0f, 0f), systems.player.transform.position);
        yield return new WaitForSeconds(0.0f);
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

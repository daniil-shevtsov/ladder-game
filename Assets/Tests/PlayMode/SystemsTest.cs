using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class SystemsTest : MonoBehaviour
{
    private GameObject systemsObject;
    private PlayerSystemShell systems;

    [OneTimeSetUp]
    public void Setup()
    {
        systemsObject = MonoBehaviour.Instantiate(
            Resources.Load<GameObject>("Prefabs/Systems"),
            new Vector3(0.0f, 0.0f, 0.0f),
            Quaternion.identity
        );
        systems = systemsObject.GetComponent<PlayerSystemShell>();
    }

    [UnityTest]
    public IEnumerator ShouldNotCrash()
    {
        yield return new WaitForSeconds(0.0f);
    }
}

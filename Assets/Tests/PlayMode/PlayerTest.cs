using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerTest : MonoBehaviour
{
    // private ButtonController controller;
    // private TMPro.TMP_Text result;

    [OneTimeSetUp]
    public void Setup() {
        
    }


    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator ButtonControllerTestSimplePasses()
    {
        GameObject playerGameObject = MonoBehaviour.Instantiate (
            Resources.Load<GameObject>("Prefabs/Body"),
            new Vector3( 0.0f, 0.0f, 1.0f),
            Quaternion.identity
        );
        
        yield return new WaitForSeconds(0.1f);
        
    }


}

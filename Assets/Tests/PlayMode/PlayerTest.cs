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
    public void Setup() {
        player = MonoBehaviour.Instantiate (
            Resources.Load<GameObject>("Prefabs/Body"),
            new Vector3( 0.0f, 0.0f, 1.0f),
            Quaternion.identity
        );
        playerSystem = player.GetComponent<PlayerSystem>();
        
    }


    // A Test behaves as an ordinary method
    [UnityTest]
    public IEnumerator ShouldStayWhenZeroInput()
    {
        var movement = playerSystem.onMoveInput(0f, 0f);
        Assert.AreEqual(movement, Vector3.zero);
        yield return new WaitForSeconds(0.1f);
    }


}

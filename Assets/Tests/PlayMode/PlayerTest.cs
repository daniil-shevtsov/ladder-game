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
        Assert.AreEqual( Vector3.zero, movement);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldMoveForwardWhenHasVerticalInput()
    {
        var movement = playerSystem.onMoveInput(0f, 1f);
        Assert.AreEqual(1f, movement.z);
        yield return new WaitForSeconds(0.1f);
    }

    
    [UnityTest]
    public IEnumerator ShouldMoveRightWhenHasHorizontalInput()
    {
        var movement = playerSystem.onMoveInput(1f, 0f);
        Assert.AreEqual(1f, movement.x);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldStayWhenOnGroundInput()
    {
        var movement = playerSystem.onMoveInput(0f, 0f);
        Assert.AreEqual(0f,movement.y);
        yield return new WaitForSeconds(0.1f);
    }

    [UnityTest]
    public IEnumerator ShouldApplyGravityInTheAirInput()
    {
        var movement = playerSystem.onMoveInput(0f, 0f, false);
        Assert.AreEqual(-9.87f, movement.y);
         yield return new WaitForSeconds(0.1f);
    }

}

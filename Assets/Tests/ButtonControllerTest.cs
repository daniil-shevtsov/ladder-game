using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class ButtonControllerTest
{
    private ButtonController controller;
    private TMPro.TMP_Text result;

    [OneTimeSetUp]
    public void Setup() {
        var rootObject = new GameObject();
        controller = rootObject.AddComponent<ButtonController>();
        result = rootObject.AddComponent<TMPro.TMP_Text>();
    }


    // A Test behaves as an ordinary method
    [Test]
    public void ButtonControllerTestSimplePasses()
    {
        controller.handleClick();
        Assert.AreEqual("0", result.text);
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    [UnityTest]
    public IEnumerator ButtonControllerTestWithEnumeratorPasses()
    {
        // Use the Assert class to test conditions.
        // Use yield to skip a frame.
        yield return null;
    }
}


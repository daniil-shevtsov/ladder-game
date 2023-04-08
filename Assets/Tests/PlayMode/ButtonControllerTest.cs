using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using TMPro;

public class ButtonControllerTest
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
        var rootObject = new GameObject();
        var controller = rootObject.AddComponent<ButtonController>();
        var result = rootObject.AddComponent<TMPro.TextMeshProUGUI>();
        var fieldObjectA = new GameObject();
        var fieldObjectB = new GameObject();
        var fieldA = fieldObjectA.AddComponent<TMP_InputField>();
        var fieldB = fieldObjectB.AddComponent<TMP_InputField>();
        controller.fieldA = fieldA;
        controller.fieldB = fieldB;
        controller.result = result;

        fieldA.text = "1";
        fieldB.text = "2";

        controller.handleClick();
        
        yield return new WaitForSeconds(0.1f);

        Assert.AreEqual("3", result.text);
        
    }


}


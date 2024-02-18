using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class RealInputSystem : MonoBehaviour, InputSystem
{
    private Action<InputAction> callback;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");
        callback.Invoke(new InputAction.Move(horizontal: horizontalMove, vertical: verticalMove));

        float horizontalRotation = Input.GetAxis("Mouse X");
        float verticalRotation = Input.GetAxis("Mouse Y");
        callback.Invoke(
            new InputAction.Rotate(horizontal: horizontalRotation, vertical: verticalRotation)
        );

        if (Input.GetKeyDown(KeyCode.G))
        {
            callback.Invoke(new InputAction.Grab());
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            callback.Invoke(new InputAction.Climb());
        }
    }

    void InputSystem.subscribe(Action<InputAction> callback)
    {
        this.callback = callback;
    }

    public void unsubscribe()
    {
        this.callback = null;
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeInputSystem : InputSystem
{
    private Action<InputAction> callback;

    public void subscribe(Action<InputAction> callback)
    {
        this.callback = callback;
    }

    public void emulateInput(InputAction action)
    {
        if (callback != null)
        {
            callback.Invoke(action);
        }
    }

    public void unsubscribe()
    {
        this.callback = null;
    }
}

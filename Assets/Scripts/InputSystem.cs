using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public interface InputSystem
{
    void subscribe(Action<InputAction> callback);
    void unsubscribe();
}

public interface InputAction
{
    struct Move : InputAction
    {
        public float horizontal;
        public float vertical;

        public Move(float horizontal, float vertical)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }
    }

    struct Rotate : InputAction
    {
        public float horizontal;
        public float vertical;

        public Rotate(float horizontal, float vertical)
        {
            this.horizontal = horizontal;
            this.vertical = vertical;
        }
    }

    struct Grab : InputAction { }

    struct Climb : InputAction { }
}

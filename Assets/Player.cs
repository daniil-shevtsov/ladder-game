using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public InputAction grabAction;
    public Camera cc;
    private Item grabbedItem;
    private GrabArea grabArea;

    // Start is called before the first frame update
    void Start()
    {
        grabArea = GetComponent<GrabArea>();
        grabAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if(grabAction.triggered) {
            Debug.Log("GRAB CLICKED");
            ToggleGrab();
        }
    }

    void ToggleGrab() {
        Item grabbed = GrabItem();
        if(grabbed == null) {
            Debug.Log("NOTHING TO GRAB");
        } else if(grabbedItem == null) {
           Debug.Log("GRABBED ITEM");
           grabbedItem = grabbed;
        } else {
            Debug.Log("DROPPED ITEM");
            grabbedItem = null;
        }
    }

    Item GrabItem() {
        if(grabArea.itemsInArea.Count == 0) {
            return null;
        }
        return grabArea.itemsInArea[0];
    }
}

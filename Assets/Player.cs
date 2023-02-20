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

    // Start is called before the first frame update
    void Start()
    {
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
        RaycastHit hit;
        if (Physics.Raycast(cc.transform.position, cc.transform.forward, out hit, 1000)) //here we go agian we shoot raycast in camera position then raycast will appear forward direction when hit something collide with the shooting range 
        {
            
            return hit.transform.GetComponent<Item>();
        }
        return null;
    }
}

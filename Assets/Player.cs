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
    private GameObject heldObject;

    // Start is called before the first frame update
    void Start()
    {
        grabArea = GetComponent<GrabArea>();
        grabAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabAction.triggered)
        {
            Debug.Log("GRAB CLICKED");
            ToggleGrab();
        }

        UpdateGrabbedItem();
    }

    void ToggleGrab()
    {
        if (grabbedItem == null)
        {
            GrabItem();
        }
        else
        {
            DropItem();
        }
    }

    void GrabItem()
    {
        Item itemToGrab = GetItemInGrabArea();
        if (itemToGrab == null)
        {
            Debug.Log("NOTHING TO GRAB");
        }
        else
        {
            Debug.Log("GRABBED ITEM");
            grabbedItem = itemToGrab;
            ToggleCollision(grabbedItem.gameObject, false);
        }
    }

    Item GetItemInGrabArea()
    {
        if (grabArea.itemsInArea.Count == 0)
        {
            return null;
        }
        return grabArea.itemsInArea[0];
    }

    void DropItem()
    {
        if (grabbedItem != null)
        {
            Debug.Log("DROPPED ITEM");
            ToggleCollision(grabbedItem.gameObject, true);
        }
        grabbedItem = null;
    }

    void UpdateGrabbedItem()
    {
        if (grabbedItem != null)
        {
            GameObject heldObject = grabbedItem.gameObject;
            Vector3 direction = (transform.position - heldObject.transform.position).normalized;
            Vector3 newPosition = transform.position + 0.5f * direction;
            heldObject.transform.position = newPosition;
        }
    }

    void ToggleCollision(GameObject gameObject, bool isEnabled)
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = !isEnabled;
    }
}

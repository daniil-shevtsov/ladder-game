using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public InputAction grabAction;
    public InputAction climbAction;
    public Camera cc;

    public float climbingSpeed;

    private Item grabbedItem;
    private GrabArea grabArea;
    private GameObject heldObject;
    private Vector3 handPositionOffset;

    private Ladder nearLadder;
    private CharacterController characterController;
    private bool isClimbing = false;

    // Start is called before the first frame update
    void Start()
    {
        grabArea = GetComponent<GrabArea>();
        characterController = GetComponent<CharacterController>();

        handPositionOffset = new Vector3(1.5f, 1.5f, 1.5f);
        grabAction.Enable();
        climbAction.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        if (grabAction.triggered)
        {
            Debug.Log("GRAB CLICKED");
            ToggleGrab();
        }

        if (climbAction.triggered)
        {
            StartClimbing();
        }

        UpdateGrabbedItem();

        if (nearLadder != null && isClimbing)
        {
            if (Input.GetKey(KeyCode.O))
            {
                characterController.enabled = false;
                Debug.Log("Climb up");
                //characterController.Move(new Vector3(0, 1, 0) * Time.deltaTime * climbingSpeed);
                transform.Translate(new Vector3(0, 1, 0) * Time.deltaTime * climbingSpeed);
            }
            else if (Input.GetKey(KeyCode.P))
            {
                characterController.enabled = false;
                Debug.Log("Climb down");
                //characterController.Move(new Vector3(0, -1, 0) * Time.deltaTime * climbingSpeed);
                transform.Translate(new Vector3(0, -1, 0) * Time.deltaTime * climbingSpeed);
            }
            else
            {
                characterController.enabled = true;
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        Ladder ladder = collision.gameObject.GetComponent<Ladder>();
        if (ladder != null)
        {
            Debug.Log("Walked to ladder");
            nearLadder = ladder;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        Ladder ladder = collision.gameObject.GetComponent<Ladder>();
        if (ladder != null)
        {
            Debug.Log("Walked away from ladder");
            nearLadder = null;
        }
    }

    void StartClimbing()
    {
        Debug.Log("Start climbing");
        isClimbing = true;
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

    void RotateAsPlayer(GameObject gameObject)
    {
        Vector3 handPosition = transform.position + handPositionOffset;
        Quaternion rotation = Quaternion.LookRotation(transform.position - handPosition);

        gameObject.transform.rotation = transform.rotation;
    }

    void RotateByMouse(GameObject gameObject)
    {
        Vector3 distance = Input.mousePosition - gameObject.transform.position;
        Quaternion rotation = Quaternion.LookRotation(distance, Vector3.up);

        Vector3 mouseScreenPosition = new Vector3(
            Input.mousePosition.x,
            Input.mousePosition.y,
            -Camera.main.transform.position.z
        );

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

        gameObject.transform.LookAt(mouseWorldPosition, Vector3.up);
    }

    void UpdateGrabbedItem()
    {
        if (grabbedItem != null)
        {
            GameObject heldObject = grabbedItem.gameObject;
            //Vector3 direction = (transform.position - heldObject.transform.position).normalized;
            Vector3 direction = transform.forward.normalized;
            float distance = Vector3.Distance(
                transform.position,
                transform.position + handPositionOffset
            );
            Vector3 handPosition = transform.position + direction * 3.5f;

            heldObject.transform.position = handPosition;
            heldObject.transform.rotation = Quaternion.LookRotation(transform.forward);

            //RotateAsPlayer(heldObject);
        }
    }

    void ToggleCollision(GameObject gameObject, bool isEnabled)
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.isKinematic = !isEnabled;
        //body.freezeRotation = !isEnabled;
    }
}

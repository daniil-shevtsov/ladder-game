using System.Transactions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
// using StarterAssets;

public class Player : MonoBehaviour
{
    public Camera cc;

    public float climbingSpeed = 3;
    public float dragForceAmount = 500;
    public bool newGrabSystem = false;
    public HingeJoint handHinge; 

    private Item grabbedItem;
    private GrabArea grabArea;
    private GameObject heldObject;
    private Vector3 handPositionOffset;
    

    private Ladder nearLadder;
    // private MyThirdPersonController characterController;
    private CharacterController characterController;
    private PlayerState currentState = PlayerState.Idle;
    private Vector3 originalObjectPosition;
    
    private PlayerSystem playerSystem;

    // Start is called before the first frame update
    void Start()
    {
        grabArea = GetComponent<GrabArea>();
        characterController = GetComponent<CharacterController>();
        playerSystem = GetComponent<PlayerSystem>();
        //handHinge = GetComponent<HingeJoint>();

        handPositionOffset = new Vector3(1.5f, 1.5f, 1.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("GRAB CLICKED");
            ToggleGrab();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("Climb clicked");
            if (currentState == PlayerState.Climbing)
            {
                StopClimbing();
            }
            else if (currentState == PlayerState.Idle && nearLadder != null)
            {
                StartClimbing();
            }
        }

        UpdateGrabbedItem();

        if(nearLadder != null && Input.GetKey(KeyCode.P)) {
            nearLadder.gameObject.GetComponent<Rigidbody>().AddForce(transform.forward, ForceMode.Force);
        }

        if (nearLadder != null && currentState == PlayerState.Climbing)
        {
            Vector3 climbingDirection = nearLadder.gameObject.transform.up;
            Debug.DrawRay(transform.position, climbingDirection, Color.red, 1);
            Debug.DrawRay(transform.position, -climbingDirection, Color.red, 1);

            if (Input.GetKey(KeyCode.W))
            {
                Debug.Log("Climb up");
                GetComponent<CharacterController>().Move(climbingDirection * Time.deltaTime * climbingSpeed);
                //GetComponent<Rigidbody>().MovePosition(climbingDirection * Time.deltaTime * climbingSpeed);
                //transform.Translate(climbingDirection * Time.deltaTime * climbingSpeed, Space.World);
            }
            else if (Input.GetKey(KeyCode.S))
            {
                Debug.Log("Climb down");
                GetComponent<CharacterController>().Move(-climbingDirection * Time.deltaTime * climbingSpeed);
                //GetComponent<Rigidbody>().MovePosition(-climbingDirection * Time.deltaTime * climbingSpeed);
                //transform.Translate(-climbingDirection * Time.deltaTime * climbingSpeed, Space.World);
            }
        }

        
        if(Input.GetKeyDown(KeyCode.U)) {
            JointSpring jointSpring = handHinge.spring;
            jointSpring.targetPosition = 90f;
            handHinge.spring = jointSpring;
        }
        if(Input.GetKeyDown(KeyCode.J)) {
            JointSpring jointSpring = handHinge.spring;
            jointSpring.targetPosition = 0f;
            handHinge.spring = jointSpring;
        }

        Move();
    }

    void Move() {
        float horizontalMove = Input.GetAxis("Horizontal");
        float verticalMove = Input.GetAxis("Vertical");

        var movement = playerSystem.onMoveInput(horizontalMove, verticalMove);

        characterController.Move(3 * Time.deltaTime * movement);
    }

     void OnTriggerEnter(Collider collision)
    {
        Ladder ladder = collision.gameObject.GetComponent<Ladder>();
        if (ladder != null)
        {
            Debug.Log("Walked to ladder");
            nearLadder = ladder;
        }
    }

    void OnTriggerExit(Collider collision)
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
        // characterController.enabled = false;
        //characterController.isGravityEnabled = false;
        currentState = PlayerState.Climbing;
        transform.parent = nearLadder.transform;
    }

    void StopClimbing()
    {
        Debug.Log("Stop climbing");
        //characterController.isGravityEnabled = true;
        currentState = PlayerState.Idle;
        transform.parent = null;
    }

    void ToggleGrab()
    {
        
        ChangeGrabState();
    }

    void ChangeGrabState() {

        if (grabbedItem == null)
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
                if(newGrabSystem) {
                    //itemToGrab.gameObject.transform.eulerAngles = new Vector3(-90f, -90f, 0f);
                    handHinge.connectedBody = itemToGrab.gameObject.GetComponent<Rigidbody>();
                    handHinge.axis = new Vector3(1,0,0);
                } else {
                    ToggleCollision(grabbedItem.gameObject, false);
                }
            }
        }
        else
        {
             Debug.Log("DROPPED ITEM");
            if(newGrabSystem) {
                handHinge.connectedBody = null;
            } else {
                ToggleCollision(grabbedItem.gameObject, true);
            }
            grabbedItem = null;
        }
    }

    Item GetItemInGrabArea()
    {
        Debug.Log($"Trying to grab but count {grabArea.itemsInArea.Count}");
        if (grabArea.itemsInArea.Count == 0)
        {
            return null;
        }
        return grabArea.itemsInArea[0];
    }

    // void RotateAsPlayer(GameObject gameObject)
    // {
    //     Vector3 handPosition = transform.position + handPositionOffset;
    //     Quaternion rotation = Quaternion.LookRotation(transform.position - handPosition);

    //     gameObject.transform.rotation = transform.rotation;
    // }

    // void RotateByMouse(GameObject gameObject)
    // {
    //     Vector3 distance = Input.mousePosition - gameObject.transform.position;
    //     Quaternion rotation = Quaternion.LookRotation(distance, Vector3.up);

    //     Vector3 mouseScreenPosition = new Vector3(
    //         Input.mousePosition.x,
    //         Input.mousePosition.y,
    //         -Camera.main.transform.position.z
    //     );

    //     Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(mouseScreenPosition);

    //     gameObject.transform.LookAt(mouseWorldPosition, Vector3.up);
    // }

    void UpdateGrabbedItem()
    {
        if (grabbedItem != null && !newGrabSystem)
        {
            GameObject heldObject = grabbedItem.gameObject;
            Vector3 direction = transform.forward.normalized;
            float distance = Vector3.Distance(
                transform.position,
                transform.position + handPositionOffset
            );
            Vector3 handPosition = transform.position + direction * 3.5f;

            heldObject.transform.position = handPosition;
           heldObject.transform.rotation = Quaternion.LookRotation(transform.forward);
        }
    }

    void ToggleCollision(GameObject gameObject, bool isEnabled)
    {
        Rigidbody body = gameObject.GetComponent<Rigidbody>();
        body.isKinematic = !isEnabled;
    }

    enum PlayerState
    {
        Idle,
        Climbing,
    }
}

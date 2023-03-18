using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Transform hammerHead;
    public Transform body;
    public Vector3 hammerRotation;
    public Vector3 hammerPosition;

    private Transform originalHammerTransform;
    private Quaternion originalRotation;

    public float maxRange = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
         Physics.IgnoreCollision(hammerHead.GetComponent<Collider>(),
                                  body.GetComponent<Collider>());
        originalHammerTransform = hammerHead;
        originalRotation = hammerHead.rotation;
    }

    // Update is called once per frame
    void FixedUpdate() {
        // Screen center and mouse position in screen space
        float depth = Mathf.Abs(Camera.main.transform.position.z);
        Vector3 center =
            new Vector3(Screen.width / 2, Screen.height / 2, depth);
        Vector3 mouse =
            new Vector3(Input.mousePosition.x, Input.mousePosition.y, depth);

        // Transform to world space
        center = Camera.main.ScreenToWorldPoint(center);
        mouse = Camera.main.ScreenToWorldPoint(mouse);
        Debug.DrawLine(transform.position, center, Color.red, 1);
        Debug.DrawLine(transform.position, mouse, Color.blue, 1);

        // Compute mouseVec for hammer control
        Vector3 mouseVec = Vector3.ClampMagnitude(mouse - center, maxRange);
        Vector3 desiredHammerDirection = mouse - transform.position;

        Quaternion orientation = Quaternion.LookRotation(desiredHammerDirection, Vector3.up);

        // Compute new hammer pos
        Vector3 newHammerPos = body.position + desiredHammerDirection;
        Vector3 hammerMoveVec = newHammerPos - hammerHead.position;
        newHammerPos = transform.position; //hammerHead.position + hammerMoveVec * 0.2f;
        Vector3 newHammerRotation = hammerRotation;

        // Update hammer pos
        //hammerHead.GetComponent<Rigidbody>().MovePosition(newHammerPos);

       // hammerHead.transform.position = transform.position;//originalHammerTransform.position;
        hammerHead.transform.rotation = orientation;//originalRotation;


        Debug.DrawLine(hammerHead.transform.position, hammerHead.transform.position + hammerHead.transform.up.normalized * 2, Color.green, 1);
        Debug.DrawLine(hammerHead.transform.position, hammerHead.transform.position + hammerHead.transform.right.normalized * 2, Color.red, 1);
        Debug.DrawLine(hammerHead.transform.position, hammerHead.transform.position + hammerHead.transform.forward.normalized * 2, Color.blue, 1);
    }
}

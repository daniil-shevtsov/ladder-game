using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Transform hammerHead;
    public Transform body;
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

        // Collider[] results = Physics.OverlapSphere(hammerHead.transform.position, 0.3f);
        // if(results.Length > 0)
        // {
        //     // Update body pos
        //     Vector3 targetBodyPos = mouse - hammerHead.position;
        //     Debug.DrawLine(hammerHead.position, targetBodyPos, Color.yellow, 1);

        //     Vector3 force = (targetBodyPos - body.position) * 80.0f;
        //     body.GetComponent<Rigidbody>().AddForce(force);

        //     body.GetComponent<Rigidbody>().velocity = Vector2.ClampMagnitude(
        //         body.GetComponent<Rigidbody>().velocity, 6);
        // }

        // Compute new hammer pos
        Vector3 newHammerPos = body.position + desiredHammerDirection;
        Vector3 hammerMoveVec = newHammerPos - hammerHead.position;
        newHammerPos = hammerHead.position + hammerMoveVec * 0.2f;

        // Update hammer pos
        hammerHead.GetComponent<Rigidbody>().MovePosition(newHammerPos);
        hammerHead.transform.position = originalHammerTransform.position;
        hammerHead.transform.rotation = originalRotation;
        // Update hammer rotation
        //hammerHead.rotation = Quaternion.FromToRotation( Vector3.forward, newHammerPos - body.position);









        //hammerHead.transform.LookAt(body.position);
        //hammerHead.transform.LookAt(body.position, -Vector3.forward);
        //body.transform.LookAt(hammerHead.position, -Vector3.right);
        // Vector3 direction = body.position - hammerHead.position;
        // //body.rotation = Quaternion.LookRotation(direction,Vector3.up);
        // //hammerHead.transform.RotateAround(body.position,Vector3.up, 0);

        // // hammerHead.forward = body.forward;

        // hammerHead.eulerAngles =  new Vector3(
        //     body.eulerAngles.x - 90,
        //     body.eulerAngles.y,
        //     body.eulerAngles.z
        // );
    }
}

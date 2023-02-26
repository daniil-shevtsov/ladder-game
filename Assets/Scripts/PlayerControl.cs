using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{

    public Transform hammerHead;
    public Transform body;

    public float maxRange = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
         Physics.IgnoreCollision(hammerHead.GetComponent<Collider>(),
                                  body.GetComponent<Collider>());
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

        // Compute mouseVec for hammer control
        Vector3 mouseVec = Vector3.ClampMagnitude(mouse - center, maxRange);

        // hammerHead.GetComponent<Rigidbody2D>().MovePosition(body.position +
        // mouseVec); return;

        // Check if hammer head is collided with scene objects
        // ContactFilter contactFilter = new ContactFilter();
        // contactFilter.useLayerMask = true;
        // contactFilter.layerMask = LayerMask.GetMask("Default");
        Collider[] results = Physics.OverlapSphere(hammerHead.transform.position, 5f);
        // if (hammerHead.GetComponent<Rigidbody>().OverlapCollider(
        //         contactFilter, results) > 0)  // If collided with scene objects
        if(results.Length > 0)
        {
            // Update body pos
            Vector3 targetBodyPos = hammerHead.position - mouseVec;

            Vector3 force = (targetBodyPos - body.position) * 80.0f;
            body.GetComponent<Rigidbody>().AddForce(force);

            body.GetComponent<Rigidbody>().velocity = Vector2.ClampMagnitude(
                body.GetComponent<Rigidbody>().velocity, 6);
        }

        // Compute new hammer pos
        Vector3 newHammerPos = body.position + mouseVec;
        Vector3 hammerMoveVec = newHammerPos - hammerHead.position;
        newHammerPos = hammerHead.position + hammerMoveVec * 0.2f;

        // Update hammer pos
        hammerHead.GetComponent<Rigidbody>().MovePosition(newHammerPos);

        // Update hammer rotation
        hammerHead.rotation = Quaternion.FromToRotation(
            Vector3.right, newHammerPos - body.position);
    }
}

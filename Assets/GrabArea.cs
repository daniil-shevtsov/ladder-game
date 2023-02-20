using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabArea : MonoBehaviour
{
    public List<Item> itemsInArea;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnCollisionEnter(Collider collider) {
        Debug.Log("COLLISION");
        if(!itemsInArea.Contains(collider.gameObject.GetComponent<Item>())) {
            itemsInArea.Add(collider.gameObject.GetComponent<Item>());
            Debug.Log("Count: " + itemsInArea.Count);
        }
     }

    void OnCollisionExit(Collider collider) {
        if(itemsInArea.Contains(collider.gameObject.GetComponent<Item>())) {
            itemsInArea.Remove(collider.gameObject.GetComponent<Item>());
        }
    }   

}

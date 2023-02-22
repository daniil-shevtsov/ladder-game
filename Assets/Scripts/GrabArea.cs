using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabArea : MonoBehaviour
{
    public List<Item> itemsInArea;

    //public Collider grabAreaCollider;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

     void OnTriggerEnter(Collider collision)
    {
         Debug.Log("grab area trigger enter");
         Item item = collision.gameObject.GetComponent<Item>();
        if(item != null && !itemsInArea.Contains(item)) {
            itemsInArea.Add(item);
            Debug.Log("Count: " + itemsInArea.Count);
            Debug.Log(item);
        }
    }

    void OnTriggerExit(Collider collision)
    {
         Debug.Log("grab area trigger exit");
       if(itemsInArea.Contains(collision.gameObject.GetComponent<Item>())) {
            Debug.Log("remove item from area");
            itemsInArea.Remove(collision.gameObject.GetComponent<Item>());
        }
    }

}

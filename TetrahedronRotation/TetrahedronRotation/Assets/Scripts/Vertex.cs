using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vertex : MonoBehaviour
{

  
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains( "Vertex"))
        {
            var v = other.transform.gameObject;
            v.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
            v.tag = "ColliderBall";
        }
    }
  
    void OnTriggerStay(Collider other)
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Vertex"))
        {
            var v = other.transform.gameObject;
            v.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;

            transform.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
            v.tag = "UnColliderBall";
        }
        // Debug.Log("No longer in contact with " + other.transform.name);
    }
}

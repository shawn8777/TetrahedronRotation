using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FpCollider : MonoBehaviour {
    public Material _sharedface;
    public Material _free;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Sphere(Clone)(Clone)")
        {
            var v = other.gameObject.transform;
            var t = v.parent;
            //Debug.Log(t);
            v.GetComponent<MeshRenderer>().material = _sharedface;
            this.GetComponent<MeshRenderer>().material = _sharedface;
            v.gameObject.tag = "ColliderBall";
        }
    }
  
    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Sphere(Clone)(Clone)")
        {
            var v = other.gameObject.transform;
            var t = v.parent;
            v.GetComponent<MeshRenderer>().material = _free;
            this.GetComponent<MeshRenderer>().material = _free;
            v.gameObject.tag = "UnColliderBall";
        }
        // Debug.Log("No longer in contact with " + other.transform.name);
    }
}

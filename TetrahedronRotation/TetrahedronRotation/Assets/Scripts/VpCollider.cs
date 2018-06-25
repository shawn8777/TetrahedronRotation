using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class VpCollider : MonoBehaviour
{
    public Material _sharedPoint;
    public Material _orignal;
    public readonly Dictionary<Transform, List<Transform>> _sharedEdgeTetra = new Dictionary<Transform, List<Transform>>();
    List<Transform> _Axis = new List<Transform>();

    //PriorityQueue<float, List<Transform>> _queue;

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Edge(Clone)")
        {
            var v = other.gameObject.transform;
            var t = v.parent;
            _Axis.Add(v);          
            _sharedEdgeTetra.Add(t, _Axis);
            CheckNeighborFace(v);
            Debug.Log(t);
            v.GetComponent<MeshRenderer>().material = _sharedPoint;
            this.GetComponent<MeshRenderer>().material = _sharedPoint;
            v.gameObject.tag = "ColliderBall";
        }
    }
    void CheckNeighborFace(Transform v)
    {
        var m = v.GetComponentInParent<MeshFilter>().mesh;
    }
    void OnTriggerStay(Collider other)
    {
        
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Edge(Clone)")
        {
            var v = other.gameObject.transform;
            var t = v.parent;
            _Axis.Remove(v);
            
            v.GetComponent<MeshRenderer>().material = _orignal;
            this.GetComponent<MeshRenderer>().material = _orignal;
            v.gameObject.tag = "UnColliderBall";
        }
       // Debug.Log("No longer in contact with " + other.transform.name);
    }



}


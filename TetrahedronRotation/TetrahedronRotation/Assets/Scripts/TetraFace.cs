using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TetraFace :MonoBehaviour
{
    //public readonly Transform vertex1;
    //public readonly Transform vertex2;
    //public readonly Transform vertex3;
    public readonly TetraEdge Edge1;
    public readonly TetraEdge Edge2;
    public readonly TetraEdge Edge3;
    public readonly Transform FacePoint;

    public Material _sharedface;
    public Material _free;

    public TetraFace (TetraEdge T1, TetraEdge T2, TetraEdge T3, Transform F)
    {
        //vertex1 = v1;
        //vertex2 = v2;
        //vertex3 = v3;
        Edge1 = T1;
        Edge2 = T2;
        Edge3 = T3;
        FacePoint = F;

    }

    //public Vector3 FaceCenter()
    //{
    //    return (vertex1.position + vertex2.position + vertex3.position) / 3;
    //}

    public List<TetraFace> GetFace()
    {
        return new List<TetraFace>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "FacePoint(Clone)"|| other.gameObject.name == "Ground")
        {
            if (other.gameObject.name == "FacePoint(Clone)")
            {
                var v = other.gameObject.transform;
                //var t = v.parent;
                Debug.Log("faceconncect");
                v.GetComponent<MeshRenderer>().material = _sharedface;             
            }
            this.GetComponent<MeshRenderer>().material = _sharedface;
            this.gameObject.tag = "Occupied";
        }
    }

    void OnTriggerStay(Collider other)
    {

    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "FacePoint(Clone)" || other.gameObject.name == "Ground")
        {
            var v = other.gameObject.transform;
            var t = v.parent;
            Debug.Log("facedisconncect");
            v.GetComponent<MeshRenderer>().material = _free;
            this.GetComponent<MeshRenderer>().material = _free;
            this.gameObject.tag = "Free";
        }
        // Debug.Log("No longer in contact with " + other.transform.name);
    }
}

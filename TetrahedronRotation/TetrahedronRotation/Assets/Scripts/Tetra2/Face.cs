using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Face : MonoBehaviour
{
    //public readonly Transform vertex1;
    //public readonly Transform vertex2;
    //public readonly Transform vertex3;
    ////public readonly TetraEdge Edge1;
    ////public readonly TetraEdge Edge2;
    ////public readonly TetraEdge Edge3;
    ////public readonly Transform FacePoint;

    public Material _sharedface;
    public Material _free;

    //public Face(Transform v1, Transform v2, Transform v3)
    //{
    //    vertex1 = v1;
    //    vertex2 = v2;
    //    vertex3 = v3;
    //    //Edge1 = T1;
    //    //Edge2 = T2;
    //    //Edge3 = T3;
    //    //FacePoint = F;

    //}

    //public Vector3 GetCenter()
    //{
    //    return (vertex1.position + vertex2.position + vertex3.position) / 3;
    //}

    public List<TetraFace> GetFace()
    {
        return new List<TetraFace>();
    }
    public string GetName()
    {
        return transform.name;
    }
    public Transform GetTransform()
    {
        return transform;
    }
    public string GetTag()
    {
        return transform.tag;
    }
    string N;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("F") || other.gameObject.name.Contains( "Ground"))
        {
            var v = other.transform.gameObject;
            var n = v.name;
            N = n;
            if (v.name.Contains( "F"))
            {               
                v.GetComponent<MeshRenderer>().material = _sharedface;
                var m = transform.parent.name;
                v.name = n + "ConnectWith" + m;
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
        if (other.gameObject.name.Contains("F") || other.gameObject.name.Contains("Ground"))
        {
            var v = other.gameObject.transform;
            v.name = N;
            var t = v.parent;
            //Debug.Log("facedisconncect");
            v.GetComponent<MeshRenderer>().material = _free;
            this.GetComponent<MeshRenderer>().material = _free;
            this.gameObject.tag = "Free";
        }
    }
}

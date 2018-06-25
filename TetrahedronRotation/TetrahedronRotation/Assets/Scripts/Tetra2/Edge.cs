using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Edge : MonoBehaviour
{
    // Variables
    public readonly Transform _start;
    public readonly Transform _end;

    private Transform _edge;
    public Material _sharedEdge;
    public Material _orignal;
   
    public List<Transform> _Axis;
    public List<string> _object;

    Transform[] P = new Transform[1];
   



    // Methods
    public void Awake()
    {
        _Axis = new List<Transform>();
        _object = new List<string>();
        var tf = this.transform;
    }

    public Edge(Transform start, Transform end)
    {
        _start = start;

        _end = end;

        //var v0 = start.position;
        //var v1 = end.position;

        //_edge = Instantiate(start);
        //_edge.parent = start.parent;
        //_edge.position = (v0 + v1) / 2;

    }

    //get vertex position of the vertex
    public List<Vector3> VertPositions()
    {
        return new List<Vector3>() { _start.position, _end.position };
    }


    public List<Transform> GetPoints()
    {
        return new List<Transform>() { _start, _end };
    }

    //get midpoint of tetra
    public Vector3 GetCenter()
    {
        return 0.5f * (_start.position + _end.position);
    }
    //get transform
    public Transform GetTransform()
    {
        return transform;
    }
    //get tag 
    public string GetTag()
    {
        return transform.tag;
    }
    //get name 
    public string GetName()
    {
        return this.transform.name;
    }
    //get sharededge direction
    public Vector3 GetDirection()
    {
        return _end.position - _start.position;
    }
    public Transform GetCenterXform()
    {
        return _edge;
    }
    string N;
    string Name;
    //get name 
    public string GetOtherName()
    {
        return Name;
    }
    

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Edge"))
        {
            var v = other.transform;
            var p = v.parent;
            var m = transform.parent.name;
            var n = v.name;         
            N = n;
            Name = v.parent.name;
            v.GetComponent<MeshRenderer>().material = _sharedEdge;
            this.GetComponent<MeshRenderer>().material = _sharedEdge;

            v.name = n+"ShareWith" +m;
            v.gameObject.tag = "ColliderBall";
        }

    }


    void OnTriggerStay(Collider other)
    {
       
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Edge"))
        {
            var v = other.transform;
            v.name = N;
            v.GetComponent<MeshRenderer>().material = _orignal;
            this.GetComponent<MeshRenderer>().material = _orignal;
            v.gameObject.tag = "UnColliderBall";
        }
       // Debug.Log("No longer in contact with " + other.transform.name);
    }


}

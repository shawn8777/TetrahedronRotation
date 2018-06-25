using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraEdge:MonoBehaviour
{
    // Variables
    public readonly Transform _start;
    public readonly Transform _end;

    private Transform _edge;
    public GameObject _trigger;
    public Material _sharedEdge;
    public Material _orignal;
    public readonly Dictionary<Transform, List<Transform>> _sharedEdgeTetra = new Dictionary<Transform, List<Transform>>();

    public List<Transform> _Axis;
    public List<string> _object;
   

    // Methods
    public void Awake()
    {
        _Axis = new List<Transform>();
        _object = new List<string>();
        var tf = this.transform;
    }

    public TetraEdge(Transform start,Transform end)
    {
        _start = start;

        _end = end;

        var v0 = start.position;
        var v1 = end.position;

        _edge = Instantiate(start);
        _edge.parent = start.parent;
        _edge.position = (v0 + v1) / 2;
       
    }

    //get vertex position of the vertex
    public List<Vector3> VertPositions()
    {
        return new List<Vector3>(){_start.position,_end.position};
    }


    public List<Transform> GetPoints()
    {
        return new List<Transform>(){_start,_end};
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
    //get sharededge direction
    public Vector3 GetDirection()
    {
        return _end.position - _start.position;
    }

    public Transform GetCenterXform()
    {
        return _edge;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name.Contains("Edge"))
        {
            var v = other.transform;
          
            Debug.Log(other.gameObject.name);
             
            v.GetComponent<MeshRenderer>().material = _sharedEdge;
            this.GetComponent<MeshRenderer>().material = _sharedEdge;
         
            v.gameObject.tag = "ColliderBall";
          
            _Axis.Add(v.gameObject.transform);
        
            Debug.Log(_Axis.Count);
        }
        
    }

   
    void OnTriggerStay(Collider other)
    {
        //var f = fjt[0];
        //Destroy(f);
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name == "Edge(Clone)")
        {
            var v = other.transform.gameObject;

            v.GetComponent<MeshRenderer>().material = _orignal;
            this.GetComponent<MeshRenderer>().material = _orignal;
            
            v.gameObject.tag = "UnColliderBall";
        }
        Debug.Log("No longer in contact with " + other.transform.name);
    }


}

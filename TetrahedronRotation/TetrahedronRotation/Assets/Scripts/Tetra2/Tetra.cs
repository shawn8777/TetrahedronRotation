using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class Tetra : MonoBehaviour
{
    private Mesh _mesh;
    //top vertex
    public Transform v0;
    public Transform v1;
    public Transform v2;
    public Transform v3;
    //edge
    public Edge E0;
    public Edge E1;
    public Edge E2;
    public Edge E3;
    public Edge E4;
    public Edge E5;
    //face
    public Face F0;
    public Face F1;
    public Face F2;
    public Face F3;
    //edge point
    public Transform e0;
    public Transform e1;
    public Transform e2;
    public Transform e3;
    public Transform e4;
    public Transform e5;
    //target
    public GameObject Target;
    public Transform rotateBall;
    
    Dictionary<Transform, List<Transform>> _edgeface;
    Dictionary<Edge, List<Face>> Edgeface;
   
   // List<Transform> _axis;
    List<Transform> _fp;
    List<Transform> _vertexs;
    List<Transform> _face;
    List<Transform> _edge;
   // List<Transform> _other;

    List<Edge> Edge;

    public String others;
   

    public void Awake()
    {
        _edgeface = new Dictionary<Transform, List<Transform>>();
        //_axis = new List<Transform>();
        _fp = new List<Transform>();
        _vertexs = new List<Transform>();  
        _face = new List<Transform>();
        _edge = new List<Transform>();
       // _other = new List<Transform>();
        Edge = new List<Edge>();
        Edgeface = new Dictionary<Edge, List<Face>>();
    }

    public void Start()
    {
        //collect the edge
        Edge.Add(E0);
        Edge.Add(E1);
        Edge.Add(E2);
        Edge.Add(E3);
        Edge.Add(E4);
        Edge.Add(E5);
        //collect edge and face
        List<Face> Ef0 = new List<Face>();
        List<Face> Ef1 = new List<Face>();
        List<Face> Ef2 = new List<Face>();
        List<Face> Ef3 = new List<Face>();
        List<Face> Ef4 = new List<Face>();
        List<Face> Ef5 = new List<Face>();
        //for E0
        Ef0.Add(F0);
        Ef0.Add(F3);
        //for E1
        Ef1.Add(F0);
        Ef1.Add(F1);
        //for E2
        Ef2.Add(F0);
        Ef2.Add(F2);
        //for E3
        Ef3.Add(F1);
        Ef3.Add(F3);
        //for E4
        Ef4.Add(F1);
        Ef4.Add(F2);
        //for E5
        Ef5.Add(F2);
        Ef5.Add(F3);
        //add edge and face 
        Edgeface.Add(E0, Ef0);
        Edgeface.Add(E1, Ef1);
        Edgeface.Add(E2, Ef2);
        Edgeface.Add(E3, Ef3);
        Edgeface.Add(E4, Ef4);
        Edgeface.Add(E5, Ef5);
        //vertex
        var p0 = v0.position;
        var p1 = v1.position;
        var p2 = v2.position;
        var p3 = v3.position;
        _vertexs.Add(v0);
        _vertexs.Add(v1);
        _vertexs.Add(v2);
        _vertexs.Add(v3);
        //face
        var fp0 = F0.transform;
        var fp1 = F1.transform;
        var fp2 = F2.transform;
        var fp3 = F3.transform;
        _face.Add(fp0);
        _face.Add(fp1);
        _face.Add(fp2);
        _face.Add(fp3);
        //edge
        _edge.Add(E0.transform);
        _edge.Add(E1.transform);
        _edge.Add(E2.transform);
        _edge.Add(E3.transform);
        _edge.Add(E4.transform);
        _edge.Add(E5.transform);
        print(_edge.Count);
    }
    //initialize the tetra
    public void Initialize(Vector3 p)
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        transform.localPosition = p;
    }
    private void FixedUpdate()
    {
        var p0 = v0.position;
        var p1 = v1.position;
        var p2 = v2.position;
        var p3 = v3.position;
        //face
        F0.transform.position = (p0 + p1 + p2) / 3;
        F1.transform.position = (p1 + p2 + p3) / 3;
        F2.transform.position = (p0 + p2 + p3) / 3;
        F3.transform.position = (p0 + p1 + p3) / 3;
        //Edge
        E0.transform.position = (p0 + p1) / 2;
        E1.transform.position = (p1 + p2) / 2;
        E2.transform.position = (p0 + p2) / 2;
        E3.transform.position = (p1 + p3) / 2;
        E4.transform.position = (p2 + p3) / 2;
        E5.transform.position = (p0 + p3) / 2;
        //EdgePoint
        e0.transform.position = (p0 + p1) / 2;
        e1.transform.position = (p1 + p2) / 2;
        e2.transform.position = (p0 + p2) / 2;
        e3.transform.position = (p1 + p3) / 2;
        e4.transform.position = (p2 + p3) / 2;
        e5.transform.position = (p0 + p3) / 2;
    }

    //get rotate edge length
    public float GetEdgeLength()
    {
        var v = GetComponent<MeshFilter>().mesh.vertices;

        return (v[0] - v[1]).magnitude;
    }
   // get rotation angle
    public float GetAngle(Transform from, Transform to, Transform edge)
    {
        var v0 = edge.position;
        var F = from.position - v0;
        var T = to.position - v0;
        var D = (from.position - to.position).magnitude;
        var E = GetEdgeLength();
        var A = Vector3.Angle(F, T);
        print(A);
        if (D > E)
        {
            A = 360 - A;
        }
        return A;
    }
    //check whether close enough
    bool CloseEnoughPoint(Transform v0, Transform v1)
    {
        bool close;
        if (Vector3.Distance(v0.position, v1.position) < 0.0001f)
        {
            close = true;
        }
        else
        {
            close = false;
        }
        return close;
    }

    Queue<float> _dis = new Queue<float>();
    Dictionary<float, Edge> _edgeDis = new Dictionary<float, Edge>();
    List<Edge> Axis = new List<Edge>();

    //checkedge 
    public void Checkedge()
    {
        foreach(var e in Edge)
        {           
            if (e.GetName().Contains("ShareWith"))
            {
                var tr = e.GetTransform();
                var d = (Target.transform.position - tr.position).magnitude;
                _dis.Enqueue(d);
                _edgeDis.Add(d, e);
            }          
        }
        var dis = _dis.Max();
        var sharedEdge = _edgeDis[dis];
        Axis.Add(sharedEdge);
        print(Axis.Count);
        GetFace(sharedEdge);
        var n = sharedEdge.GetOtherName();
        others = n;
        print(others);
    }

    public void GetFace(Edge e)
    {
        var f = Edgeface[e];     
        Evaulate(f);
    }

    public void CheckNeighbor(Tetra T)
    {
        foreach(var e in T.Edge)
        {
            if(e.GetName().Contains(this.transform.name))
            {
                var f = T.Edgeface[e];
                Evaulate(f);
            }
        }
    }

    public void Evaulate(List<Face> f)
    {
        foreach (var fp in f)
        {
            if (fp.GetTag()=="Free")
            {
                _fp.Add(fp.GetTransform());
            }
        }
        print(_fp.Count);
    }

    public void Rotate()
    {
        var tr = Axis[0].GetTransform();
        var r = Instantiate(rotateBall);
        r.localPosition = tr.position;
        r.localScale = 0.08f * Vector3.one;
        var p = E0.transform.parent;
        r.parent = p.parent;
        //print(p);
        p.parent = r;
        var a = GetAngle(_fp[0], _fp[1], tr);
        r.Rotate(-tr.up, a);
        p.parent = r.parent;
        CleanList(r);
    }

    public void CleanList(Transform r)
    {
        var q = r.gameObject;
        Destroy(q);
        Axis.Clear();
        _fp.Clear();
        _dis.Clear();
        _edgeDis.Clear();
        //print(_dis.Count);
        // _edgeface.Clear();
    }

    public void Move()
    {

    }

}


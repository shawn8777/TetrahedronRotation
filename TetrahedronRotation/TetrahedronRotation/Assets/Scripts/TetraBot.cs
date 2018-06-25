using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class TetraBot : MonoBehaviour
{

    private Mesh _mesh;
    [SerializeField] private Transform _Vertex;
    [SerializeField] private TetraEdge _edge;
    [SerializeField] private TetraFace _fpVertex;

    public Transform Target;
    public List<TetraFace> _face = new List<TetraFace>(4);

    // Dictionary<Transform, Queue<TetraFace>> _sharededge = new Dictionary<Transform, Queue<TetraFace>>();
    Dictionary<Transform, TetraFace> _sharedEdge = new Dictionary<Transform, TetraFace>();
    Dictionary<float, Transform> _pointDis = new Dictionary<float,Transform>();
    //get sharededge
    //public TetraEdge GetSharedEdge(TetraBot nb, int edgeIndex)
    //{
    //    int i = Mathf.Clamp(edgeIndex, 0, _sharedBotEdgeDic[nb].Count - 1);

    //    return _sharedBotEdgeDic[nb][i];
    //}

    List<TetraEdge> Edges = new List<TetraEdge>(4);
   
    List<Transform> _vertexs = new List<Transform>();
    List<Transform> _axis = new List<Transform>();
    Queue<float> _dis = new Queue<float>();

    //faceCenter
    Vector3[] fc = new Vector3[4];

    //get tetrahedron edge
    public List<TetraEdge> GetEdges()
    {
        return Edges;
    }

    public List<TetraFace> GetFaces()
    {
        return _face;
    }

   

    //add vertex
    public void Initialize(Vector3 p)
    {
        _mesh = GetComponent<MeshFilter>().mesh;
        transform.localPosition = p;

        //add vertex prfab oreach (var v in _mesh.vertices)
        for(int i=0; i < _mesh.vertexCount; i++)
        {
            var v = _mesh.vertices[i];
            var vert = Instantiate(_Vertex, transform);
            vert.transform.localScale = 0.05f * Vector3.one;
            vert.transform.localPosition = v;
            vert.gameObject.name ="Vertex_" + i.ToString();
            _vertexs.Add(vert.transform);
        }
        //print(_vertexs.Count);

        //get mesh triangles index
        var fi = _mesh.triangles;
        fc[0] = GetCenter(fi[0], fi[1], fi[2]);
        fc[1] = GetCenter(fi[1], fi[2], fi[3]);
        fc[2] = GetCenter(fi[0], fi[2], fi[3]);
        fc[3] = GetCenter(fi[0], fi[1], fi[3]);
        var ver = _vertexs;
        var v0 = ver[fi[0]].position;
        var v1 = ver[fi[1]].position;
        var v2 = ver[fi[2]].position;
        var v3 = ver[fi[3]].position;
        var v4 = ver[fi[4]].position;
        var v5 = ver[fi[5]].position;

        //for fc0
        //facepoint
        var fp0 = Instantiate(_fpVertex, transform);
        var F0 = fp0.transform;
        fp0.transform.localPosition = fc[0];
        fp0.transform.localScale = 0.05f * Vector3.one;
        //edge01
        var e0 = Instantiate(_edge);
        e0.name = this.transform.name + "_Edge_0";
        var E0 = e0.transform;
        E0.localPosition = (v0+ v1) / 2;
        E0.localScale = new Vector3(0.05f, (v1 - v0).magnitude / 2, 0.05f);
        var d0 = v1 - v0;
        E0.localRotation = Quaternion.FromToRotation(E0.up, d0);
        E0.parent = this.transform;
        //edge12
        var e1 = Instantiate(_edge);
        e1.name = this.transform.name + "_Edge_1";
        var E1 = e1.transform;
        E1.localPosition = (v1 + v2) / 2;
        E1.localScale = new Vector3(0.05f, (v2 - v1).magnitude / 2, 0.05f);
        var d1 = v2 - v1;
        E1.localRotation = Quaternion.FromToRotation(E1.up, d1);
        E1.parent = this.transform;
        //edge02
        var e2 = Instantiate(_edge);
        e2.name = this.transform.name + "_Edge_2";
        var E2 = e2.transform;
        E2.localPosition = (v0 + v2) / 2;
        E2.localScale = new Vector3(0.05f, (v2 - v0).magnitude / 2, 0.05f);
        var d2 = v2 - v0;
        E2.localRotation = Quaternion.FromToRotation(E2.up, d2);
        E2.parent = this.transform;
        _face.Add(new TetraFace(e0, e1, e2, F0));
        //_sharedFace.Add()
      


        //for fc1
        //facepoint
        var fp1 = Instantiate(_fpVertex, transform);
        var F1 = fp1.transform;
        fp1.transform.localPosition = fc[1];
        fp1.transform.localScale = 0.05f * Vector3.one;
        //edge13
        var e3 = Instantiate(_edge);
        e3.name = this.transform.name + "_Edge_3";
        var E3 = e3.transform;
        E3.localPosition = (v1 + v3) / 2;
        E3.localScale = new Vector3(0.05f, (v3 - v1).magnitude / 2, 0.05f);
        var d3 = v3 - v1;
        E3.localRotation = Quaternion.FromToRotation(E3.up, d3);
        E3.parent = this.transform;
        //edge23
        var e4 = Instantiate(_edge);
        e4.name = this.transform.name + "_Edge_4";
        var E4 = e4.transform;
        E4.localPosition = (v3 + v2) / 2;
        E4.localScale = new Vector3(0.05f, (v3 - v2).magnitude / 2, 0.05f);
        var d4 = v3 - v2;
        E4.localRotation = Quaternion.FromToRotation(E4.up, d4);
        E4.parent = this.transform;
        _face.Add(new TetraFace(e1, e3, e4, F1));

        //for fc2
        //facepoint
        var fp2 = Instantiate(_fpVertex, transform);
        var F2 = fp2.transform;
        fp2.transform.localPosition = fc[2];
        fp2.transform.localScale = 0.05f * Vector3.one;
        //edge03
        var e5 = Instantiate(_edge);
        e5.name = this.transform.name + "_Edge_5";
        var E5 = e5.transform;
        E5.localPosition = (v0 + v3) / 2;
        E5.localScale = new Vector3(0.05f, (v3 - v0).magnitude / 2, 0.05f);
        var d5 = v3 - v0;
        E5.localRotation = Quaternion.FromToRotation(E5.up, d5);
        E5.parent = this.transform;
        _face.Add(new TetraFace(e2, e4, e5, F2));

        //for fc3
        //facepoint
        var fp3 = Instantiate(_fpVertex, transform);
        var F3 = fp3.transform;
        fp3.transform.localPosition = fc[3];
        fp3.transform.localScale = 0.05f * Vector3.one;
        _face.Add(new TetraFace(e0, e3, e5, F3));


       // _sharedEdgefaces.Add(e0,)
        //_face.Add(new TetraFace(ver[fi[0]], ver[fi[1]], ver[fi[2]]));
        //_face.Add(new TetraFace(ver[fi[1]], ver[fi[2]], ver[fi[3]]));
        //_face.Add(new TetraFace(ver[fi[0]], ver[fi[2]], ver[fi[3]]));
        //_face.Add(new TetraFace(ver[fi[0]], ver[fi[1]], ver[fi[3]]));


        ////add facevertex prfab
        //for (int i = 0; i< 4; i++)
        //{
        //    var face = Instantiate(_fpVertex, transform);
        //    face.transform.localPosition = fc[i];
        //    face.transform.localScale = 0.05f * Vector3.one;    
        //}
       SetEdge();
       // print(Edges.Count);
      
        ////add edge prefab
        //for (int i = 0; i < Edges.Count;i++)
        //{
        //    var e = Edges[i];
        //    var v0 = e._start.position;
        //    var v1 = e._end.position;
        //    var E = Instantiate(_edge);
        //    var v = E.transform;          
        //    // e.parent = start.parent;
        //    v.localPosition = (v0 + v1) / 2;
        //    v.localScale = new Vector3(0.05f, (v1 - v0).magnitude/2, 0.05f);
        //    var d = v1 - v0;
        //    v.localRotation = Quaternion.FromToRotation(v.up, d);
        //    v.parent = this.transform;
        //}
    }

   // set edge
    void SetEdge()
    {
        foreach (var v0 in _vertexs)
        {
            foreach (var v1 in _vertexs)
            {
                if (v0 != v1 && !EdgeRepeat(v0, v1))
                {
                    Edges.Add(new TetraEdge(v0, v1));
                }
            }
        }
    }

    //judge whether the edge is repeat
    bool EdgeRepeat(Transform v0, Transform v1)
    {
        bool repeat = false;

        foreach (var e in Edges)
        {
            if (e.GetPoints().Contains(v0) && e.GetPoints().Contains(v1))
            {
                repeat = true;
                break;
            }
        }

        return repeat;
    }

    //get vertex
    public List<Transform> GetVerts()
    {
        return _vertexs;
    }
 

    //get rotate edge
    public Vector3 GetAxis(int v0, int v1)
    {
        return _vertexs[v0].localPosition - _vertexs[v1].localPosition;
    }

    ////get rotate direction
    //public Vector3 GetDirection(int face, int vert)
    //{
    //    var fi = _mesh.GetTriangles(face);

    //    var c = GetCenter(fi[0], fi[1], fi[2]);

    //    return _mesh.vertices[vert] - c;
    //}

    //get center point of face
    public Vector3 GetCenter(int v0, int v1, int v2)
    {
        var ver = _vertexs;
        return (ver[v0].localPosition + ver[v1].localPosition + ver[v2].localPosition) / 3;
    }

    //get rotate edge length
    public float GetEdgeLength()
    {
        var v = GetComponent<MeshFilter>().mesh.vertices;

        return (v[0] - v[1]).magnitude;
    }

    //get rotation angle
    //public float GetAngle(Transform from, Transform to, TetraEdge edge)
    //{
    //    var v0 = edge.GetCenter();
    //    var F = from.position - v0;
    //    var T = to.position - v0;
    //    var D = (from.position - to.position).magnitude;
    //    var E = GetEdgeLength();
    //    var A = Vector3.Angle(F, T);
    //    print(A);
    //    if (D > E)
    //    {
    //        A = 360 - A;
    //    }
    //    return A;
    //}

    //check the edge and get axis edge if edge was shared with others 
    public void CheckSharedEdge(TetraBot nb)
    {
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    var e = transform.GetChild(i);
        //    if (e.tag == "ColliderBall")
        //    {

        //        //get other tetra edge which is shared with this one
        //        Debug.Log("get connect edge" + e);
        //        GetTetraEdge(e);
        //        _axis.Add(e);
        //    }
        //    else
        //    {

        //    }
        //}
        foreach (var e in Edges)
        {
            var tf = e.GetTransform();
            if (tf.tag == "ColliderBall")
            {
                var ee = e._Axis[0];
                GetTetraEdge(ee);
            }
        }
        print(_axis);
        Evaluatedis();
    }

    //Get tetra edge who occupied this edge
    public void GetTetraEdge(Transform e)
    {
        //Queue<TetraFace> _freeface = new Queue<TetraFace>();
        foreach (var f in _face)
        {           
            var ver = _vertexs;
            print("checkneighbor");
            var e1 = f.Edge1.transform;
            print(e1);
            var e2 = f.Edge2.transform;
            var e3 = f.Edge3.transform;
            List<Transform> Fe = new List<Transform>();
            // edgeCenter.Add(e1);
            Fe.Add(e1);
            Fe.Add(e2);
            Fe.Add(e3);
            if (Fe.Contains(e))
            {
                //_freeface.Enqueue(f);
                var fp = f.FacePoint;
                //check this edge face whether available
                if (fp.tag == "Free")
                {
                    _sharedEdge.Add(e, f);
                }
            }       
        }
       // _sharededge.Add(e, _freeface);
        print("addfreeface");
    }

    //Evaluate the distance with target
    public void Evaluatedis()
    {
        for (int i = 0; i < _axis.Count; i++)
        {
            var se = _sharedEdge[_axis[i]];
            var fp = se.FacePoint;
            var d = (Target.position - fp.position).magnitude;
            _pointDis.Add(d, _axis[i]);
            _dis.Enqueue(d);
            Comparedis();
        }
    }
    //Deciding which face to connect 
    public void Comparedis()
    {
        var m = _dis.Peek();
        var e = _pointDis[m];
        var fp = _sharedEdge[e];
        Rotate();
    }

    //rotating the axis edge(sharededge)
    //public void Rotate(TetraBot nb, int edge, Transform fromPoint, Transform toPoint)
    //{
    //    var e = nb._sharedBotEdgeDic[this][edge];

    //    var axis = e.GetDirection();

    //    var angle = GetAngle(fromPoint, toPoint, e);

    //    var center = e.GetCenter();

    //    var pivot = e.GetCenterXform();

    //    transform.parent = pivot;

    //    pivot.Rotate(axis, angle);

    //    transform.parent = pivot.parent.parent;
    //}

    // rotate
    void Rotate()
    {
        Debug.Log("rotate");
    }

   


}


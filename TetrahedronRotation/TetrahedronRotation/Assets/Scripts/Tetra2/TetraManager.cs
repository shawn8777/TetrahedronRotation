using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetraManager : MonoBehaviour
{
    private List<Tetra> Bots = new List<Tetra>();
    [SerializeField] private Tetra _tbpfb;
    [SerializeField] private int CountX = 5;
    [SerializeField] private int CountZ = 5;
    [SerializeField] private float Scale = 1f;
   // private int s = -1;
    List<Vector3> SavedPositions = new List<Vector3>();
    Dictionary<string, int> _bot;
    public void Awake()
    {
        _bot = new Dictionary<string, int>();
    }
    // Use this for initialization
    void Start()
    {
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
           // Chose();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckNeighbor();
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            CheckEdge();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Rotate();
        }
    }

    void Chose()
    {       
        print(Bots[0].name);
    }

    void CheckNeighbor()
    {
       
        //for (int i = 1; i < Bots.Count; i++)
        //{
        //    //Bots[0].CheckNeighborTetra(Bots[i]);
        //}
    }
    
    void CheckEdge()
    {
        Bots[0].Checkedge();
        SelectFace();
    }

    public void SelectFace()
    {
        var n = Bots[0].others;
        var i = _bot[n];
        var b = Bots[i];
        Bots[0].CheckNeighbor(b);
        Rotate();
    }

    void Rotate()
    {
        Bots[0].Rotate();
    }
   
    //add tetrahedron
    void SetPosition()
    {
        for (int z = 0; z < CountZ; z++)
        {
            for (int x = 0; x < CountX; x++)
            {
                var tb = Instantiate(_tbpfb, transform);
              
                Bots.Add(tb);

                tb.gameObject.name = "Tetra_" + Bots.IndexOf(tb);//x.ToString() + ":" + z.ToString();

                _bot.Add(tb.gameObject.name, Bots.IndexOf(tb));

                

                Scale = tb.GetEdgeLength();

                float a = 0.5f * Scale * (Mathf.Sqrt(3));
                Vector3 p;

                if (z % 2 == 0)
                {
                    if (x % 2 == 0)
                    {
                        p = (new Vector3(0.5f * Scale * x, 0, z * a));

                        SavedPositions.Add(p);
                        tb.transform.localEulerAngles = new Vector3(0, 180, 0);
                    }
                    else
                    {
                        p = (new Vector3(0.5f * Scale * x, 0, z * a + a / 3));

                        SavedPositions.Add(p);
                    }
                }
                else
                {
                    if (x % 2 != 0)
                    {
                        p = (new Vector3(0.5f * Scale * x, 0, z * a));

                        SavedPositions.Add(p);
                        tb.transform.localEulerAngles = new Vector3(0, 180, 0);
                    }
                    else
                    {
                        p = (new Vector3(0.5f * Scale * x, 0, z * a + a / 3));

                        SavedPositions.Add(p);

                    }
                }

                if (z % 2 == 0 || x % 2 == 0)
                {

                }
                //  var tb = Instantiate(_tbpfb, transform);
                tb.Initialize(p);

            }
        }


    }



}

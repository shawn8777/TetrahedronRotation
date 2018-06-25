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

    public Material Selected;
    public Material UnSelected;
    string[] N = new string[1];

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
  
    GameObject Slb;

    // Update is called once per frame
    void Update()
    {   
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CheckEdge();
        }
        
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            bool didHit = Physics.Raycast(ray, out hitInfo, 500.0f);
            if(didHit)
            {
                print(hitInfo.collider.name);
                if(hitInfo.collider.name.Contains("Tetra"))
                {
                    //Slb = hitInfo.transform.gameObject;
                    //Slb.GetComponent<MeshRenderer>().material = Selected;
                    N[0] = hitInfo.collider.name;
                }
            }
            else
            {
               
                print("nothing");
            }           
        }
    }

    
    void CheckEdge()
    {
        if(N[0]!=null)
        {
            var i = _bot[N[0]];
            Bots[i].Checkedge();
            SelectFace(i);
        }
        else
        {
            print("Please select the tetrahedron");
        }        
       
    }

    public void SelectFace(int i)
    {
        var n = Bots[i].others;
        var e = _bot[n];
        var b = Bots[e];
        Bots[i].CheckNeighbor(b);
        Rotate(i);
    }

    void Rotate(int i)
    {
        Bots[i].Rotate();
        //N[0] = null;
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

                tb.gameObject.name = "TetraR_" + Bots.IndexOf(tb);//x.ToString() + ":" + z.ToString();

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

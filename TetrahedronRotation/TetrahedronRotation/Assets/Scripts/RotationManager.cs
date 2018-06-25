using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationManager : MonoBehaviour
{
    private List<TetraBot> Bots=new List<TetraBot>(); 
    [SerializeField] private TetraBot _tbpfb;
    [SerializeField] private int CountX = 5;
    [SerializeField] private int CountZ = 5;
    [SerializeField] private float Scale = 1f;

    List<Vector3> SavedPositions=new List<Vector3>();

    // Use this for initialization
    void Start()
    {
        SetPosition();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Evaluate();
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
    



    void Evaluate()
    {
        //for (int i = 0; i < CountX; i++)
        //{
        //    Bots[i].EvaluateNeighbor(Bots[i+1]);
        //Bots[0].EvaluateNeighbor(Bots[1]);
      
        //}
    }

    void CheckEdge()
    {
        Bots[0].CheckSharedEdge(Bots[0]);
    }

    void Rotate()
    {
        //for (int i = 0; i < CountX; i++)
        //{
        //    Bots[i].Rotate(Bots[i+1], i, Bots[i]._movablePoints[Bots[i+1]][i], Bots[i+1]._movablePoints[Bots[i]][i]);
      
       // Bots[0].Rotate(Bots[1], 0, Bots[0]._movablePoints[Bots[1]][0], Bots[1]._movablePoints[Bots[0]][0]);
        //}
    }


    //add tetrahedron
    void SetPosition()
   {
        for (int z = 0; z < CountZ; z++)
        {
            for (int x = 0; x < CountX; x++)
            {
                var tb = Instantiate(_tbpfb, transform);
                tb.gameObject.name = "Tetrabot_" + x.ToString() + ":" + z.ToString();

                Bots.Add(tb);

                Scale = tb.GetEdgeLength();

                float a = 0.5f*Scale * (Mathf.Sqrt(3));
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
              
                if (z % 2 == 0||x%2==0)
                {
                   
                }
                //  var tb = Instantiate(_tbpfb, transform);
                tb.Initialize(p);
               
            }
        }

      
    }

   

}

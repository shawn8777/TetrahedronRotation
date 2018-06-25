using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class angle : MonoBehaviour
{
    public Transform v0;
    public Transform v1;
    public Transform v2;
	// Use this for initialization
	void Start ()
    {
        Vector3 v01 = v0.localPosition - v1.localPosition;
        Vector3 v12 = v2.localPosition - v1.localPosition;
        var A = Vector3.Angle(v01, v12);
        print(A);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineMono : MonoBehaviour
{
    public GeodesicLine line = new GeodesicLine(0, 0);
    public PointMono start;
    public PointMono end;

    // Start is called before the first frame update
    void Start()
    {
        line.start = start.modelPosition;
        line.end = end.modelPosition;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

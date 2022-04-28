using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuatTest : MonoBehaviour
{
    private Quaternion p = new Quaternion(0, 0, 0, 0);
    private Quaternion q = new Quaternion(0, 0, 0, 0);

    private Matrix matA = new Matrix(new Complex(0, 1),
                                     new Complex(0, 0),
                                     new Complex(1, 0),
                                     new Complex(0, -1));

    public ModelPoint pointA = new ModelPoint( new Quaternion(0, 3, 1, 0) );
    public ModelPoint pointB = new ModelPoint(new Quaternion(0, 3, 2, 0));

    public GeodesicLine line = new GeodesicLine(0, 0);

    private void Start()
    {
        line.start = pointA;
        line.end = pointB;
        line.Normalise();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            RandomQuats();
        }
    }

    private void RandomQuats()
    {
        p.x = Random.Range(-10, 10);
        p.y = Random.Range(-10, 10);
        p.z = 10;
        p.w = 0;
    }
}

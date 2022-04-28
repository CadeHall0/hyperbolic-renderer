using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Triangle
{
    public int v1_index;
    public int v2_index;
    public int v3_index;

    public Triangle(int a, int b, int c)
    {
        v1_index = a;
        v2_index = b;
        v3_index = c;
    }
}

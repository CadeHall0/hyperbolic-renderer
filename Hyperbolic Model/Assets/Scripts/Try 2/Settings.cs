using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public bool isEuclidean;
    [Range(1, 5)] public int subdivisions;
    public float lineThickness;
    public float pointSize;
    public Material darkMat;
    public Material redMat;
    public Material transparent;
    public Material gridMat;
    public Material veryDark;
    public int gridSize;
}

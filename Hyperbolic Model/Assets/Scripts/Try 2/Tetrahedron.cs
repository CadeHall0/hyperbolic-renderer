using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetrahedron : MonoBehaviour
{
    public Transform p1;
    public Transform p2;
    public Transform p3;
    public Transform p4;

    private CurvedPlane pl_123;
    private CurvedPlane pl_124;
    private CurvedPlane pl_134;
    private CurvedPlane pl_234;

    private GameObject obj1;
    private GameObject obj2;
    private GameObject obj3;
    private GameObject obj4;
    // Start is called before the first frame update
    void Awake()
    {
        transform.position = Vector3.zero;

        obj1 = new GameObject();
        obj1.AddComponent<MeshRenderer>();
        obj1.AddComponent<MeshFilter>();
        pl_123 = obj1.AddComponent<CurvedPlane>();
        
        obj2 = new GameObject();
        obj2.AddComponent<MeshRenderer>();
        obj2.AddComponent<MeshFilter>();
        pl_124 = obj2.AddComponent<CurvedPlane>();

        obj3 = new GameObject();
        obj3.AddComponent<MeshRenderer>();
        obj3.AddComponent<MeshFilter>();
        pl_134 = obj3.AddComponent<CurvedPlane>();

        obj4 = new GameObject();
        obj4.AddComponent<MeshRenderer>();
        obj4.AddComponent<MeshFilter>();
        pl_234 = obj4.AddComponent<CurvedPlane>();

        pl_123.p1 = p1;
        pl_123.p2 = p2;
        pl_123.p3 = p3;

        pl_124.p1 = p1;
        pl_124.p2 = p2;
        pl_124.p3 = p4;

        pl_134.p1 = p1;
        pl_134.p2 = p3;
        pl_134.p3 = p4;

        pl_234.p1 = p2;
        pl_234.p2 = p3;
        pl_234.p3 = p4;
    }

    private void Start()
    {
        var center = (p1.position + p2.position + p3.position + p4.position) / 4;
        pl_123.tetCenter = center;
        pl_124.tetCenter = center;
        pl_134.tetCenter = center;
        pl_234.tetCenter = center;
    }
}

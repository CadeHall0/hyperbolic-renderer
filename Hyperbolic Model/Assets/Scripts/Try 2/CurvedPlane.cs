using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum RenderState
{
    PASUED,
    LIVE
}

public class CurvedPlane : MonoBehaviour
{
    private Material mat;
    private Vector3 planeCenter;
    public Vector3 tetCenter;

    public bool isDark;

    public Transform p1;
    public Transform p2;
    public Transform p3;

    private CurvedGeodesic l12;
    private CurvedGeodesic l13;
    private CurvedGeodesic l23;

    private GameObject objl12;
    private GameObject objl13;
    private GameObject objl23;

    private Settings settings;
    private Mesh mesh;
    private List<Vector3> tempVerts;
    private List<int> tempTris;
    private List<Triangle> temptriStorage = new List<Triangle>();
    private List<Triangle> triStorage = new List<Triangle>();
    private RenderState renderState = RenderState.LIVE;

    private Vector3[] vertices;
    private int[] triangles;
    private Vector3 sphereCenter = Vector3.zero;
    private float sphereRadius = 0;

    private Triangle t1;
    private Triangle t2;
    private Triangle t3;
    private Triangle t4;

    private GameObject p2Upp;
    private GameObject p3Upp;
    private Transform camPos;

    private float orientationVal;
    private Vector3 outNormal;

    private float neededHeight;
    private bool isFinite = true;
    private int subdivs;
    // Create abstract lines, and assign start/end points
    private void Awake()
    {
        camPos = FindObjectOfType<Camera>().transform;
        transform.position = Vector3.zero;

        settings = FindObjectOfType<Settings>();
        mat = settings.transparent;

        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshRenderer>().material = mat;

        objl12 = new GameObject();
        objl13 = new GameObject();
        objl23 = new GameObject();

        objl12.AddComponent<LineRenderer>();
        objl13.AddComponent<LineRenderer>();
        objl23.AddComponent<LineRenderer>();
        l12 = objl12.AddComponent<CurvedGeodesic>();
        l13 = objl13.AddComponent<CurvedGeodesic>();
        l23 = objl23.AddComponent<CurvedGeodesic>();
    }

    private void Start()
    {
        if (p1.tag == "INFINITY")
        {
            isFinite = false;

            neededHeight = (p2.position - p3.position).magnitude * 5;
            p2Upp = new GameObject();
            p3Upp = new GameObject();
            p2Upp.AddComponent<MeshRenderer>();
            p3Upp.AddComponent<MeshRenderer>();


            p2Upp.transform.position = p2.position + Vector3.up * neededHeight;
            p3Upp.transform.position = p3.position + Vector3.up * neededHeight;
            p2Upp.transform.parent = p2;
            p3Upp.transform.parent = p3;

            l12.start = p2Upp.transform;
            l12.end = p2;

            l13.start = p3Upp.transform;
            l13.end = p3;

            l23.start = p2;
            l23.end = p3;

            subdivs = settings.subdivisions;

            tempVerts = new List<Vector3>();
            tempTris = new List<int>();
            InfinitePlane();
        }
        else
        {
            l12.start = p1;
            l12.end = p2;

            l13.start = p1;
            l13.end = p3;

            l23.start = p2;
            l23.end = p3;

            subdivs = settings.subdivisions;

            InitialPlane();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDark)
        {
            GetComponent<MeshRenderer>().material = settings.veryDark;
        }
        else
        {
            GetComponent<MeshRenderer>().material = settings.transparent;
        }

        if (settings.isEuclidean)
        {
            orientationVal = Vector3.Dot(Vector3.Cross(p1.position - p2.position, p1.position - p3.position), camPos.position - planeCenter);

            vertices = new Vector3[]
            {
                p1.position, p2.position, p3.position
            };

            if(orientationVal >= 0)
            {
                triangles = new int[]
                {
                    0, 1, 2, 0, 2, 1
                };
            }
            else
            {
                triangles = new int[]
                {
                    0, 2, 1, 0, 1, 2
                };
            }

            ResetMesh();
        }
        else
        {
            if (renderState == RenderState.LIVE)
            {
                if (isFinite)
                {
                    orientationVal = Vector3.Dot(Vector3.Cross(p1.position - p2.position, p1.position - p3.position), camPos.position - planeCenter);
                    orientationVal *= Mathf.Pow(-1, settings.subdivisions % 2);

                    planeCenter = (p1.position + p2.position + p3.position) / 3;
                    subdivs = settings.subdivisions;

                    l12.offset = (((l12.start.position + l12.end.position) / 2) - tetCenter).normalized * 0.05f;
                    l13.offset = (((l13.start.position + l13.end.position) / 2) - tetCenter).normalized * 0.05f;
                    l23.offset = (((l23.start.position + l23.end.position) / 2) - tetCenter).normalized * 0.05f;

                    MakeMesh();
                    Normalise();
                }
                else
                {
                    var mid = (p2.position + p3.position) / 2;
                    var tempp1 = mid - mid.y * Vector3.up;

                    orientationVal = Vector3.Dot(Vector3.Cross(tempp1 - p2.position, tempp1 - p3.position), camPos.position - tempp1);
                    tetCenter = 2 * mid - camPos.position;

                    InfinitePlane();
                }
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                if (renderState == RenderState.LIVE)
                {
                    if (isFinite)
                    {
                        renderState = RenderState.PASUED;
                        subdivs = 7;

                        MakeMesh();
                        Normalise();
                    }
                    else
                    {
                        InfinitePlane();
                    }
                }
                else
                {
                    renderState = RenderState.LIVE;
                }
            }
        }
    }

    private void InfinitePlane()
    {
        neededHeight = (p2.position - p3.position).magnitude * 5;
        p2Upp.transform.position = Vector3.up * (neededHeight + p3.position.y) + p2.position;
        p3Upp.transform.position = Vector3.up * (neededHeight + p2.position.y) + p3.position;

        tempVerts.Clear();
        tempTris.Clear();

        tempVerts = new List<Vector3>
        {
            p2Upp.transform.position, ( p2Upp.transform.position + p3Upp.transform.position)/2, p3Upp.transform.position,
            p2.position, p3.position,
        };

        tempVerts.AddRange(l23.vertices);

        if (orientationVal >= 0)
        {
            tempTris = new List<int>
            {
                0, 1, 3,
                1, 2, 4,
                1, 5, 3,
                1, 4, tempVerts.Count - 1,
            };

            for (int i = 5; i <= tempVerts.Count - 2; i++)
            {
                tempTris.Add(1);
                tempTris.Add(i + 1);
                tempTris.Add(i);
            }
        }
        else
        {
            tempTris = new List<int>
            {
                0, 3, 1,
                1, 4, 2,
                1, 3, 5,
                1, tempVerts.Count - 1, 4
            };

            for (int i = 5; i <= tempVerts.Count - 2; i++)
            {
                tempTris.Add(1);
                tempTris.Add(i);
                tempTris.Add(i + 1);
            }
        }

        ConvertToArrays();
        ResetMesh();
    }

    private void InitialPlane()
    {
        tempVerts = new List<Vector3>
        {
            p1.position,
            p2.position,
            p3.position
        };

        tempTris = new List<int>
        {
            0, 1, 2
        };

        t1 = new Triangle(0, 1, 2);
        triStorage.Add(t1);
        ConvertToArrays();
    }
    
    private bool AreCoplanar()
    {
        return (Vector3.Cross(p1.position - p3.position, p2.position - p3.position).y == 0);
    }

    private void MakeMesh()
    {
        if (AreCoplanar())
        {
            p1.position += Vector3.left * 0.03f;
            p2.position += Vector3.one * 0.03f;
        }
        else
        {
            tempVerts = new List<Vector3>(vertices);
            triStorage.Clear();

            InitialPlane();

            for (int i = 1; i <= subdivs; i++)
            {
                tempTris.Clear();
                temptriStorage = new List<Triangle>();

                foreach (Triangle tri in triStorage)
                {
                    var v1 = tempVerts[tri.v1_index];
                    var v2 = tempVerts[tri.v2_index];
                    var v3 = tempVerts[tri.v3_index];

                    var u1 = (v1 + v2) / 2;
                    var u2 = (v2 + v3) / 2;
                    var u3 = (v3 + v1) / 2;

                    var u1_index = 0;
                    var u2_index = 0;
                    var u3_index = 0;

                    var baseIndex = tempVerts.Count - 1;

                    if (!tempVerts.Contains(u1))
                    {
                        tempVerts.Add(u1);
                        baseIndex++;
                        u1_index = baseIndex;
                    }
                    else
                    {
                        u1_index = tempVerts.IndexOf(u1);
                    }

                    if (!tempVerts.Contains(u2))
                    {
                        tempVerts.Add(u2);
                        baseIndex++;
                        u2_index = baseIndex;
                    }
                    else
                    {
                        u2_index = tempVerts.IndexOf(u2);
                    }

                    if (!tempVerts.Contains(u3))
                    {
                        tempVerts.Add(u3);
                        baseIndex++;
                        u3_index = baseIndex;
                    }
                    else
                    {
                        u3_index = tempVerts.IndexOf(u3);
                    }

                    t1 = new Triangle(tri.v1_index, u3_index, u1_index);
                    t2 = new Triangle(tri.v2_index, u1_index, u2_index);
                    t3 = new Triangle(tri.v3_index, u2_index, u3_index);
                    t4 = new Triangle(u1_index, u3_index, u2_index);

                    if (true)
                    {
                        var lst = new List<int>
                        {
                            tri.v1_index, u3_index, u1_index,
                            tri.v2_index, u1_index, u2_index,
                            tri.v3_index, u2_index, u3_index,
                            u1_index, u3_index, u2_index
                        };

                        tempTris.AddRange(lst);

                        lst = new List<int>
                        {
                            tri.v1_index, u1_index, u3_index,
                            tri.v2_index, u2_index, u1_index,
                            tri.v3_index, u3_index, u2_index,
                            u1_index, u2_index, u3_index
                        };

                        tempTris.AddRange(lst);
                    }
                    else
                    {
                        var lst = new List<int>
                        {
                            tri.v1_index, u1_index, u3_index,
                            tri.v2_index, u2_index, u1_index,
                            tri.v3_index, u3_index, u2_index,
                            u1_index, u2_index, u3_index
                        };

                        tempTris.AddRange(lst);
                    }

                    temptriStorage.Add(t1);
                    temptriStorage.Add(t2);
                    temptriStorage.Add(t3);
                    temptriStorage.Add(t4);
                }

                triStorage = new List<Triangle>(temptriStorage);
            }

            ConvertToArrays();
        }
    }

    private void ConvertToArrays()
    {
        vertices = tempVerts.ToArray();
        triangles = tempTris.ToArray();
    }

    private List<Vector3> temptempVerts;
    private void ProjectVertices()
    {
        tempVerts.Clear();
        temptempVerts = new List<Vector3>(vertices);

        foreach(Vector3 v in vertices)
        {
            var tv = v - sphereCenter;
            tv.y = Mathf.Pow(sphereRadius * sphereRadius - tv.x * tv.x - tv.z * tv.z, 0.5f);
            tv += sphereCenter;
            tempVerts.Add(tv);
        }

        vertices = tempVerts.ToArray();
    }

    private void Normalise()
    {
        var a = l12.idealStart;
        var b = l13.idealStart;
        var c = l23.idealStart;

        var t = b - a;
        var u = c - a;
        var v = c - b;

        // triangle normal
        var w = Vector3.Cross(t, u);
        var wsl = w.sqrMagnitude;
        
        // helpers
        var iwsl2 = 1.0f / (2.0f * wsl);
        var tt = Vector3.Dot(t, t);
        var uu = Vector3.Dot(u, u);

        // result circle
        sphereCenter = a + (u * tt * (Vector3.Dot(u, v)) - t * uu * Vector3.Dot(t, v)) * iwsl2;
        sphereRadius = Mathf.Pow(tt * uu * Vector3.Dot(v, v) * iwsl2 * 0.5f, 0.5f);
        
        if(sphereRadius < 450)
        {
            ProjectVertices();
        }

        if (!AreCoplanar())
        {
            ResetMesh();
        }
    }

    private void ResetMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        //mesh.RecalculateNormals();
        //mesh.RecalculateTangents();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurvedGeodesic : MonoBehaviour
{
    public Vector3 offset;
    public Transform start;
    public Transform end;
    public float length;
    public List<Vector3> vertices = new List<Vector3>();

    private RenderState renderState = RenderState.LIVE;
    private Settings settings;
    private LineRenderer lr;
    private Vector3 idealCenter = Vector3.zero;
    private float radius;
    private void Awake()
    {
        transform.position = Vector3.zero;
        var v = Vector3.one;
        vertices = new List<Vector3>();

        for (int i = 0; i < 100; i++)
        {
            vertices.Add(v);
        }

        settings = FindObjectOfType<Settings>();
        lr = GetComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = settings.lineThickness;
        lr.startColor = lr.endColor = new Color(1, 1, 1);
        lr.material = settings.darkMat;
    }

    private void Start()
    {
        subdivs = settings.subdivisions;
        start.GetComponent<MeshRenderer>().material = settings.redMat;
        end.GetComponent<MeshRenderer>().material = settings.redMat;
    }

    private void SetUpLine()
    {
        vertices.Clear();
        length = (end.position - start.position).magnitude;

        if(end.position.x == start.position.x && end.position.z == start.position.z || settings.isEuclidean)
        {
            lr.positionCount = 2;
            lr.SetPosition(0, start.position);
            lr.SetPosition(1, end.position);

            vertices.Add(start.position);
            vertices.Add(end.position);
        }
        else
        {
            var dir = (end.position - start.position).normalized;
            var numPoints = (int)Mathf.Pow(2, subdivs);
            var trueSpacing = (end.position - start.position).magnitude / (numPoints);
            lr.positionCount = numPoints + 1;

            for (int i = 0; i <= numPoints; i++)
            {
                var eucPos = start.position + dir * (i * trueSpacing);
                eucPos -= idealCenter;
                eucPos.y = Mathf.Pow(radius*radius - eucPos.x*eucPos.x - eucPos.z*eucPos.z, 0.5f);
                var hypPos = eucPos + idealCenter;

                if(i != 0 && i != numPoints)
                {
                    vertices.Add(hypPos);
                }
                lr.SetPosition(i, hypPos);
            }
        }
    }

    private int subdivs;
    // Update is called once per frame
    void Update()
    {
        lr.startWidth = lr.endWidth = settings.lineThickness;
        start.transform.localScale = end.transform.localScale = settings.pointSize * Vector3.one;

        if(renderState == RenderState.LIVE)
        {
            subdivs = settings.subdivisions;
            Normalise();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (renderState == RenderState.LIVE)
            {
                renderState = RenderState.PASUED;
                subdivs = 7;

                Normalise();
            }
            else
            {
                renderState = RenderState.LIVE;
            }
        }
    }

    public Vector3 idealStart;
    public Vector3 idealEnd;
    public void Normalise()
    {
        if (end.position.x != start.position.x || end.position.z != start.position.z )
        {
            var dir = (end.position - start.position) / 2;
            var planeBase = start.position + 4 * dir;
            planeBase.y = 0;

            var normal = Vector3.Cross(start.position - planeBase, end.position - planeBase);
            var perpendicular = Vector3.Cross(normal, dir);

            var t = -(start.position.y + dir.y) / (perpendicular.y);
            idealCenter.x = start.position.x + dir.x + t * perpendicular.x;
            idealCenter.z = start.position.z + dir.z + t * perpendicular.z;

            radius = (start.position - idealCenter).magnitude;

            dir = (dir - (Vector3.up)*dir.y).normalized;
            idealStart = idealCenter - radius * (dir);
            idealEnd = idealCenter + radius * (dir);
        }

        SetUpLine();
    }
}

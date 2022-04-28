using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoGrid : MonoBehaviour
{
    private GameObject tempObj;
    private LineRenderer lr;
    private Settings settings;
    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<Settings>();

        tempObj = new GameObject();
        lr = tempObj.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = settings.lineThickness * 0.45f;
        lr.material = settings.darkMat;

        tempObj.transform.position = new Vector3(30 * settings.gridSize, 0, 0);

        lr.positionCount = 2;
        lr.SetPosition(0, tempObj.transform.position);
        lr.SetPosition(1, tempObj.transform.position - 60 * Vector3.right * settings.gridSize);

        tempObj = new GameObject();
        lr = tempObj.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = settings.lineThickness * 0.45f;
        lr.material = settings.darkMat;

        tempObj.transform.position = new Vector3(0, 30 * settings.gridSize, 0);

        lr.positionCount = 2;
        lr.SetPosition(0, tempObj.transform.position);
        lr.SetPosition(1, tempObj.transform.position - 60 * Vector3.up * settings.gridSize);
    }
}

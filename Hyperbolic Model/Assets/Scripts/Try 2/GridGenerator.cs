using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridGenerator : MonoBehaviour
{
    private Settings settings;
    private GameObject tempObj;
    private LineRenderer lr;

    // Start is called before the first frame update
    void Start()
    {
        settings = FindObjectOfType<Settings>();

        for (int i = 0; i <= 10 * settings.gridSize; i++)
        {
            tempObj = new GameObject();
            lr = tempObj.AddComponent<LineRenderer>();
            lr.startWidth = lr.endWidth = settings.lineThickness/3;
            lr.material = settings.gridMat;

            tempObj.transform.position = new Vector3(5 * settings.gridSize, 0.21f, -5 * settings.gridSize + i);

            lr.positionCount = 2;
            lr.SetPosition(0, tempObj.transform.position);
            lr.SetPosition(1, tempObj.transform.position - 10 * Vector3.right * settings.gridSize);
        }

        for (int i = 0; i <= 10 * settings.gridSize; i++)
        {
            tempObj = new GameObject();
            lr = tempObj.AddComponent<LineRenderer>();
            lr.startWidth = lr.endWidth = settings.lineThickness/3;
            lr.material = settings.gridMat;

            tempObj.transform.position = new Vector3(-5 * settings.gridSize + i, 0.21f, 5 * settings.gridSize);

            lr.positionCount = 2;
            lr.SetPosition(0, tempObj.transform.position);
            lr.SetPosition(1, tempObj.transform.position - 10 * Vector3.forward * settings.gridSize);
        }

        tempObj = new GameObject();
        lr = tempObj.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = settings.lineThickness *0.45f;
        lr.material = settings.darkMat;

        tempObj.transform.position = new Vector3(30 * settings.gridSize, 0.225f, 0);

        lr.positionCount = 2;
        lr.SetPosition(0, tempObj.transform.position);
        lr.SetPosition(1, tempObj.transform.position - 60 * Vector3.right * settings.gridSize);

        tempObj = new GameObject();
        lr = tempObj.AddComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = settings.lineThickness *0.45f;
        lr.material = settings.darkMat;

        tempObj.transform.position = new Vector3(0, 0.225f, 30 * settings.gridSize);

        lr.positionCount = 2;
        lr.SetPosition(0, tempObj.transform.position);
        lr.SetPosition(1, tempObj.transform.position - 60 * Vector3.forward * settings.gridSize);
    }
}

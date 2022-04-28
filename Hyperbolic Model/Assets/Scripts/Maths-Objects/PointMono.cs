using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointMono : MonoBehaviour
{
    public ModelPoint modelPosition = new ModelPoint(0);
    public Vector3 initialPoint;

    // Start is called before the first frame update
    void Start()
    {
        modelPosition.x1.x = initialPoint.x;
        modelPosition.x1.y = initialPoint.z;
        modelPosition.x1.z = initialPoint.y;

        transform.position = (modelPosition.AsVector());
    }

    // Update is called once per frame
    void Update()
    {
        // Change later lol
        modelPosition.x1.x = transform.position.x;
        modelPosition.x1.y = transform.position.z;
        modelPosition.x1.z = transform.position.y;
    }
}

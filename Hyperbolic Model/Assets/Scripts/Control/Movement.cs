using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float speed;
    public float lookSpeed;

    private float yaw;
    private float pitch;
    private Vector3 moveDir;
    private bool camLock = false;

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            camLock = !camLock;
        }


        moveDir.x = Input.GetAxisRaw("Horizontal");
        moveDir.y = Input.GetAxisRaw("Orthogonal");
        moveDir.z = Input.GetAxisRaw("Vertical");

        transform.Translate(moveDir * speed * Time.fixedDeltaTime);

        yaw += lookSpeed * Input.GetAxis("Mouse X");
        pitch -= lookSpeed * Input.GetAxis("Mouse Y");

        if (!camLock)
        {
            transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ScreenCapture.CaptureScreenshot("C:/Users/cadeh/Desktop/scrshot.png", 2);
        }
    }
}

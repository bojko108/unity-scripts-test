using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float MouseSensitivity = 100.0f;
    public float MoveSensitivity = 100.0f;
    public float ClampAngle = 80.0f;

    private float rotY = 0.0f; // rotation around the up/y axis
    private float rotX = 0.0f; // rotation around the right/x axis

    void Start()
    {
        Vector3 rot = transform.localRotation.eulerAngles;
        rotY = rot.y;
        rotX = rot.x;
    }

    void Update()
    {
        #region ROTATION

        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = -Input.GetAxis("Mouse Y");

        rotY += mouseX * this.MouseSensitivity * Time.deltaTime;
        rotX += mouseY * this.MouseSensitivity * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -this.ClampAngle, this.ClampAngle);

        Quaternion localRotation = Quaternion.Euler(rotX, rotY, 0.0f);
        transform.rotation = localRotation;

        #endregion

        #region MOVEMENT

        if (Input.GetAxis("Vertical") != 0)
        {
            transform.Translate(transform.forward * this.MoveSensitivity * Time.deltaTime * Input.GetAxis("Vertical"), Space.World);
        }
        if (Input.GetAxis("Horizontal") != 0)
        {
            transform.Translate(transform.right * this.MoveSensitivity * Time.deltaTime * Input.GetAxis("Horizontal"), Space.World);
        }

        #endregion
    }
}
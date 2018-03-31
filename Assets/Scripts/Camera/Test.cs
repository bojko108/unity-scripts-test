using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    private GameObject cameraContainer;

    private bool gyroReady = false;
    private bool mouseReady = false;

    // when mouse is used
    private float speedH = 1.0f;
    private float speedV = 1.0f;
    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // Use this for initialization
    public void Start()
    {
        // check if gyrospoce is available
        if (SystemInfo.supportsGyroscope)
        {

            Input.gyro.enabled = true;
            this.gyroReady = true;
        }
        // if not use mouse input
        else
        {
            Debug.Log("Gyroscope is not available...");

            if (Input.mousePresent)
            {
                this.mouseReady = true;
            }
            else
            {
                Debug.Log("Mouse is not available...");
                return;
            }
        }

        this.cameraContainer = new GameObject("Camera Container");
        this.cameraContainer.transform.position = this.transform.position;
        this.transform.parent = this.cameraContainer.transform;
    }

    // Update is called once per frame
    public void Update()
    {
        // close the game
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (this.gyroReady)
        {
            // update gyroscope
            this.cameraContainer.transform.Rotate(0, -Input.gyro.rotationRateUnbiased.y, 0);
            this.transform.Rotate(-Input.gyro.rotationRateUnbiased.x, 0, 0);


            // this needs to be tested!

            //Quaternion cameraRotation = new Quaternion(Input.gyro.attitude.x, Input.gyro.attitude.y, -Input.gyro.attitude.z, -Input.gyro.attitude.w);
            //this.transform.localRotation = cameraRotation;
        }

        if (this.mouseReady)
        {
            Cursor.lockState = CursorLockMode.Locked;

            this.yaw += this.speedH * Input.GetAxis("Mouse X");
            this.pitch += this.speedV * Input.GetAxis("Mouse Y");

            transform.eulerAngles = new Vector3(-this.pitch, this.yaw, 0.0f);
        }
    }
}

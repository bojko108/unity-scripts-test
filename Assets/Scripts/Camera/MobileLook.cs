using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Import;

public class MobileLook : MonoBehaviour
{
    private bool gyroscopeEnabled;
    private Gyroscope gyroscope;

    private GPS gps;

    private GameObject cameraContainer;

    private void Start()
    {
        this.cameraContainer = new GameObject("Camera Container");
        this.cameraContainer.transform.position = this.transform.position;
        this.transform.SetParent(this.cameraContainer.transform);

        // add GPS script
        this.gps = this.cameraContainer.AddComponent<GPS>();

        // set initial rotation
        this.cameraContainer.transform.rotation = Quaternion.Euler(90f, 0f, 0f);

        this.gyroscopeEnabled = this.EnableGyroscope();
    }

    private void Update()
    {
        #region CLOSE GAME

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        #endregion

        #region UPDATE CAMERA ROTATION

        if (this.gyroscopeEnabled)
        {
            Quaternion rotation = new Quaternion(this.gyroscope.attitude.x, this.gyroscope.attitude.y, -this.gyroscope.attitude.z, -this.gyroscope.attitude.w);
            this.transform.localRotation = rotation;
        }

        #endregion

        #region UPDATE CAMERA POSITION

        Vertex currentLocation = this.gps.Location;

        this.cameraContainer.transform.position = currentLocation.ReduceToVector3(this.cameraContainer.transform.position);

        #endregion
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Latitude: " + GPS.Instance.Latitude.ToString());
        GUILayout.Label("Longitude: " + GPS.Instance.Longitude.ToString());
        GUILayout.Label("Altitude: " + GPS.Instance.Altitude.ToString());
        GUILayout.Label("Northing: " + GPS.Instance.Location.Northing.ToString());
        GUILayout.Label("Easting: " + GPS.Instance.Location.Easting.ToString());
        GUILayout.Label("---------------------------------");
        GUILayout.Label("Camera position: " + this.cameraContainer.transform.position);
        
        if (this.gyroscopeEnabled)
        {
            GUILayout.Label("---------------------------------");
            GUILayout.Label("Input.gyro.attitude: " + this.gyroscope.attitude);
            GUILayout.Label("Input.gyro.rotationRate: " + this.gyroscope.rotationRate);
            GUILayout.Label("Input.gyro.rotationRateUnbiased: " + this.gyroscope.rotationRateUnbiased);
        }
    }

    private bool EnableGyroscope()
    {
        if (SystemInfo.supportsGyroscope)
        {
            this.gyroscope = Input.gyro;
            this.gyroscope.enabled = true;

            return true;
        }

        return false;
    }
}

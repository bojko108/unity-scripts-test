using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Import;

public class MobileLook : MonoBehaviour
{
    public Vector3 Origin;
    public float Scale;

    private GPS gps;
    private bool gyroscopeEnabled;
    private Gyroscope gyroscope;

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
        currentLocation.Scale(this.Scale);
        Vector3 currentLocationVector = currentLocation.ToVector3();

        // Y (height) will be equal to GPS height
        //TODO: add VeticalScale property for heights, simmilar for current property Scale
        this.cameraContainer.transform.position = new Vector3(currentLocationVector.x - this.Origin.x, 1.8f/*currentLocationVector.y*/, currentLocationVector.z - this.Origin.z);

        #endregion
    }

    private void OnGUI()
    {
        GUI.skin.label.fontSize = Screen.width / 40;

        GUILayout.Label("Origin position: " + this.Origin);
        GUILayout.Label("GPS position: " + this.gps.Location.ToVector3());
        GUILayout.Label("Camera position: " + this.cameraContainer.transform.position);

        if (this.gyroscopeEnabled)
        {
            GUILayout.Label("---------------------------------");
            GUILayout.Label("Input.gyro.enabled: " + this.gyroscope.enabled);
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
            this.gyroscope.enabled = false;
            this.gyroscope.enabled = true;

            return true;
        }

        return false;
    }
}

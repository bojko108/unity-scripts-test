using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Import;

public class GPS : MonoBehaviour
{
    public static GPS Instance { get; set; }
    
    [Tooltip("Set accuracy for GPS coordinates update in meters")]
    public float DesiredAccuracy = 10f;
    [Tooltip("Set the update distance for GPS results in meters")]
    public float UpdateDistance = 1f;

    public Vertex Location;

    public float Latitude;
    public float Longitude;
    public float Altitude;

    private void Start()
    {
        Instance = this;
        // do not destroy this object when changing the scene...
        DontDestroyOnLoad(gameObject);

        StartCoroutine(this.StartLocationService());
    }

    private void Update()
    {
        this.updateLocation(Input.location.lastData);
    }

    private void updateLocation(LocationInfo locationData)
    {
        this.Latitude = locationData.latitude;
        this.Longitude = locationData.longitude;
        this.Altitude = locationData.altitude;

        this.Location = new Vertex(this.Latitude, this.Longitude, this.Altitude);
        this.Location.ProjectToWebMercator();
    }

    private IEnumerator StartLocationService()
    {
        #region CHECK GPS STATUS

        if (!Input.location.isEnabledByUser)
        {
            yield break;
        }

        Input.location.Start(this.DesiredAccuracy, this.UpdateDistance);

        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (maxWait <= 0)
        {
            yield break;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            yield break;
        }
        
        #endregion

        this.Latitude = Input.location.lastData.latitude;
        this.Longitude = Input.location.lastData.longitude;
        this.Altitude = Input.location.lastData.altitude;

        yield break;
    }
}

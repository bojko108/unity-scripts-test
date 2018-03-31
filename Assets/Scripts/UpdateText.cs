using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateText : MonoBehaviour
{
    public Text LatitudeText;
    public Text LongitudeText;
    public Text AltitudeText;
    
    private void Update()
    {
        this.LatitudeText.text = "Latitude: " + GPS.Instance.Latitude.ToString();
        this.LongitudeText.text = "Longitude: " + GPS.Instance.Longitude.ToString();
        this.AltitudeText.text = "Altitude: " + GPS.Instance.Altitude.ToString();
        this.AltitudeText.text = Random.Range(0f, 100f).ToString();
    }
}

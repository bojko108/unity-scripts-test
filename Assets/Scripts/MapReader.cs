using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Import.GML;
using Import;

public class MapReader : MonoBehaviour
{
    /// <summary>
    /// GML map data source
    /// </summary>
    public string resourceFile;
    /// <summary>
    /// GML map data bounds - used to calculate map's center and reduce the coordinate values
    /// </summary>
    public MapBounds MapBounds;
    /// <summary>
    /// list of polygon features
    /// </summary>
    public List<MapFeature> Features;

    public Boolean IsReady { get; private set; }

    // Use this for initialization
    public void Start()
    {
        // GML map data source
        TextAsset mapData = Resources.Load<TextAsset>(this.resourceFile);

        try
        {
            this.readGMLData(mapData.name, mapData.text);

            this.IsReady = true;
        }
        catch (Exception ex)
        {
            this.IsReady = false;
            Debug.Log(ex);
        }
    }

    //public void Update()
    //{
    //    if (!this.IsReady) return;

    //    // all features are drawn in RED color
    //    Color color = Color.green;

    //    foreach (GMLFeature feature in this.GMLFeatures)
    //    {
    //        // draw a polygon, staring from the second vertex
    //        for (int i = 1; i < feature.Vertices.Count; i++)
    //        {
    //            // reduce coordinates
    //            Vector3 v1 = feature.Vertices[i - 1].ReduceToVector3(this.GMLBounds.Center);
    //            Vector3 v2 = feature.Vertices[i].ReduceToVector3(this.GMLBounds.Center);

    //            Debug.DrawLine(v1, v2, color);

    //            // draw the closing line: vertices[length] to vertices[0]
    //            if (i == feature.Vertices.Count - 1)
    //            {
    //                Vector3 v0 = feature.Vertices[0].ReduceToVector3(this.GMLBounds.Center);
    //                Debug.DrawLine(v2, v0, color);
    //            }
    //        }
    //    }
    //}

    private void readGMLData(string fileName, string gmlText)
    {
        GMLReader gmlReader = new GMLReader(fileName, gmlText);

        this.MapBounds = gmlReader.Bounds;
        this.Features = gmlReader.Features;
    }
}

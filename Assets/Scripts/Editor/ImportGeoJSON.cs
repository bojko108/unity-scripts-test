﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.Linq;

using Import.GeoJSON;
using Import;

public class ImportGeoJSON : ScriptableWizard
{
    [Tooltip("Select the file you want to import from.")]
    public TextAsset GeoJSONSource;
    [Tooltip("Set the horizotnal scale for imported data. By default the horizotnal scale is 1:1, if you need for example scale 1:1000, set this parameter to 1000.")]
    public float HorizontalScale = 1f;
    [Tooltip("Set the vertical scale for imported data. By default the scale is 1:1, if you need for example scale 1:10, set this parameter to 10.")]
    public float VerticalScale = 1f;
    [Tooltip("Set the name of the parent GameObject")]
    public string ParentName = "Map data";
    [Tooltip("Set source ObjectID field name. This field is used for uniquely identifying the game objects.")]
    public string ObjectIDFieldName = "OBJECTID";

    //public bool Extrude = false;
    //public string ExtrudeField;
    //public float ExtrudeFactor;
    //[Tooltip("If the faces are not visible toggle this parameter")]
    //public bool InvertFaces = true;

    [MenuItem("GIS Tools/Import GeoJSON data")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<ImportGeoJSON>("Import data from a GeoJSON file", "Import");
    }

    /// <summary>
    /// Called when the "Import" button is clicked
    /// </summary>
    private void OnWizardCreate()
    {
        GameObject parentGameObject = new GameObject();
        parentGameObject.name = this.ParentName;

        try
        {
            FeatureCollection features = GeoJSONObject.Deserialize(this.GeoJSONSource.text);

            MapBounds bounds = features.bbox;
            bounds.ProjectToWebMercator();
            bounds.SetScale(this.HorizontalScale);

            this.UpdateCameraParameters(bounds.Center.ToVector3(), this.HorizontalScale, this.VerticalScale);

            foreach (FeatureObject ftr in features.features)
            {
                MapFeature feature = new MapFeature(ftr.properties[this.ObjectIDFieldName]);

                List<Vertex> vertices = ftr.geometry.AllPositions().ConvertAll((v) => new Vertex(v.latitude, v.longitude, 0.0));

                feature.SetGeometry(EnumGeometryType.Polygon, vertices);

                if (feature.Geometry.IsEmpty) continue;

                feature.Geometry.ProjectToWebMercator();
                feature.Geometry.SetScale(this.HorizontalScale);

                Vector3 cityOrigin = feature.Geometry.GetCentroid();

                GameObject go = feature.ToGameObject();
                go.transform.position = cityOrigin - bounds.Center.ToVector3();
                go.transform.parent = parentGameObject.transform;

                Material material = new Material(Shader.Find("Standard"));
                material.color = UnityEngine.Random.ColorHSV();
                go.GetComponent<Renderer>().material = material;

                //if (this.Extrude)
                //{
                //    // TODO: get extrusion values from a field....
                //    // TODO: use this.ExtrusionFactor
                //    MeshExtrusion.Extrude(go, UnityEngine.Random.Range(1, 500), this.InvertFaces);
                //}
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }

    private void UpdateCameraParameters(Vector3 origin, float horizontalScale, float verticalScale)
    {
        GameObject camera = GameObject.FindGameObjectWithTag("MainCamera") as GameObject;

        if (camera)
        {
            MapProperties prop = camera.GetComponent<MapProperties>() as MapProperties;
            prop.HorizontalScale = horizontalScale;
            prop.VerticalScale = verticalScale;
            prop.Origin = origin;
        }
    }
}
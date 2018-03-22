using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Import.GML;
using Import;

public class ImportGML : ScriptableWizard
{
    [Tooltip("Select the file you want to import from.")]
    public TextAsset GMLSource;
    [Tooltip("Set the scale for imported data. By default the scale is 1:1, if you need for example scale 1:1000, set 1000 for Scale value.")]
    public float Scale = 1f;
    [Tooltip("Set the name of the parent GameObject")]
    public string ParentName = "Map data";
    [Tooltip("Material to use for game objects")]
    public string MaterialName = "Red";


    public bool Extrude = false;
    public string ExtrudeField;
    public float ExtrudeFactor;
    [Tooltip("If the faces are not visible toggle this parameter")]
    public bool InvertFaces;

    [MenuItem("GIS Tools/Import GML data")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<ImportGML>("Import data from a GML file", "Import");
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
            GMLReader gmlReader = new GMLReader(this.GMLSource.name, this.GMLSource.text);

            MapBounds bounds = gmlReader.Bounds;
            // project geographic to web mercator...
            bounds.ProjectToWebMercator();
            bounds.SetScale(this.Scale);

            foreach (MapFeature feature in gmlReader.Features)
            {
                if (feature.Geometry.IsEmpty) continue;

                // project geographic to web mercator...
                feature.Geometry.ProjectToWebMercator();
                feature.Geometry.SetScale(this.Scale);

                // calculate object's centroid
                Vector3 cityOrigin = feature.Geometry.GetCentroid();
                
                GameObject go = feature.ToGameObject();
                go.transform.position = cityOrigin - bounds.Center.ToVector3();
                go.transform.parent = parentGameObject.transform;

                Material material = new Material(Shader.Find("Standard"));
                material.color = UnityEngine.Random.ColorHSV();
                go.GetComponent<Renderer>().material = material;

                if (this.Extrude == true)
                {
                    // TODO: get extrusion values from a field....
                    // TODO: use this.ExtrusionFactor
                    MeshExtrusion.Extrude(go, UnityEngine.Random.Range(1, 500), this.InvertFaces);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
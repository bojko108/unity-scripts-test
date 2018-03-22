using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

using Import.GeoJSON;

public class ImportGeoJSON : ScriptableWizard
{
    [Tooltip("Select the file you want to import from.")]
    public TextAsset GeoJSONSource;
    [Tooltip("Set the scale for imported data. By default the scale is 1:1, if you need for example scale 1:1000, set 1000 for Scale value.")]
    public float Scale = 1f;
    [Tooltip("Set the name of the parent GameObject")]
    public string ParentName = "Map data";
    [Tooltip("Material to use for game objects")]
    public string MaterialName = "Red";

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
            
            foreach(FeatureObject feature in features.features)
            {
                Debug.Log(feature.type);
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
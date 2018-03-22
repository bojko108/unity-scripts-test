using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Import.GML;
using Import;

public class ReadMapAndDrawFeatures
{
    // scale
    private float scale = 2f;    // 1:2

    //[MenuItem("Tools/Draw From GML")]
    public static void Execute()
    {
        // GML map data source
        TextAsset mapData = Resources.Load<TextAsset>("cities");

        GameObject parentGameObject = new GameObject();
        parentGameObject.name = "Map Objects";

        try
        {
            GMLReader gmlReader = new GMLReader(mapData.name, mapData.text);

            MapBounds bounds = gmlReader.Bounds;
            
            foreach (MapFeature feature in gmlReader.Features)
            {
                if (feature.Geometry.IsEmpty) continue;

                // project geographic to web mercator...
                feature.Geometry.ProjectToWebMercator();

                // calculate object's centroid
                Vector3 cityOrigin = feature.Geometry.GetCentroid();

                // triangulate object vertices to create a mesh
                Poly2Mesh.Polygon polygon = new Poly2Mesh.Polygon();
                polygon.outside = feature.Geometry.Vertices.ConvertAll((v) => v.ReduceToVector3(cityOrigin));

                // create game object and assign a material to it
                GameObject go = Poly2Mesh.CreateGameObject(polygon, feature.FID.ToString());
                go.transform.position = cityOrigin - bounds.Center.ToVector3();

                Material newMat = Resources.Load("CityMaterial", typeof(Material)) as Material;
                go.GetComponent<Renderer>().material = newMat;

                go.transform.parent = parentGameObject.transform;
            }
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Import;

[RequireComponent(typeof(MapReader))]
public class CityMaker : MonoBehaviour
{
    public Material ObjectMaterial;
    public bool Extrude = false;

    private MapReader Map;

    IEnumerator Start()
    {
        this.Map = GetComponent<MapReader>();

        while (!this.Map.IsReady)
        {
            yield return null;
        }

        foreach (MapFeature feature in this.Map.Features)
        {
            if (feature.Geometry.IsEmpty) continue;

            // calculate object's centroid
            Vector3 cityOrigin = feature.Geometry.GetCentroid();

            // triangulate object vertices to create a mesh
            Poly2Mesh.Polygon polygon = new Poly2Mesh.Polygon();
            polygon.outside = feature.Geometry.Vertices.ConvertAll((v) => v.ReduceToVector3(cityOrigin));

            // create game object and assign a material
            GameObject go = Poly2Mesh.CreateGameObject(polygon, feature.FID.ToString(), this.ObjectMaterial);
            go.transform.position = cityOrigin - this.Map.MapBounds.Center.ToVector3();

            if (this.Extrude)
            {
                // Mesh extrusion
                float extrusion = 100f;// UnityEngine.Random.Range(100, 1000);

                Mesh srcMesh = go.GetComponent<MeshFilter>().mesh;
                Mesh extrudedMesh = srcMesh;

                if (extrusion > 0)
                {
                    MeshExtrusion.ExtrudeMesh(srcMesh, go.GetComponent<MeshFilter>().mesh, extrusion, go.transform, false);
                }

                //go.GetComponent<MeshFilter>().mesh = extrudedMesh;
            }

            yield return null;
        }
    }


    // OLD CODE


    //IEnumerator Start()
    //{
    //    this.Map = GetComponent<MapReader>();

    //    while (!this.Map.IsReady)
    //    {
    //        yield return null;
    //    }

    //    // create cities...
    //    foreach (GMLFeature feature in this.Map.GMLFeatures)
    //    {
    //        if (feature.IsEmpty) continue;

    //        Vector3 cityOrigin = feature.GetCentroid();

    //        GameObject go = new GameObject(feature.FID.ToString());
    //        go.transform.position = cityOrigin - this.Map.GMLBounds.Center;

    //        MeshFilter mf = go.AddComponent<MeshFilter>();
    //        MeshRenderer mr = go.AddComponent<MeshRenderer>();

    //        mr.material = this.CityMaterial;

    //        List<Vector3> vectors = new List<Vector3>();
    //        List<Vector3> normals = new List<Vector3>();
    //        List<Vector2> uvs = new List<Vector2>();
    //        List<int> indexes = new List<int>();

    //        // center of the city + 10meters height
    //        Vector3 oTop = new Vector3(0, this.Height, 0);

    //        vectors.Add(oTop);
    //        normals.Add(Vector3.up);
    //        uvs.Add(new Vector2(0.5f, 0.5f));

    //        for (int i = 1; i < feature.Vertices.Count; i++)
    //        {
    //            // reduce coordinates to city's origin = localOrigin
    //            Vector3 v1 = feature.Vertices[i - 1].ToVector3() - cityOrigin;
    //            Vector3 v2 = feature.Vertices[i].ToVector3() - cityOrigin;
    //            Vector3 v3 = v1 + new Vector3(0, this.Height, 0);
    //            Vector3 v4 = v2 + new Vector3(0, this.Height, 0);

    //            vectors.Add(v1);
    //            vectors.Add(v2);
    //            vectors.Add(v3);
    //            vectors.Add(v4);

    //            uvs.Add(new Vector2(0, 0));
    //            uvs.Add(new Vector2(1, 0));
    //            uvs.Add(new Vector2(0, 1));
    //            uvs.Add(new Vector2(1, 1));

    //            normals.Add(-Vector3.forward);
    //            normals.Add(-Vector3.forward);
    //            normals.Add(-Vector3.forward);
    //            normals.Add(-Vector3.forward);

    //            int idx1, idx2, idx3, idx4;
    //            idx4 = vectors.Count - 1;
    //            idx3 = vectors.Count - 2;
    //            idx2 = vectors.Count - 3;
    //            idx1 = vectors.Count - 4;

    //            indexes.Add(idx1);
    //            indexes.Add(idx3);
    //            indexes.Add(idx2);

    //            indexes.Add(idx3);
    //            indexes.Add(idx4);
    //            indexes.Add(idx2);

    //            indexes.Add(idx2);
    //            indexes.Add(idx3);
    //            indexes.Add(idx1);

    //            indexes.Add(idx2);
    //            indexes.Add(idx4);
    //            indexes.Add(idx3);


    //            // roof:
    //            if (this.DrawRoofs == true)
    //            {
    //                indexes.Add(0);
    //                indexes.Add(idx3);
    //                indexes.Add(idx4);

    //                indexes.Add(idx4);
    //                indexes.Add(idx3);
    //                indexes.Add(0);
    //            }



    //            if (i == feature.Vertices.Count - 1)
    //            {
    //                Vector3 v0 = feature.Vertices[0].ToVector3() - cityOrigin;
    //                // v2
    //                Vector3 v30 = v0 + new Vector3(0, this.Height, 0);
    //                Vector3 v40 = v2 + new Vector3(0, this.Height, 0);

    //                vectors.Add(v0);
    //                vectors.Add(v2);
    //                vectors.Add(v30);
    //                vectors.Add(v40);

    //                uvs.Add(new Vector2(0, 0));
    //                uvs.Add(new Vector2(1, 0));
    //                uvs.Add(new Vector2(0, 1));
    //                uvs.Add(new Vector2(1, 1));

    //                normals.Add(-Vector3.forward);
    //                normals.Add(-Vector3.forward);
    //                normals.Add(-Vector3.forward);
    //                normals.Add(-Vector3.forward);

    //                //                    int idx1, idx2, idx3, idx4;
    //                idx4 = vectors.Count - 1;
    //                idx3 = vectors.Count - 2;
    //                idx2 = vectors.Count - 3;
    //                idx1 = vectors.Count - 4;

    //                indexes.Add(idx1);
    //                indexes.Add(idx3);
    //                indexes.Add(idx2);

    //                indexes.Add(idx3);
    //                indexes.Add(idx4);
    //                indexes.Add(idx2);

    //                indexes.Add(idx2);
    //                indexes.Add(idx3);
    //                indexes.Add(idx1);

    //                indexes.Add(idx2);
    //                indexes.Add(idx4);
    //                indexes.Add(idx3);


    //                // roof:
    //                if (this.DrawRoofs)
    //                {
    //                    indexes.Add(0);
    //                    indexes.Add(idx3);
    //                    indexes.Add(idx4);

    //                    indexes.Add(idx4);
    //                    indexes.Add(idx3);
    //                    indexes.Add(0);
    //                }
    //            }
    //        }

    //        mf.mesh.vertices = vectors.ToArray();
    //        mf.mesh.normals = normals.ToArray();
    //        mf.mesh.triangles = indexes.ToArray();
    //        mf.mesh.uv = uvs.ToArray();

    //        yield return null;
    //    }
    //}
}

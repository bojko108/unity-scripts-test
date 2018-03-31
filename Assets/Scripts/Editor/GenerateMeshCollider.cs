using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GenerateMeshCollider : ScriptableWizard
{
    public GameObject GameObject;

    [MenuItem("Tools/Generate Mesh Collider")]
    private static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<GenerateMeshCollider>("Generate a Mesh collider from all children", "Generate");
    }

    private void OnWizardCreate()
    {
        try
        {
            MeshFilter[] meshFilters = this.GameObject.GetComponentsInChildren<MeshFilter>();
            CombineInstance[] combine = new CombineInstance[meshFilters.Length];

            int i = 0;
            while (i < meshFilters.Length)
            {
                combine[i].mesh = meshFilters[i].sharedMesh;
                combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
                //meshFilters[i].gameObject.SetActive(false);
                i++;
            }

            MeshCollider meshCollider = null;

            if (this.GameObject.GetComponent<MeshCollider>() != null)
            {
                meshCollider = this.GameObject.transform.GetComponent<MeshCollider>();
            }
            else
            {
                meshCollider = this.GameObject.AddComponent(typeof(MeshCollider)) as MeshCollider;
            }

            if (meshCollider.sharedMesh == null)
            {
                meshCollider.sharedMesh = new Mesh();
            }

            meshCollider.sharedMesh.CombineMeshes(combine);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
    }
}
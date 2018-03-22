using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtrudeTest : MonoBehaviour
{
    public float Extrusion;
    public bool InvertFaces = false;

    private Mesh srcMesh;
    private MeshExtrusion.Edge[] precomputedEdges;


    private void Awake()
    {
        this.srcMesh = GetComponent<MeshFilter>().mesh;
        this.precomputedEdges = MeshExtrusion.BuildManifoldEdges(this.srcMesh);
    }

    private void Start()
    {
        Matrix4x4[] finalSections = this.PrepareMesh();
        MeshExtrusion.ExtrudeMesh(this.srcMesh, GetComponent<MeshFilter>().mesh, finalSections, this.precomputedEdges, this.InvertFaces);
    }

    private Matrix4x4[] PrepareMesh()
    {
        Matrix4x4[] finalSections = new Matrix4x4[2];

        //Matrix4x4 matrix = transform.localToWorldMatrix;
        Vector3 currentPosition = this.transform.position;
        Vector3 targetPosition = currentPosition;
        targetPosition.y += this.Extrusion;

        //Quaternion direction = Quaternion.LookRotation(targetPosition - currentPosition);
        Quaternion direction = Quaternion.LookRotation(this.transform.forward, this.transform.up);

        Matrix4x4 worldToLocal = this.transform.worldToLocalMatrix;

        finalSections[0] = worldToLocal * Matrix4x4.TRS(currentPosition, direction, this.transform.lossyScale);
        finalSections[1] = worldToLocal * Matrix4x4.TRS(targetPosition, direction, this.transform.lossyScale);

        return finalSections;
    }
}
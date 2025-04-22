using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SightVisualizer : MonoBehaviour
{
    [Header("Sight Settings")]
    public float radius = 10f;
    public float angle = 90f;
    public int segments = 30;
    public float yOffset = 0.1f;

    [Header("Visual Settings")]
    public Color viewColor = new Color(1f, 1f, 0f, 0.2f); 
    public Material customMaterial;

    private Mesh viewMesh;

    void Start()
    {
        GetComponent<MeshFilter>().mesh = viewMesh = new Mesh();
        if (customMaterial != null)
        {
            GetComponent<MeshRenderer>().material = customMaterial;
        }
        else
        {
            var defaultMat = new Material(Shader.Find("Unlit/Color")) { color = viewColor };
            GetComponent<MeshRenderer>().material = defaultMat;
        }
    }

    void LateUpdate()
    {
        DrawViewCone();
        transform.localPosition = new Vector3(0, yOffset, 0);
    }

    void DrawViewCone()
    {
        viewMesh.Clear();

        int stepCount = Mathf.Clamp(segments, 3, 100);
        float stepAngle = angle / stepCount;

        List<Vector3> vertices = new List<Vector3> { Vector3.zero };
        List<int> triangles = new List<int>();

        for (int i = 0; i <= stepCount; i++)
        {
            float currentAngle = -angle / 2 + stepAngle * i;
            float rad = currentAngle * Mathf.Deg2Rad;
            Vector3 dir = new Vector3(Mathf.Sin(rad), 0, Mathf.Cos(rad));
            vertices.Add(dir * radius);
        }

        for (int i = 1; i < vertices.Count - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i);
            triangles.Add(i + 1);
        }

        viewMesh.SetVertices(vertices);
        viewMesh.SetTriangles(triangles, 0);
        viewMesh.RecalculateNormals();
    }
}

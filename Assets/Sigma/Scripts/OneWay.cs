using System.Collections.Generic;
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshCollider))]
public class OneWay : MonoBehaviour
{
    public float Angle = 45.0f;
    public Vector3 Direction = new Vector3(1, 0, 0);
    public bool InvertDirection = false;

    // Collider mesh collides only with objects that come from outside, not from inside. This is determined by normals.
    // So we need to analyze each normal and sometimes remove associated triangles.
    //
    // Allowed direction that we want to achieve is defined using a cone - a direction and an angle.
	void Start ()
	{
        // Prcalculate some values
	    Vector3 actualDirection = InvertDirection ? -Direction : Direction;
        float cos = Mathf.Cos(Angle);

        // Get the mesh objects
        MeshFilter meshFilter = GetComponent<MeshFilter>();
        MeshCollider meshCollider = GetComponent<MeshCollider>();
        if (meshFilter == null || meshCollider == null || meshFilter.sharedMesh == null)
        {
            Debug.LogError("Need a MeshFilter and a MeshCollider. Maybe you forgot to set gameObject to be NON-static?");
            return;
        }

        // Get all faces (triangles) of the mesh
        Vector3[] vertices = meshFilter.sharedMesh.vertices;
        List<int> triangles = new List<int>(meshFilter.sharedMesh.triangles);

        // Cycle through every face (triangle = 3 points)
        for (int i = triangles.Count - 1; i >= 0; i -= 3)
        {
            // If angle between face's normal and predefined direction is greater than predefined angle - remove that face
            Vector3 point1 = transform.TransformPoint(vertices[triangles[i - 2]]);
            Vector3 point2 = transform.TransformPoint(vertices[triangles[i - 1]]);
            Vector3 point3 = transform.TransformPoint(vertices[triangles[i]]);
            Vector3 faceNormal = Vector3.Cross(point3 - point2, point1 - point2).normalized;
            if (Vector3.Dot(faceNormal, actualDirection) <= cos)
            {
                triangles.RemoveAt(i);
                triangles.RemoveAt(i - 1);
                triangles.RemoveAt(i - 2);
            }
        }

        // Assign the new triangle array to a new mesh
        Mesh mesh = new Mesh();
        mesh.vertices = vertices;
        mesh.triangles = triangles.ToArray();

        // Make this new mesh be our collider mesh
        meshCollider.sharedMesh = mesh;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

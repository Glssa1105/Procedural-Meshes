using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer),typeof(MeshFilter))]
public class SimpleProceduralMesh : MonoBehaviour
{
    private void OnEnable()
    {
        Mesh mesh = new Mesh {
            name = "Procedural Mesh"
        };

        GetComponent<MeshFilter>().mesh = mesh;

        mesh.vertices = new Vector3[]
        {
            Vector3.zero,Vector3.up,Vector3.right,new Vector3(1f,1f)
        };

        mesh.triangles = new int[]
        {
            0,1,2,2,1,3
        };
        mesh.normals = new Vector3[]
        {
            Vector3.back,Vector3.back,Vector3.back,Vector3.back
        };

        mesh.uv = new Vector2[]
            {
                Vector2.zero,Vector2.up,Vector2.right,Vector2.one
            };

        mesh.tangents = new Vector4[] {
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f, 0f, 0f, -1f),
            new Vector4(1f,0f,0f, -1f)
        };

    }
}

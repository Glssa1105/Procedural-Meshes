using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshes
{
    public struct Vertex
    {
        public float3 position, normal;
        public float4 tangent;
        public float2 texCoord0;
    }

    public interface IMeshStreams
    {
        public void Setup(Mesh.MeshData data, int vertexCount, int indexCount);

        public void SetVertex(int index, Vertex data);

        public void SetTriangle(int index, int3 triangle);
    }

}
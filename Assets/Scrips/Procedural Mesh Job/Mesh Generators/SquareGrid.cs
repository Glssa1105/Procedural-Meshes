using System.Collections;
using Unity.Burst;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

namespace ProceduralMeshes.Generators
{
    public struct SquareGrid : IMeshGenerator
    {
        public int VertexCount => 4;

        public int IndexCount => 6;

        public int JobLength => 1;

        public void Execute<S>(int i, S stream) where S : struct, IMeshStreams
        {
            var vertex = new Vertex();
            vertex.normal.z = -1f;
            vertex.tangent.xw = math.float2(1f, -1f);
            
            stream.SetVertex(0,vertex);
            vertex.position = math.right();
            vertex.texCoord0 = math.float2(1f, 0f);
            stream.SetVertex(1, vertex);

            vertex.position = math.up();
            vertex.texCoord0 = math.float2(0f, 1f);
            stream.SetVertex(2, vertex);

            vertex.position = math.float3(1f, 1f, 0f);
            vertex.texCoord0 = 1f;
            stream.SetVertex(3, vertex);
            
            stream.SetTriangle(0,math.int3(0,2,1));
            stream.SetTriangle(1,math.int3(1,2,3));
        }
        
        

    }
}
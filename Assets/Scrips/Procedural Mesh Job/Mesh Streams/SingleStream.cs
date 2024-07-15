using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralMeshes.Streams
{
    public class SingleStream : IMeshStreams
    {

        [StructLayout(LayoutKind.Sequential)]
        struct Stream0
        {
            public float3 position, normal;
            public float4 tangent;
            public float2 texCoord0;
        }

        NativeArray<Stream0> stream_0;
        NativeArray<int3> triangles;

        public void Setup(Mesh.MeshData data, int vertexCount, int indexCount)
        {
            Mesh.MeshDataArray dataArray = Mesh.AllocateWritableMeshData(1);
            var meshData = dataArray[0];

            var descriptor = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            descriptor[0] = new VertexAttributeDescriptor(VertexAttribute.Position,dimension:3);
            descriptor[1] = new VertexAttributeDescriptor(VertexAttribute.Normal,dimension:3);
            descriptor[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent,dimension:4);
            descriptor[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, dimension: 2);
            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            meshData.subMeshCount = 1;
            meshData.SetSubMesh(0, new SubMeshDescriptor(0, indexCount));

            stream_0 = meshData.GetVertexData<Stream0>();
            triangles = meshData.GetIndexData<int>().Reinterpret<int3>(sizeof(int));
        }

        //尽可能使用内联，减少function call
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void SetVertex(int index, Vertex data) => stream_0[index] = new Stream0
        {
            position = data.position,
            normal = data.normal,
            tangent = data.tangent, 
            texCoord0 = data.texCoord0,
        };

        
        public void SetTriangle(int index, int3 triangle) => triangles[index] = triangle;



    }
}

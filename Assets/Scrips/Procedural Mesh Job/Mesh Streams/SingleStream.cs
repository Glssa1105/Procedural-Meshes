using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Rendering;

namespace ProceduralMeshes.Streams
{
    public struct SingleStream : IMeshStreams
    {

        [StructLayout(LayoutKind.Sequential)]
        struct Stream0
        {
            public float3 position, normal;
            public float4 tangent;
            public float2 texCoord0;
        }
        
        [NativeDisableContainerSafetyRestriction]
        NativeArray<Stream0> stream_0;
        
        [NativeDisableContainerSafetyRestriction]
        NativeArray<int3> triangles;

        public void Setup(Mesh.MeshData meshData, int vertexCount, int indexCount)
        {
            var descriptor = new NativeArray<VertexAttributeDescriptor>(4, Allocator.Temp, NativeArrayOptions.UninitializedMemory);
            descriptor[0] = new VertexAttributeDescriptor(VertexAttribute.Position, dimension: 3);
            descriptor[1] = new VertexAttributeDescriptor(VertexAttribute.Normal, dimension: 3);
            descriptor[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, dimension: 4);
            descriptor[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, dimension: 2);
            meshData.SetVertexBufferParams(vertexCount, descriptor);
            descriptor.Dispose();

            meshData.SetIndexBufferParams(indexCount, IndexFormat.UInt32);
            meshData.subMeshCount = 1;
            NativeArray<UInt32> triangleIndeces = meshData.GetIndexData<UInt32>();
            triangleIndeces[0] = 0;
            triangleIndeces[1] = 2;
            triangleIndeces[2] = 1;
            triangleIndeces[3] = 1;
            triangleIndeces[4] = 2;
            triangleIndeces[5] = 3;

            stream_0 = meshData.GetVertexData<Stream0>();
            triangles = meshData.GetIndexData<UInt32>().Reinterpret<int3>(sizeof(UInt32));

            
            meshData.SetSubMesh(
                0, new SubMeshDescriptor(0, indexCount),
                MeshUpdateFlags.DontRecalculateBounds |
                MeshUpdateFlags.DontValidateIndices
            );
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

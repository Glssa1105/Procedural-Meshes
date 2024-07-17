using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using Unity.Mathematics;
using Unity.Collections;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class AdvancedSingleStreamProceduralMesh : MonoBehaviour
{
    [StructLayout(LayoutKind.Sequential)]
    struct Vertex
    {
        public float3 position, normal;
        public half4 tangent;
        public half2 texCoord0;
    }

    private void OnEnable()
    {
        int vertexAttributeCount = 4;
        int vertexCount = 4;
        int triangleIndexCount = 6;

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(
           vertexAttributeCount, Allocator.Temp, NativeArrayOptions.UninitializedMemory
           );

        vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position,dimension: 3);
        vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal,dimension:3);
        vertexAttributes[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent, VertexAttributeFormat.Float16, dimension: 4);
        vertexAttributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, 2 
       );
        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();



        half h0 = math.half(0f), h1 = math.half(1f);

        var vertex = new Vertex
        {
            normal = math.back(),
            tangent = math.half4(h1, h0, h0, math.half(-1f))
        };

        NativeArray<Vertex> vertices = meshData.GetVertexData<Vertex>();

        vertex.position = 0f;
        vertex.texCoord0 = h0;
        vertices[0] = vertex;

        vertex.position = math.right();
        vertex.texCoord0 = math.half2(h1, h0);
        vertices[1] = vertex;

        vertex.position = math.up();
        vertex.texCoord0 = math.half2(h0, h1);
        vertices[2] = vertex;

        vertex.position = math.float3(1f, 1f, 0f);
        vertex.texCoord0 = h1;
        vertices[3] = vertex;

        meshData.SetIndexBufferParams(triangleIndexCount, IndexFormat.UInt16);
        NativeArray<ushort> triangleIndeces = meshData.GetIndexData<ushort>();
        triangleIndeces[0] = 0;
        triangleIndeces[1] = 2;
        triangleIndeces[2] = 1;
        triangleIndeces[3] = 1;
        triangleIndeces[4] = 2;
        triangleIndeces[5] = 3;


        var bounds = new Bounds(new Vector3(0.5f, 0.5f), new Vector3(1f, 1f));
        //设置submesh，submesh是mesh下的一系列索引，用于指定Mesh中的一部分以用于不同材质渲染
        //可以设置MeshUpdateFlags来控制submesh的数据更新，如包围盒更新，骨骼位置重置等...
        meshData.subMeshCount = 1;
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangleIndexCount)
        {
            bounds = bounds,
            vertexCount = vertexCount
        }, MeshUpdateFlags.DontRecalculateBounds);


        var mesh = new Mesh
        {
            bounds = bounds,
            name = "Procedural Mesh"
        };


        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray, mesh);
        GetComponent<MeshFilter>().mesh = mesh;
    }

}

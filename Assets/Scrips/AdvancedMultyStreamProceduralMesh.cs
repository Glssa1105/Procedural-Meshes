using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using Unity.Mathematics;
using Unity.VisualScripting;
[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class AdvancedMultyStreamProceduralMesh : MonoBehaviour
{
    private void OnEnable()
    {
        //设置MeshDate
        //声明属性(attribute)格式->设置顶点格式->设置顶点数据
        //声明/设置索引(index)格式->设置顶点数据
        //可类比OpenGL，VBO&VAO 

        int vertexAttributeCount = 4;
        int vertexCount = 4;
        int triangleIndex = 6;

        Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
        Mesh.MeshData meshData = meshDataArray[0];

        //声明属性数组
        //NativeArrayOptions可以为clear(清零)或uninitialized(跳过清零)
        var vertexAttributes = new NativeArray<VertexAttributeDescriptor>(
            vertexAttributeCount, Allocator.Temp,NativeArrayOptions.UninitializedMemory
            );
        
        //声明属性格式,重点*：每个属性的总大小必须是四个字节的倍数，这也是为什么不能对Position和normal简单采用float16
        vertexAttributes[0] = new VertexAttributeDescriptor(VertexAttribute.Position, dimension: 3, stream: 0);
        vertexAttributes[1] = new VertexAttributeDescriptor(VertexAttribute.Normal,dimension: 3, stream: 1);
        //可以通过设置VertexAttributeFormat减小占用缓解内存压力与提高Cache成功率
        vertexAttributes[2] = new VertexAttributeDescriptor(VertexAttribute.Tangent,VertexAttributeFormat.Float16,dimension: 4, stream: 2);
        vertexAttributes[3] = new VertexAttributeDescriptor(VertexAttribute.TexCoord0, VertexAttributeFormat.Float16, dimension:2, stream: 3);

        //根据声明设置格式
        meshData.SetVertexBufferParams(vertexCount, vertexAttributes);
        vertexAttributes.Dispose();

        //根据stream来GetVertexData
        NativeArray<float3> positions = meshData.GetVertexData<float3>(0);
        positions[0] = 0f;
        positions[1] = math.right();
        positions[2] = math.up();
        positions[3] = math.float3(1f, 1f, 0f);

        NativeArray<float3> normals = meshData.GetVertexData<float3>(1);
        normals[0] = normals[1] = normals[2] = normals[3] = math.back();

        //用16bit占用的half替代占用32bit的float32
        half h0 = math.half(0f),h1 = math.half(1f);
        NativeArray<half4> tangents = meshData.GetVertexData<half4>(2);
        tangents[0] = tangents[1] = tangents[2] = tangents[3] = math.half4(h1,h0,h0, math.half(-1f));

        NativeArray<half2> texCoords = meshData.GetVertexData<half2>(3);
        texCoords[0] = h0;
        texCoords[1] = math.half2(h1, h0);
        texCoords[2] = math.half2(h0, h1); 
        texCoords[3] = h1;

        //设置顶点格式
        //UInt16使用ushort,可以表示65535个顶点；UInt32对应uint
        meshData.SetIndexBufferParams(triangleIndex, IndexFormat.UInt16);
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
        meshData.SetSubMesh(0, new SubMeshDescriptor(0, triangleIndex)
        {
            bounds = bounds,
            vertexCount = vertexCount
        }, MeshUpdateFlags.DontRecalculateBounds);


        var mesh = new Mesh
        {
            bounds = bounds,
            name = "Procedural Mesh"
        };


        Mesh.ApplyAndDisposeWritableMeshData(meshDataArray,mesh);
        GetComponent<MeshFilter>().mesh = mesh;

        
    }

    
}

﻿using System;
using ProceduralMeshes;
using ProceduralMeshes.Generators;
using ProceduralMeshes.Streams;
using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ProceduralMesh : MonoBehaviour
{
        private Mesh mesh;

        private void Awake()
        {
                mesh = new Mesh()
                {
                        name = "Procedural Mesh"
                };
                GenerateMesh();
                GetComponent<MeshFilter>().mesh = mesh;
        }

        void GenerateMesh()
        {
                Mesh.MeshDataArray meshDataArray = Mesh.AllocateWritableMeshData(1);
                Mesh.MeshData meshData = meshDataArray[0];
                
                MeshJob<SquareGrid,SingleStream>.ScheduleParallel(meshData,default).Complete();
                Mesh.ApplyAndDisposeWritableMeshData(meshDataArray,mesh);
        }
}
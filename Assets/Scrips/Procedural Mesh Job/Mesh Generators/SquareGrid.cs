﻿using System.Collections;
using Unity.Burst;
using Unity.Jobs;
using UnityEngine;

namespace ProceduralMeshes.Generators
{
    public struct SquareGrid : IMeshGenerator
    {
        public int VertexCount => 0;

        public int IndexCount => 0;

        public int JobLength => 1;

        public void Execute<S>(int i, S stream) where S : struct, IMeshStreams
        {

        }
        

    }
}
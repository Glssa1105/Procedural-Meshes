using UnityEngine;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
namespace ProceduralMeshes
{
    public interface IMeshGenerator
    {
        int VertexCount { get; }

        int IndexCount { get; }

        int JobLength { get; }

        void Execute<S>(int i, S stream) where S : struct, IMeshStreams;
    }
}

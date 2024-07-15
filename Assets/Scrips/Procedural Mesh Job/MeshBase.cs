using Unity.Burst;
using Unity.Jobs;

namespace ProceduralMeshes
{
    [BurstCompile(FloatPrecision.Standard,FloatMode.Fast,CompileSynchronously = true)]
    public struct MeshJob<G, S> : IJobFor
        where G : struct, IMeshGenerator
        where S : struct, IMeshStreams
    {
        public void Execute(int index)
        {
            throw new System.NotImplementedException();
        }
    }
}

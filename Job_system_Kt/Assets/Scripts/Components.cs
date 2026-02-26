using Unity.Entities;
using Unity.Mathematics;

public struct MoveSpeed : IComponentData
{
    public float Value;
}

public struct OrbitRadius : IComponentData
{
    public float Value;
}

public struct OrbitCenter : IComponentData
{
    public float3 Value;
}

public struct PhaseOffset : IComponentData
{
    public float Value;
}
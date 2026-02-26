using Unity.Burst;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct OrbitMovementSystem : ISystem
{
    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        float elapsedTime = (float)SystemAPI.Time.ElapsedTime;

        new OrbitMovementJob
        {
            ElapsedTime = elapsedTime
        }.ScheduleParallel();
    }
}

[BurstCompile]
public partial struct OrbitMovementJob : IJobEntity
{
    public float ElapsedTime;

    public void Execute(
        ref LocalTransform transform,
        in MoveSpeed speed,
        in OrbitRadius radius,
        in OrbitCenter center,
        in PhaseOffset phase)
    {
        float angle = ElapsedTime * speed.Value + phase.Value;

        float3 offset = new float3(
            math.sin(angle) * radius.Value,
            0f,
            math.cos(angle) * radius.Value
        );

        transform.Position = center.Value + offset;
    }
}
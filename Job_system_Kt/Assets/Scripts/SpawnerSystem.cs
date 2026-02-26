using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

[BurstCompile]
public partial struct SpawnerSystem : ISystem
{
    [BurstCompile]
    public void OnCreate(ref SystemState state)
    {
        state.RequireForUpdate<SpawnerConfig>();
    }

    [BurstCompile]
    public void OnUpdate(ref SystemState state)
    {
        var configEntity = SystemAPI.GetSingletonEntity<SpawnerConfig>();
        var config = SystemAPI.GetComponent<SpawnerConfig>(configEntity);

        if (config.HasSpawned)
            return;

        config.HasSpawned = true;
        SystemAPI.SetComponent(configEntity, config);

        int totalCount = config.CountX * config.CountZ;

        var ecb = new EntityCommandBuffer(Allocator.Temp);

        float offsetX = (config.CountX - 1) * config.Spacing * 0.5f;
        float offsetZ = (config.CountZ - 1) * config.Spacing * 0.5f;

        var rng = new Random(42);

        for (int i = 0; i < config.CountX; i++)
        {
            for (int j = 0; j < config.CountZ; j++)
            {
                var entity = ecb.Instantiate(config.Prefab);

                float3 gridPosition = new float3(
                    i * config.Spacing - offsetX,
                    0f,
                    j * config.Spacing - offsetZ
                );

                ecb.SetComponent(entity, LocalTransform.FromPosition(gridPosition));
                ecb.SetComponent(entity, new OrbitCenter { Value = gridPosition });
                ecb.SetComponent(entity, new MoveSpeed { Value = config.MoveSpeed });
                ecb.SetComponent(entity, new OrbitRadius { Value = config.OrbitRadius });
                ecb.SetComponent(entity, new PhaseOffset { Value = rng.NextFloat(0f, math.PI * 2f) });
            }
        }

        ecb.Playback(state.EntityManager);
        ecb.Dispose();
    }
}
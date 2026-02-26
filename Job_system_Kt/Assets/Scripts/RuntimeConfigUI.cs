using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

public class RuntimeConfigUI : MonoBehaviour
{
    [Header("Параметры движения (изменяются в реальном времени)")]
    [Tooltip("Скорость вращения")]
    public float moveSpeed = 2f;

    [Tooltip("Радиус орбиты")]
    public float orbitRadius = 0.5f;

    [Header("Информация (только чтение)")]
    [SerializeField] private int totalEntities;

    private float previousSpeed;
    private float previousRadius;

    void Start()
    {
        previousSpeed = moveSpeed;
        previousRadius = orbitRadius;
    }

    void Update()
    {
        if (!Application.isPlaying) return;

        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null || !world.IsCreated) return;

        var entityManager = world.EntityManager;

        var query = entityManager.CreateEntityQuery(typeof(MoveSpeed));
        totalEntities = query.CalculateEntityCount();

        bool speedChanged = math.abs(moveSpeed - previousSpeed) > 0.001f;
        bool radiusChanged = math.abs(orbitRadius - previousRadius) > 0.001f;

        if (speedChanged || radiusChanged)
        {
            var entities = query.ToEntityArray(Unity.Collections.Allocator.Temp);

            for (int i = 0; i < entities.Length; i++)
            {
                if (speedChanged)
                {
                    entityManager.SetComponentData(entities[i], new MoveSpeed { Value = moveSpeed });
                }
                if (radiusChanged)
                {
                    entityManager.SetComponentData(entities[i], new OrbitRadius { Value = orbitRadius });
                }
            }

            entities.Dispose();

            previousSpeed = moveSpeed;
            previousRadius = orbitRadius;
        }
    }
}
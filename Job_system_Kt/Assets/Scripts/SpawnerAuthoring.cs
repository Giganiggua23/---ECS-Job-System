using Unity.Entities;
using UnityEngine;

public struct SpawnerConfig : IComponentData
{
    public Entity Prefab;
    public int CountX;
    public int CountZ;
    public float Spacing;
    public float MoveSpeed;
    public float OrbitRadius;
    public bool HasSpawned; 
}

public class SpawnerAuthoring : MonoBehaviour
{
    [Header("Prefab сущности (куб с OrbitAuthoring)")]
    public GameObject prefab;

    [Header("Сетка спавна")]
    [Tooltip("Количество по X")]
    public int countX = 224;
    [Tooltip("Количество по Z")]
    public int countZ = 224;
    [Tooltip("Расстояние между объектами")]
    public float spacing = 2f;

    [Header("Параметры движения")]
    public float moveSpeed = 2f;
    public float orbitRadius = 0.5f;

    public class Baker : Baker<SpawnerAuthoring>
    {
        public override void Bake(SpawnerAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.None);

            AddComponent(entity, new SpawnerConfig
            {
                Prefab = GetEntity(authoring.prefab, TransformUsageFlags.Dynamic),
                CountX = authoring.countX,
                CountZ = authoring.countZ,
                Spacing = authoring.spacing,
                MoveSpeed = authoring.moveSpeed,
                OrbitRadius = authoring.orbitRadius,
                HasSpawned = false
            });
        }
    }
}
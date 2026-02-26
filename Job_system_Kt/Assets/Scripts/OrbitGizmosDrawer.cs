using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;

public class OrbitGizmosDrawer : MonoBehaviour
{
    [Header("Настройки Gizmos")]
    [Tooltip("Максимальное количество сущностей для отображения (0 = все)")]
    public int maxDisplay = 500;

    [Tooltip("Размер сферы Gizmo")]
    public float gizmoSize = 0.3f;

    [Tooltip("Цвет позиции сущности")]
    public Color entityColor = Color.cyan;

    [Tooltip("Показывать центры орбит")]
    public bool showOrbitCenters = true;

    [Tooltip("Цвет центра орбиты")]
    public Color centerColor = Color.yellow;

    [Tooltip("Показывать линию от центра к сущности")]
    public bool showOrbitLines = false;

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;

        var world = World.DefaultGameObjectInjectionWorld;
        if (world == null || !world.IsCreated)
            return;

        var entityManager = world.EntityManager;

        var query = entityManager.CreateEntityQuery(
            typeof(LocalTransform),
            typeof(OrbitCenter)
        );

        var entities = query.ToEntityArray(Unity.Collections.Allocator.Temp);

        int count = entities.Length;
        if (maxDisplay > 0 && count > maxDisplay)
            count = maxDisplay;

        for (int i = 0; i < count; i++)
        {
            var entity = entities[i];
            var transform = entityManager.GetComponentData<LocalTransform>(entity);
            var center = entityManager.GetComponentData<OrbitCenter>(entity);

            Gizmos.color = entityColor;
            Gizmos.DrawSphere(transform.Position, gizmoSize);

            if (showOrbitCenters)
            {
                Gizmos.color = centerColor;
                Gizmos.DrawWireSphere(
                    new Vector3(center.Value.x, center.Value.y, center.Value.z),
                    gizmoSize * 0.5f
                );
            }

            if (showOrbitLines)
            {
                Gizmos.color = Color.gray;
                Gizmos.DrawLine(
                    new Vector3(center.Value.x, center.Value.y, center.Value.z),
                    new Vector3(transform.Position.x, transform.Position.y, transform.Position.z)
                );
            }
        }

        entities.Dispose();
    }
}
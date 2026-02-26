using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;
public class OrbitAuthoring : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float orbitRadius = 3f;

    public class Baker : Baker<OrbitAuthoring>
    {
        public override void Bake(OrbitAuthoring authoring)
        {
            var entity = GetEntity(TransformUsageFlags.Dynamic);

            AddComponent(entity, new MoveSpeed { Value = authoring.moveSpeed });
            AddComponent(entity, new OrbitRadius { Value = authoring.orbitRadius });
            AddComponent(entity, new OrbitCenter { Value = float3.zero });
            AddComponent(entity, new PhaseOffset { Value = 0f });
        }
    }
}
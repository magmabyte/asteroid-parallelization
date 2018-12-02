using JetBrains.Annotations;
using Unity.Burst;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;
using UnityEngine;

namespace Asteroids.Ecs {

[UsedImplicitly]
public class AsteroidMovementEcsSystem : JobComponentSystem {
    protected override JobHandle OnUpdate(JobHandle inputDependencies) {
        var job = new AsteroidEcsMovementJob {
            zBound = Globals.Instance.zBound,
            deltaTime = Time.deltaTime
        };

        return job.Schedule(this);
    }
}

[BurstCompile]
struct AsteroidEcsMovementJob : IJobProcessComponentData<Position, Rotation, AsteroidSettings> {
    public MinMax zBound;
    public float deltaTime;

    public void Execute(ref Position position, ref Rotation rotation,
                        [ReadOnly] ref AsteroidSettings settings) {
        var newPosition = position.Value + settings.speed * deltaTime;
        if (newPosition.z < zBound.min)
            newPosition = new Vector3(newPosition.x, newPosition.y, zBound.max);
        position.Value = newPosition;

        var newRotation = rotation.Value * Quaternion.Euler(settings.angularSpeed * deltaTime);
        rotation.Value = newRotation;
    }
}

}

using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UI;

namespace Asteroids.Jobs {

public class AsteroidLeanJobMovement : MonoBehaviour {
    [SerializeField] JobAsteroid[] asteroidTemplates;
    [SerializeField] int numSpawning;

    Globals globals;
    DebugCanvas debugCanvas;
    TransformAccessArray transforms;
    NativeArray<Vector3> velocity;
    NativeArray<Vector3> angularVelocity;

    JobHandle jobHandle;

    void OnDisable() {
        jobHandle.Complete();
        transforms.Dispose();
        velocity.Dispose();
        angularVelocity.Dispose();
    }

    void Start() {
        globals = Globals.Instance;
        debugCanvas = FindObjectOfType<DebugCanvas>();
        transforms = new TransformAccessArray(0, -1);
    }

    void SpawnAsteroids() {
        var previousLength = transforms.length;
        transforms.capacity = previousLength + numSpawning;

        var previousVelocity = velocity;
        velocity = new NativeArray<Vector3>(transforms.capacity, Allocator.Persistent);

        var previousAngularVelocity = angularVelocity;
        angularVelocity = new NativeArray<Vector3>(transforms.capacity, Allocator.Persistent);

        if (previousLength > 0) {
            for (var i = 0; i < previousLength; i++) {
                velocity[i] = previousVelocity[i];
                angularVelocity[i] = previousAngularVelocity[i];
            }

            previousVelocity.Dispose();
            previousAngularVelocity.Dispose();
        }

        for (var i = previousLength; i < transforms.capacity; i++) {
            var asteroid = Instantiate(asteroidTemplates[Random.Range(0, asteroidTemplates.Length)],
                                       new Vector3(Random.Range(globals.xBound.min, globals.xBound.max), 0, 0),
                                       Quaternion.identity, transform);
            velocity[i] = asteroid.speed;
            angularVelocity[i] = asteroid.angularSpeed;
            transforms.Add(asteroid.transform);
        }

        debugCanvas.SetNumAsteroids(transforms.length);
    }

    void Update() {
        jobHandle.Complete();

        if (Input.GetKeyDown(KeyCode.K))
            SpawnAsteroids();

        var job = new AsteroidLeanMovementJob(velocity, angularVelocity, globals.zBound, Time.deltaTime);
        jobHandle = job.Schedule(transforms);

        JobHandle.ScheduleBatchedJobs();
    }
}

[BurstCompile]
public struct AsteroidLeanMovementJob : IJobParallelForTransform {
    readonly NativeArray<Vector3> velocity;
    readonly NativeArray<Vector3> angularVelocity;
    readonly MinMax zBound;
    readonly float deltaTime;

    public AsteroidLeanMovementJob(NativeArray<Vector3> velocity,
                                   NativeArray<Vector3> angularVelocity,
                                   MinMax zBound,
                                   float deltaTime) {
        this.velocity = velocity;
        this.angularVelocity = angularVelocity;
        this.zBound = zBound;
        this.deltaTime = deltaTime;
    }

    public void Execute(int i, TransformAccess transform) {
        var position = transform.position + velocity[i] * deltaTime;
        if (position.z < zBound.min)
            position = new Vector3(position.x, position.y, zBound.max);

        transform.position = position;
        transform.rotation *= Quaternion.Euler(angularVelocity[i] * deltaTime);
    }
}

}


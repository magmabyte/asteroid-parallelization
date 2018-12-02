using System.Collections.Generic;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Jobs {

public class AsteroidJobMovement : MonoBehaviour {
    [SerializeField] JobAsteroid[] asteroidTemplates;
    [SerializeField] int numSpawning;

    readonly List<JobAsteroid> asteroids = new List<JobAsteroid>();
    NativeArray<Vector3> positions;
    NativeArray<Quaternion> rotations;
    NativeArray<Vector3> velocity;
    NativeArray<Vector3> angularVelocity;

    DebugCanvas debugCanvas;
    JobHandle jobHandle;
    Globals globals;

    void Awake() {
        globals = Globals.Instance;
        debugCanvas = FindObjectOfType<DebugCanvas>();
    }

    void SpawnAsteroids() {
        for (var i = 0; i < numSpawning; i++)
            asteroids.Add(Instantiate(asteroidTemplates[Random.Range(0, asteroidTemplates.Length)],
                                      new Vector3(Random.Range(globals.xBound.min, globals.xBound.max), 0, 0),
                                      Quaternion.identity, transform));

        debugCanvas.SetNumAsteroids(asteroids.Count);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.K))
            SpawnAsteroids();

        var numAsteroids = asteroids.Count;
        if (numAsteroids <= 0)
            return;

        positions = new NativeArray<Vector3>(numAsteroids, Allocator.Persistent);
        rotations = new NativeArray<Quaternion>(numAsteroids, Allocator.Persistent);

        velocity = new NativeArray<Vector3>(numAsteroids, Allocator.Persistent);
        angularVelocity = new NativeArray<Vector3>(numAsteroids, Allocator.Persistent);
        for (var i = 0; i < numAsteroids; i++) {
            positions[i] = asteroids[i].transform.position;
            rotations[i] = asteroids[i].transform.rotation;
            velocity[i] = asteroids[i].speed;
            angularVelocity[i] = asteroids[i].angularSpeed;
        }

        var job = new AsteroidMovementJob(velocity, positions, angularVelocity, rotations,
                                          globals.zBound, Time.deltaTime);
        jobHandle = job.Schedule(numAsteroids, 64);
    }

    void LateUpdate() {
        if (asteroids.Count <= 0)
            return;

        jobHandle.Complete();

        for (var i = 0; i < asteroids.Count; i++) {
            asteroids[i].transform.position = positions[i];
            asteroids[i].transform.rotation = rotations[i];
        }

        velocity.Dispose();
        angularVelocity.Dispose();
        positions.Dispose();
        rotations.Dispose();
    }
}

[BurstCompile]
public struct AsteroidMovementJob : IJobParallelFor {
    readonly NativeArray<Vector3> velocity;
    NativeArray<Vector3> position;

    readonly NativeArray<Vector3> angularVelocity;
    NativeArray<Quaternion> quaternions;

    readonly MinMax zBound;
    readonly float deltaTime;

    public AsteroidMovementJob(NativeArray<Vector3> velocity,
                               NativeArray<Vector3> position,
                               NativeArray<Vector3> angularVelocity,
                               NativeArray<Quaternion> quaternions,
                               MinMax zBound,
                               float deltaTime) {
        this.velocity = velocity;
        this.position = position;
        this.angularVelocity = angularVelocity;
        this.quaternions = quaternions;
        this.zBound = zBound;
        this.deltaTime = deltaTime;
    }

    public void Execute(int i) {
        position[i] += velocity[i] * deltaTime;

        if (position[i].z < zBound.min)
            position[i] = new Vector3(position[i].x, position[i].y, zBound.max);

        quaternions[i] *= Quaternion.Euler(angularVelocity[i] * deltaTime);
    }
}

}

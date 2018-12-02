using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Ecs {

public class AsteroidEcsManager : MonoBehaviour {
    [SerializeField] GameObject[] asteroidTemplates;
    [SerializeField] int numSpawning;

    int numSpawned;
    Globals globals;
    DebugCanvas debugCanvas;
    EntityManager entityManager;

    void Start() {
        globals = Globals.Instance;
        debugCanvas = FindObjectOfType<DebugCanvas>();
        entityManager = World.Active.GetOrCreateManager<EntityManager>();
    }

    void SpawnAsteroids() {
        var entities = new NativeArray<Entity>(numSpawning, Allocator.Temp);

        entityManager.Instantiate(asteroidTemplates[UnityEngine.Random.Range(0, asteroidTemplates.Length)], entities);

        for (var i = 0; i < numSpawning; i++) {
            entityManager.SetComponentData(entities[i], new Position {
                Value = new float3(UnityEngine.Random.Range(globals.xBound.min, globals.xBound.max),
                                   0, globals.zBound.max)
            });

            entityManager.SetComponentData(entities[i], new Rotation { Value = new quaternion(0, 1, 0, 0) });

            entityManager.SetComponentData(entities[i], new AsteroidSettings {
                speed = new Vector3(0, 0,
                                    UnityEngine.Random.Range(globals.asteroidSpeed.min, globals.asteroidSpeed.max)),
                angularSpeed = UnityEngine.Random.insideUnitSphere *
                               UnityEngine.Random.Range(globals.asteroidAngularSpeed.min,
                                                        globals.asteroidAngularSpeed.max)
            });
        }

        entities.Dispose();

        numSpawned += numSpawning;
        debugCanvas.SetNumAsteroids(numSpawned);
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.K))
            SpawnAsteroids();
    }
}

}

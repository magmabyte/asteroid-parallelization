using UnityEngine;
using UnityEngine.UI;

namespace Asteroids.Classic {

public class ClassicAsteroidSpawner : MonoBehaviour {
	[SerializeField] GameObject[] hazards;
	[SerializeField] int numSpawning;
	DebugCanvas debugCanvas;

	int numSpawned;

	void Start() => debugCanvas = FindObjectOfType<DebugCanvas>();

	void Update () {
		if (Input.GetKeyDown(KeyCode.K))
			SpawnAsteroids();
	}

	void SpawnAsteroids() {
		for (var i = 0; i < numSpawning; i++)
			Instantiate(hazards[Random.Range(0, hazards.Length)], transform);

		numSpawned += numSpawning;
		debugCanvas.SetNumAsteroids(numSpawned);
	}
}

}

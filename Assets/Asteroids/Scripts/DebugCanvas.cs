using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Asteroids {

public class DebugCanvas : MonoBehaviour {
	[SerializeField] Text asteroidText;
	[SerializeField] Text fpsText;

	void Update() => fpsText.text = $"FPS: {(int)(1.0f / Time.smoothDeltaTime)}";

	public void SetNumAsteroids(int num) => asteroidText.text = $"Num Asteroids: {num}";
}

}

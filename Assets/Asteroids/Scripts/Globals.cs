using UnityEngine;

namespace Asteroids {

public class Globals : MonoBehaviour {
	public static Globals Instance { get; private set; }
	public MinMax asteroidSpeed;
	public MinMax asteroidAngularSpeed;
	public MinMax xBound;
	public MinMax zBound;

	void Awake() {
		if (Instance)
			DestroyImmediate(this);
		else
			Instance = this;
	}
}

}

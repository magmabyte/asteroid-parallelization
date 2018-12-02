using UnityEngine;

namespace Asteroids {

public abstract class AbstractAsteroid : MonoBehaviour {
    public Vector3 speed;
    public Vector3 angularSpeed;

    void Awake() {
        var globals = Globals.Instance;

        transform.position = new Vector3(Random.Range(globals.xBound.min, globals.xBound.max), 0, globals.zBound.max);
        speed = new Vector3(0, 0,
                            Random.Range(globals.asteroidSpeed.min, globals.asteroidSpeed.max));
        angularSpeed = Random.insideUnitSphere *
                       Random.Range(globals.asteroidAngularSpeed.min, globals.asteroidAngularSpeed.max);
    }
}

}

using UnityEngine;

namespace Asteroids.Classic {
	
public class ClassicAsteroidMovement : AbstractAsteroid {
	void Update () {
		var position = transform.position;
		position += speed * Time.deltaTime;

		if (position.z < Globals.Instance.zBound.min)
			position.z = Globals.Instance.zBound.max;

		transform.position = position;

		transform.rotation *= Quaternion.Euler(angularSpeed * Time.deltaTime);
	}
}

}

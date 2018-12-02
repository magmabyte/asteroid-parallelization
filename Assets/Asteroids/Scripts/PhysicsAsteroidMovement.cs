using UnityEngine;

namespace Asteroids.Physics {

public class PhysicsAsteroidMovement : AbstractAsteroid {
	void Start () {
		var rigidbody = GetComponent<Rigidbody>();
		rigidbody.velocity = speed;
		rigidbody.angularVelocity = angularSpeed;
	}

	void FixedUpdate() {
		var position = transform.position;
		if (position.z < Globals.Instance.zBound.min)
			position.z = Globals.Instance.zBound.max;
		transform.position = position;
	}
}

}

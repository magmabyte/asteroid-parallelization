using System;
using Unity.Entities;
using Unity.Mathematics;

namespace Asteroids.Ecs {

[Serializable]
public struct AsteroidSettings : IComponentData {
	public float3 speed;
	public float3 angularSpeed;
}

public class AsteroidComponent : ComponentDataWrapper<AsteroidSettings> {}

}

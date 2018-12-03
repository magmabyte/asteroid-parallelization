# Asteroid Parallelization

The Unity project compares different ways of parallizing moving and rotating asteroids. The idea is based on [this Unity tutorial](https://unity3d.com/de/learn/tutorials/topics/scripting/introduction-ecs) which I could not find sources for.

### Installation

Just clone this repository and open it with Unity. The scenes are located under Assets > Asteroids > Scripts.

### Implementations

It contains the following implementations:

* Classic Asteroids: Asteroids have a script attached that modifies the transform in the Update statement. Based on the code in [this video](https://unity3d.com/de/learn/tutorials/topics/scripting/implementing-job-system?playlist=17117).
* Physics Asteroids: Asteroids have a rigidbody component with gravity scale 0. A scripts sets an initial velocity and angular velocity. Based on the code of [the Space Shooter code](https://assetstore.unity.com/packages/essentials/tutorial-projects/space-shooter-tutorial-13866?_ga=2.176785957.146512240.1543442913-951448717.1534938922) that I based my work on.
* Job system Asteroids: The same as Classic Asteroids but with the code being moved into a job using `IJobParallelFor`. Based on the code in [the script reference](https://docs.unity3d.com/ScriptReference/Unity.Jobs.IJobParallelFor.html) for the interface.
* Lean Job system Asteroids: An implementation based on `IJobParallelForTransform` which I originally thought would be leaner and it maybe is a tiny bit. Based on [this tutorial](https://unity3d.com/de/learn/tutorials/topics/scripting/implementing-job-system?playlist=17117).
* ECS Asteroids: An implementation based on Unity's new ECS. It is based on the code in [this video](https://unity3d.com/de/learn/tutorials/topics/scripting/implementing-ecs?playlist=17117).

### Results

The Unity tutorial compared the number of objects that can be spawned until the framerate drops to 30 (using `Time.smoothDeltaTime` or `Time.deltaTime` as far as I could tell) so I did the same.

I personally found the results rather disappointing on my *MacBook Pro (Retina, 13-inch, Early 2015)* to the point where I am wondering whether I did something wrong. Apart from the Physics implementation I could only observe marginal improvements between the different implementations. 


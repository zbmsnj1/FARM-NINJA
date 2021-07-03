# FARM NINJA


**A simple game (scene) developed based on Unity3D, controls farmers to drive away wolves who want to eat the flock. Sheep and wolves have their own FSM state machine, and are associated with the animation in Unity. If there are sheep in the range, the wolf will use the A star pathfinding algorithm to find the sheep and eat it. The movement of the sheep has a random and flock cluster mode, which can escape the chase of the wolf or be driven by the farmer.**


![image](https://github.com/zbmsnj1/FARM-NINJA/blob/master/screenshot/1.png)


### FSM of Sheep
![image](https://github.com/zbmsnj1/FARM-NINJA/blob/master/screenshot/2.png)

### Animation in Unity3D
![image](https://github.com/zbmsnj1/FARM-NINJA/blob/master/screenshot/4.png)


### FSM of Wolf
![image](https://github.com/zbmsnj1/FARM-NINJA/blob/master/screenshot/3.png)



**Using the same model, we also built a deep reinforcement learning scenario, where the sheep walk randomly, the farmers follow a fixed route to patrol, and the wolf learns to avoid the farmers and eat more sheep.**


![image](https://github.com/zbmsnj1/FARM-NINJA/blob/master/screenshot/6.png)

### Observation data 

Black ray: 
      to detect the environment
      and collect observation data
       (the distance from wolf to 
	ray hit point position)
Tag:
      using tag to mark ray-hit
      gameobject (sheep, boss, fence)


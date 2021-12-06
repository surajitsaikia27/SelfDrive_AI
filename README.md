# SelfDrive_AI

Reinforcement learning for self-driving in  a 3D simulation

<img src="https://github.com/surajitsaikia27/SelfDrive_AI/blob/master/Scenes/sample2.png"/>

<center>![Glimpse from the simulation environment](https://github.com/surajitsaikia27/SelfDrive_AI/blob/master/Scenes/cargif.gif)


###  1. Requirements for the SelfDrive_AI Gym
------------

You need Python 3.6 or later to run the simulation. (Note: the current environment is only supported in windows) Also, you can directly interact with the simulation by clicking the exe file and then by using W,A, S and D keys.


The image below illustrates the target goal of the AIcar, where the car needs to explore all the trajectories to find the bridge first.

<img src="https://github.com/surajitsaikia27/SelfDrive_AI/blob/master/Scenes/goalsample.png"/>

[Install Unity-Gym](https://github.com/Unity-Technologies/ml-agents/tree/main/gym-unity)
[Install Stable-Baselines3](https://stable-baselines3.readthedocs.io/en/master/guide/install.html)

mlagents can be installed using pip:

    $ python3 -m pip install mlagents
    
### 2. (Training) You can train the environment by using the code below which has OpenAI gym structure. It will save the training results into a log directory which you can view using tensorboard. Feel free to change the parameters inside the code

```python
from stable_baselines3 import PPO, SAC, ppo
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
channel = EngineConfigurationChannel()
from gym_unity.envs import UnityToGymWrapper
from mlagents_envs.environment import UnityEnvironment
import time,os
from stable_baselines3.common.vec_env import DummyVecEnv
from stable_baselines3.common.monitor import Monitor
from stable_baselines3.common.policies import ActorCriticPolicy
import math


env_name = "./UnityEnv"
speed = 15


env = UnityEnvironment(env_name,seed=1, side_channels=[channel])
channel.set_configuration_parameters(time_scale =speed)
env= UnityToGymWrapper(env, uint8_visual=False) # OpenAI gym interface created using UNITY

time_int = int(time.time())

# Diretories for storing results 
log_dir = "stable_results/Euler_env_3{}/".format(time_int)
log_dirTF = "stable_results/tensorflow_log_Euler3{}/".format(time_int) 
os.makedirs(log_dir, exist_ok=True)

env = Monitor(env, log_dir, allow_early_resets=True)
env = DummyVecEnv([lambda: env])  # The algorithms require a vectorized environment to run


model = PPO(ActorCriticPolicy, env, verbose=1, tensorboard_log=log_dirTF, device='cuda')
model.learn(int(200000)) # you can change the step size
time_int2 = int(time.time()) 
print('TIME TAKEN for training',time_int-time_int2)
# # save the model
model.save("Env_model")


# # # # # LOAD FOR TESTING
# del model
model = PPO.load("Env_model")

obs = env.reset()

# Test the agent for 1000 steps after training

for i in range(400):
    action, states = model.predict(obs)
    obs, rewards, done, info = env.step(action)
    env.render()





```

To monitor the training progress using tensorboard you type the following command from the terminal

    $ tensorboard --logdir "HERE PUT THE PATH TO THE DIRECTORY"
 
 If the training goes well you should be able to see the following graphs using tensorboard
 
 ![Screenshot](tfresults.png)
 
 
### 3. (Testing) The following code can be used to test the trained Humanoid Agent
```python
from stable_baselines3 import PPO, SAC, ppo
from mlagents_envs.side_channel.engine_configuration_channel import EngineConfigurationChannel
channel = EngineConfigurationChannel()
from gym_unity.envs import UnityToGymWrapper
from mlagents_envs.environment import UnityEnvironment
import time,os
from stable_baselines3.common.vec_env import DummyVecEnv
from stable_baselines3.common.monitor import Monitor
from stable_baselines3.common.policies import ActorCriticPolicy
import math


env_name = "./UnityEnv"
speed = 1


env = UnityEnvironment(env_name,seed=1, side_channels=[channel])
channel.set_configuration_parameters(time_scale =speed)
env= UnityToGymWrapper(env, uint8_visual=False) # OpenAI gym interface created using UNITY

time_int = int(time.time())

# Diretories for storing results
log_dir = "stable_results/Euler_env_3{}/".format(time_int)
log_dirTF = "stable_results/tensorflow_log_Euler3{}/".format(time_int)
os.makedirs(log_dir, exist_ok=True)

env = Monitor(env, log_dir, allow_early_resets=True)
env = DummyVecEnv([lambda: env])  # The algorithms require a vectorized environment to run


model = PPO.load("Env_model")

obs = env.reset()

# Test the agent for 1000 steps after training

for i in range(1000):
    action, states = model.predict(obs)
    obs, rewards, done, info = env.step(action)
    env.render()

```



![sample video of AI car](https://github.com/surajitsaikia27/Humanoid_Gym/blob/main/carGym_example.gif)


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




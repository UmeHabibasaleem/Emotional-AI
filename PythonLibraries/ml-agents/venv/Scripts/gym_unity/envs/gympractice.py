import matplotlib.pyplot as plt
import numpy as np
import sys

from gym_unity.envs import UnityEnv

print("Python version:")
print(sys.version)

# check Python version
if (sys.version_info[0] < 3):
    raise Exception("ERROR: ML-Agents Toolkit (v0.3 onwards) requires Python 3")

env_name ="C:/HinaProgramm/UnityProjects/Gympratice/RollerGymunity"  # Name of the Unity environment binary to launch
env = UnityEnv(environment_filename=env_name, worker_id=0, use_visual=False)
#env = UnityEnv(env_name)
# Examine environment parameters
print(str(env))

# Reset the environment
initial_observation = env.reset()

if len(env.observation_space.shape) == 1:
    # Examine the initial vector observation
    print("Agent state looks like: \n{}".format(initial_observation))
else:
    # Examine the initial visual observation
    print("Agent observations look like:")
    if env.observation_space.shape[2] == 3:
        plt.imshow(initial_observation[:,:,:])
    else:
        plt.imshow(initial_observation[:,:,0])

for episode in range(10):
    initial_observation = env.reset()
    done = False
    episode_rewards = 0
    while not done:
        observation, reward, done, info = env.step(env.action_space.sample())
        episode_rewards += reward
    print("Total reward this episode: {}".format(episode_rewards))
env.close()
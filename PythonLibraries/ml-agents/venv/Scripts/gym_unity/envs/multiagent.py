import os
os.environ["TF_CPP_MIN_LOG_LEVEL"]="3"
import random
from collections import deque

import gym
import numpy as np
import tensorflow
from gym.envs.tests.test_envs_semantics import episodes
from tensorflow.python import keras
from tensorflow.python.keras import Sequential
from tensorflow.python.keras.optimizers import Adam
#from tensorflow.python.keras._impl.keras.optimizers import Adam
from tensorflow.python.layers.core import Dense
import matplotlib.pyplot as plt
import sys
from gym_unity.envs import UnityEnv
multi_env_name = "D:/ml-agents-0.8.0/UnitySDK/A.exe"
multi_env = UnityEnv(multi_env_name, worker_id=1,
                     use_visual=False, multiagent=True)

# Examine environment parameters
print(str(multi_env))
# Reset the environment
initial_observations = multi_env.reset()

if len(multi_env.observation_space.shape) == 1:
    # Examine the initial vector observation
    print("Agent observations look like: \n{}".format(initial_observations[0]))
else:
    # Examine the initial visual observation
    print("Agent observations look like:")
    if multi_env.observation_space.shape[2] == 3:
        plt.imshow(initial_observations[0][:,:,:])
    else:
        plt.imshow(initial_observations[0][:,:,0])


for episode in range(10):
    initial_observation = multi_env.reset()
    done = False
    episode_rewards = 0
    while not done:
        actions = [multi_env.action_space.sample() for agent in range(multi_env.number_agents)]
        observations, rewards, dones, info = multi_env.step(actions)
        episode_rewards += np.mean(rewards)
        print("________Reward_______")
        print(episode_rewards)
        done = dones[0]
    print("Total reward this episode: {}".format(episode_rewards))
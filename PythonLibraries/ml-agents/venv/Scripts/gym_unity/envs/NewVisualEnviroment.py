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


class DQNAgent:
    def __init__(self, state_size, action_size):
        self.state_size = int(state_size)
        self.action_size = action_size
        self.memory = deque(maxlen=2000)
        print(self.memory)
        self.gamma = 0.95    # discount rate
        self.epsilon = 1.0  # exploration rate
        self.epsilon_min = 0.01
        self.epsilon_decay = 0.995
        self.learning_rate = 0.001
        self.model = self._build_model()
        self.Cumulative_reward = 0
        self.last_reward=0

    def _build_model(self):
        # Neural Net for Deep-Q learning Model
        model = Sequential()
        print (self.state_size)
        print (self.action_size)
        print (keras.__version__)
        model.add(Dense(24, input_shape=(self.state_size,), activation=tensorflow.nn.relu))
        model.add(Dense(24, activation=tensorflow.nn.relu))
        model.add(Dense(self.action_size,activation=tensorflow.nn.softmax))
        model.compile(loss='mse',
                      optimizer=Adam(lr=self.learning_rate))
        return model
    def remember(self, state, action, reward, next_state, done):
        self.memory.append((state, action, reward, next_state, done))
        print("__________Memory________")
        print(self.memory)

    def act(self, state):
        if np.random.rand() <= self.epsilon:
            return random.randrange(self.action_size)
        act_values = self.model.predict(state)
        return np.argmax(act_values[0])  # returns action
    def replay(self, batch_size):
        minibatch = random.sample(self.memory, batch_size)
        for state, action, reward, next_state, done in minibatch:
            target = reward
            if not done:
              target = reward + self.gamma * \
                       np.amax(self.model.predict(next_state)[0])
            target_f = self.model.predict(state)
            target_f[0][action] = target
            self.model.fit(state, target_f, epochs=1, verbose=0)
        if self.epsilon > self.epsilon_min:
            self.epsilon *= self.epsilon_decay

if __name__ == "__main__":
    # initialize gym environment and the agent
    #env = gym.make('CartPole-v0')
    env_name = "C:/HinaProgramm/Scenes/ml-agents-0.8.0/UnitySDK/A"  # Name of the Unity environment binary to launch
    env = UnityEnv(environment_filename=env_name, worker_id=0, use_visual=False, multiagent = True)
    print(str(env))
    state_size = env.observation_space.shape[0]
    print("______statesize__________________---")
    print(state_size)
    action_size = env.action_space.n
    print (env.number_agents)
    print("______actionsize__________________---")
    print(action_size)
    #action_size = 2
    #agent = DQNAgent(state_size,  action_size )
    agent = DQNAgent(state_size,  action_size )
    # Iterate the game
    for e in range(episodes):
        # reset state in the beginning of each game
        state = env.reset()
        print("Sate")
        # time_t represents each frame of the game
        # Our goal is to keep the pole upright as long as possible until score of 500
        # the more time_t the more score
        for time_t in range(500):
            # turn this on if you want to render
            # env.render()
            # Decide action\
            #print("_______________time_T_________________")
            #print(time_t)
            #print("______________episode Number______________")
            #print(e)
            actionlst = []
            action = agent.act(state)
            #action2 = agent2.act(state)
            print("__Randomly Selected Action__________")
            #print(action1)
            print(action)
            #actionlst.append(action1)
            actionlst.append(action)


            # Advance the game to the next frame based on the action.
            # Reward is 1 for every frame the pole survived
            print ("______TIME")
            print (time_t)
            next_state, reward, done, _ = env.step(actionlst)

            print("_____________reward_____________")
            print(reward)
            print("____________nextstaet_____________")
            print(next_state)
            #next_state = np.reshape(next_state, [1, 42336])
            # Remember the previous state, action, reward, and done
            agent.remember(state, actionlst, reward, next_state, done)
            print(last_reward)
            last_reward=reward

            # make next_state the new current state for the next frame.
            state = next_state
            # done becomes True when the game ends
            # ex) The agent drops the pole
            if done:
                # print the score and break out of the loop
                print("________This is time step______________")
                print(time_t)
                print("episode: {}/{}, score: {}"
                      .format(e, episodes, time_t))
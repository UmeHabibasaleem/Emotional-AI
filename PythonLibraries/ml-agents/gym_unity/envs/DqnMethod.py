from __future__ import absolute_import, division, print_function, unicode_literals
import os
os.environ["TF_CPP_MIN_LOG_LEVEL"]="3"
import sys
sys.path.append(".")
import random
from collections import deque
import gym
import tensorflow as tf
from gym.envs.tests.test_envs_semantics import episodes
from tensorflow import keras
from tensorflow.python.keras.models import Sequential
from tensorflow.python.keras.optimizers import Adam
#from tensorflow.python.keras._impl.keras.optimizers import Adam
from tensorflow.python.keras.layers import Dense
import matplotlib.pyplot as plt
from tensorflow.python.keras.models import load_model
import sys
import h5py
from gym_unity.envs import UnityEnv
#from tensorflow.python.keras import numpy as np
import numpy as np
from tensorflow.python.saved_model import builder as pb_builder
print(tf.VERSION)
print(tf.keras.__version__)

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
        self.model =  self.load('my_model2.h5')
        self.model.summary()
        self.last_reward=0
    def _build_model(self):
        # Neural Net for Deep-Q learning Model
        model = Sequential()
        print (self.state_size)
        print (self.action_size)
        print (keras.__version__)
        model.add(Dense(24, input_shape=(self.state_size,), activation='relu'))
        model.add(Dense(24, activation='relu'))
        model.add(Dense(self.action_size,activation='linear'))
        model.compile(loss='mse',
                      optimizer=Adam(lr=self.learning_rate))
        return model
    def remember(self, state, action, reward, next_state, done):
        self.memory.append((state, action, reward, next_state, done))
        #print("__________Memory________")
        #print(self.memory)

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
    def load(self , name):
        #self.model.load_weights(name)
        newmodel = tf.keras.models.load_model(name)
        print("__________New model____________")
        print(newmodel)
        return newmodel
        #newmodel.summary()
    def save(self, name):
        self.model.save(name)
        #newmodel.summary()
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
    agent1 = DQNAgent(state_size,  action_size )
    agent2 = DQNAgent(state_size,  action_size )
    # Iterate the game
    for e in range(episodes):
        # reset state in the beginning of each game
        state = env.reset()
        print("___________State_______________")
        state1 = state[0]
        state2 = state[1]

        # time_t represents each frame of the game
        # Our goal is to keep the pole upright as long as possible until score of 500
        # the more time_t the more score
        for time_t in range(100):
            # turn this on if you want to render
            # env.render()
            # Decide action\
            #print("_______________time_T_________________")
            #print(time_t)
            #print("______________episode Number______________")
            #print(e)\\\\\
            actionlst = []
            action1 = agent1.act(state1)
            action2 = agent2.act(state2)
            print("__Randomly Selected Action__________")
            print(action1)
            print(action2)
            actionlst.append(action1)
            actionlst.append(action2)


            # Advance the game to the next frame based on the action.
            # Reward is 1 for every frame the pole survived
            print ("______TIME")
            print (time_t)
            next_state, reward, done, _ = env.step(actionlst)

            print("_____________reward_____________")
            print(reward)
            print("____________nextstaet agen 1_____________")
            print(next_state[0])
            print ("_____________next state agent 2_______")
            print(next_state[1])
            #next_state = np.reshape(next_state, [1, 42336])
            # Remember the previous state, action, reward, and done
            agent1.remember(state1, action1, reward[0], next_state[0], done[0])
            agent2.remember(state2, action2, reward[1], next_state[1], done[1])


            # make next_state the new current state for the next frame.
            state1 = next_state[0]
            state2 = next_state[1]
            # done becomes True when the game ends
            # ex) The agent drops the pole
            if done:
                # print the score and break out of the loop
                print("________This is time step______________")
                print(time_t)
                print("episode: {}/{}, score: {}"
                      .format(e, episodes, time_t))
            '''if e == 10:
                #agent1.load()
                #tf.keras.backend.set_learning_phase(0)

                newmodel= agent1.save('my_model2.h5')
                print("____Model has been saved_______________")
                print(newmodel)

                tf.keras.backend.set_learning_phase(0)
                newmodel1 = agent1.load('my_model2.h5')
                print("____Model has been loaded________")
                print(newmodel1)
                builder = pb_builder.SavedModelBuilder('C:/HinaProgramm/models/MyBuiltModels')
                print("It  is built first time")
                builder.save()'''
            '''agent2.save('mymodel3.h5')
                builder = pb_builder.SavedModelBuilder('C:/HinaProgramm/models/MySecondBuiltModels')
                print("It  is built Second time")
                builder.save()'''
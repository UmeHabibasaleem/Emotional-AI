from __future__ import absolute_import, division, print_function, unicode_literals
import sys
sys.path.append(".")
import random
import copy
import gym
import numpy as np
import os
os.environ["TF_CPP_MIN_LOG_LEVEL"]="3"
import random
from collections import deque
import gym
#import tensorflow
#tf.__file__
from gym.envs.tests.test_envs_semantics import episodes
#from tensorflow import keras
import keras
from keras.models import Sequential
from keras.optimizers import Adam
#from tensorflow.python.keras._impl.keras.optimizers import Adam
from keras.layers import Dense
import matplotlib.pyplot as plt
from tensorflow.python.keras.models import load_model
import sys
import h5py
from gym_unity.envs import UnityEnv
#from tensorflow.python.keras import numpy as np
import numpy as np
from tensorflow.python.saved_model import builder as pb_builder

from scores.score_logger import ScoreLogger
ENV_NAME = 'C:/HinaProgramm/Gridworld'
print(ENV_NAME)
GAMMA = 0.95
LEARNING_RATE = 0.001

MEMORY_SIZE = 1000000
BATCH_SIZE = 20

EXPLORATION_MAX = 1.0
EXPLORATION_MIN = 0.01
EXPLORATION_DECAY = 0.995


class DQNSolver:

    def __init__(self, observation_space, action_space):
        self.exploration_rate = EXPLORATION_MAX

        self.action_space = action_space
        self.memory = deque(maxlen=MEMORY_SIZE)

        self.model = Sequential()
        self.model.add(Dense(128, input_dim=observation_space , activation="relu"))
        self.model.add(Dense(128, activation="relu"))
        self.model.add(Dense(self.action_space, activation="linear"))
        self.model.compile(loss="mse", optimizer=Adam(lr=LEARNING_RATE))

    def set_model(self , name):
        newmodel = keras.models.load_model(name)
        self.model = newmodel

    def remember(self, state, action, reward, next_state, done):
        self.memory.append((state, action, reward, next_state, done))

    def act(self, state):
        if np.random.rand() < self.exploration_rate:
            return random.randrange(self.action_space)
        q_values = self.model.predict(state)
        return np.argmax(q_values[0])

    def experience_replay(self):
        if len(self.memory) < BATCH_SIZE:
            return
        batch = random.sample(self.memory, BATCH_SIZE)
        for state, action, reward, state_next, terminal in batch:
            q_update = reward
            if not terminal:
                q_update = (reward + GAMMA * np.amax(self.model.predict(state_next)[0]))
            q_values = self.model.predict(state)
            q_values[0][action] = q_update
            self.model.fit(state, q_values, verbose=0)
        self.exploration_rate *= EXPLORATION_DECAY
        self.exploration_rate = max(EXPLORATION_MIN, self.exploration_rate)

    def save(self, name):
        self.model.save(name)


def cartpole():
    env = UnityEnv(environment_filename=ENV_NAME, worker_id=2, use_visual=False, multiagent = True)
    score_logger = ScoreLogger(ENV_NAME)
    agents_brain = []
    agents_action = []
    index_list = []
    agents_alive = []
    count = 0
    count1 = 0
    num_agents = env.number_agents
    print("___________Number of agents in cartpole __")
    print(num_agents)
    observation_space = env.observation_space.shape[0]
    action_space = env.action_space.n
    dqn_solver = DQNSolver(observation_space, action_space)
    print("__dqn solver______")
    print(dqn_solver)
    #model = tf.keras.models.load_model("")
    for x in range ((env.number_agents)):
        agents_brain.append(dqn_solver)
        print("______agentbrain____")
        print(agents_brain)
        print("_Agent action___")
        print(agents_action)

    learning_brain = copy.deepcopy(agents_brain)
    run = 0
    state = env.reset()
    initialstate = copy.deepcopy(state)
    while True:
        run += 1
        env.reset()
        print("____________STATE____________-")
        print(state[0])
        state = copy.deepcopy(initialstate)
        agents_brain = []
        agents_action = []
        index_list = []
        agents_alive = []
        count = 0
        count1 = 0
        num_agents = int (state[0][-5])
        agents_brain = copy.deepcopy(learning_brain)
        print (learning_brain)
        print (agents_brain)
        print(state)
        #for x in range ( (env.number_agents - 1) ):

        step = 0
        while True:
            step += 1
            env.render()
            print ("___________STatte Lenth_______")
            print (len(state))
            print ("______selffish___")
            print(state[0])
            agents_action = [1] * len(state)
            copied_agents_alive = copy.deepcopy(agents_alive)
            print("__________numagents_____")
            for x in range ( num_agents - 1 ):
                state[x] = np.reshape(state[x], [1, observation_space])
                agents_action[x] = agents_brain[x].act(state[x])
            print(agents_action)
            state_next, reward, terminal, info = env.step(agents_action , num_agents)
            print ("_______Reward________")
            print (reward)
            print ("_____________NEXT STATE LENGTH____________")
            print (len(state_next))
            if (len(state_next) == 0):
                break
            agents_alive = state_next[0][-13:-5]
            num_agents = int(state_next[0][-5])
            print("_______num agnets in cartpole________")
            print(num_agents)
            print("_____index list")
            print(index_list)
            print(agents_alive)
            agents_alive1 = np.delete(agents_alive, index_list)
            print("_______Alive agent list_______")
            print(agents_alive1)
            flag = False
            # del agents_alive[index_list[x]]
            for x in range(len(agents_alive)):
                if (agents_alive[x] == float(1)):
                    for y in range(len(index_list)):
                        if (index_list[y] == x):
                            flag = True
                    if (flag == False):
                        index_list.append(x)

                flag = False

            index_to_remove = []
            for x in range(len(agents_alive1)):
                if (agents_alive1[x] == float(1)):
                    learning_brain[index_list[count]] = agents_brain[x]
                    index_to_remove.append(x)
                    count = count + 1

            agents_brain = [i for j, i in enumerate(agents_brain) if j not in index_to_remove]
            print("____________AGENTS_BRAIN_________")
            print(len(agents_brain))
            print("_______________Terminal_____________")
            print(terminal)
            if (terminal[0] == True):
                print ("Run: " + str(run) + ", exploration: " + str(dqn_solver.exploration_rate) + ", score: " + str(step))
                score_logger.add_score(step, run)
                for x in range(len(copied_agents_alive)):
                        learning_brain[x] = agents_brain[count1]
                        count1 = count1 + 1
                for x in range (len(learning_brain)):
                    learning_brain[x].save( str(run) + "brain" + str(x) + ".h5")

                break

            for x in range ( num_agents - 1 ):
                state[x] = np.reshape(state[x], [1, observation_space])
                state_next[x] = np.reshape(state_next[x], [1, observation_space])
                agents_brain[x].remember(state[x],agents_action[x],reward[x],state_next[x],terminal[x])
                agents_brain[x].experience_replay()
            state = state_next




if __name__ == "__main__":
    cartpole()

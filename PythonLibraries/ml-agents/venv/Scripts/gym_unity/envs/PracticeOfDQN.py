import random
import copy
import gym
import numpy as np
from collections import deque
import keras
from keras.models import Sequential
from keras.layers import Dense
import tensorflow as tf
from keras.optimizers import Adam
import h5py
import sys
#sys.path.insert(0, '/home/maida/Downloads/Thesis/Unity3D/uni/ml-agents/gym-unity')
from gym_unity.envs import UnityEnv

from scores.score_logger import ScoreLogger
ENV_NAME = 'C:/HinaProgramm/BuildForGridworld/Gridworld'
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
        self.model.add(Dense(128, input_dim=observation_space, activation="relu"))
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
    env = UnityEnv(environment_filename=ENV_NAME, worker_id=5, use_visual=False, multiagent = True)
    score_logger = ScoreLogger(ENV_NAME)
    agents_brain = []
    agents_action = []

    num_agents = env.number_agents
    observation_space = env.observation_space.shape[0]
    print("____________Observation_space")
    print(observation_space)
    action_space = env.action_space.n
    dqn_solver = DQNSolver(observation_space, action_space)
    for x in range ((env.number_agents)):
        agents_brain.append(DQNSolver(observation_space, action_space))
    print ("Length of BrainList:    ",len(agents_brain))
    run = 0
    state = env.reset()
    print("______INITIAL______")
    print(state)
    #initialstate = copy.deepcopy(state)
    print("*****************************initial state for unity  envirmonet**************")
    #print(initialstate)
    jk = 1
    while True:
        run += 1
        state = env.reset()
        #state = copy.deepcopy(initialstate)
        num_agents = int(state[0][-5])
        print("_____________State _______________")
        print(int(state[0][12]))
        step = 0

        print("################################This is loop################################# :" , jk)
        while True:
            step += 1
            env.render()
            agents_action = [1] * len(state)
            print(state[0])
            print("*******************Length of state******************")
            print(len(state))
            for x in range(len(state)):
                state[x] = np.reshape(state[x], [1, observation_space])
                agents_action[x] = agents_brain[int(state[x][0,12]) - 1].act(state[x])
            print("Agents Actions List: ",agents_action)
            state_next, reward, terminal, info = env.step(agents_action)
            #print ("_____________STATE_NEXT___________")
            #print (state_next)
            if (len(state_next) == 0):
                break
            agents_alive = state_next[0][-13:-5]
            print ("Agents_alive:    ", agents_alive)
            print ("Rewards:    ",reward)
            num_agents = int(state_next[0][-5])
            print ("Number of agents:   ",num_agents)
            print("_________Terminal list_______" , terminal)
            if (terminal[0] == True):
                print("**************************Brain saved******************************")
                for x in range(len(agents_brain)):
                    agents_brain[x].save(str(run) + "brain" + str(x) + ".h5")

                jk+=1
                print("#####################################Loop is######################## :" , jk)
                #break

            for x in range(len(state_next)):
                state[x] = np.reshape(state[x], [1, observation_space])
                state_next[x] = np.reshape(state_next[x], [1, observation_space])
                agents_brain[int(state_next[x][0,12]) - 1].remember(state[x], agents_action[x], reward[x], state_next[x], terminal[x])
                agents_brain[int(state_next[x][0,12]) - 1].experience_replay()
            state = state_next
            #del agents_action

if __name__ == "__main__":
    cartpole()
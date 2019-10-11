import random
#import copy
import warnings
warnings.filterwarnings('ignore')
#import gym
import numpy as np
import array
from collections import deque
'''import keras
from keras.models import Sequential
from keras.layers import Dense
import tensorflow as tf
from keras.optimizers import Adam'''
#import tensorflow as tf
#from gym.envs.tests.test_envs_semantics import episodes
import keras
from keras.models import Sequential
from keras.optimizers import Adam
#from tensorflow.python.keras.models import Sequential
#from tensorflow.python.keras.optimizers import Adam
#from tensorflow.python.keras._impl.keras.optimizers import Adam
#from tensorflow.python.keras.layers import Dense
from keras.layers import Dense
#import h5py
import sys
#sys.path.insert(0, '/home/maida/Downloads/Thesis/Unity3D/uni/ml-agents/gym-unity')
#from gym_unity.envs import UnityEnv

#from scores.score_logger import ScoreLogger
#ENV_NAME ='C:/HinaProgramm/testingFolder/Unity Environment'
#print(ENV_NAME)
GAMMA = 0.90
LEARNING_RATE = 0.0001

MEMORY_SIZE = 100000
BATCH_SIZE = 32

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
        #self.model.add(Dense(64, activation = "relu"))
        self.model.add(Dense(self.action_space, activation="linear"))
        self.model.compile(loss="mse",optimizer=Adam(lr=LEARNING_RATE))

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
    #env = UnityEnv(environment_filename=ENV_NAME, worker_id=1, use_visual=False, multiagent = True)
    #score_logger = ScoreLogger(ENV_NAME)
    agents_brain = []
    agents_action = []
    #pathname = "C:/HinaProgramm/testingFolder/Unity Environment"
    #num_agents = env.number_agents
    #print("Number of agents in enviroment : " , num_agents)
    observation_space= int(sys.argv[1])
    #print("____________Observation_space______________")
    #print("__________Action Space________________")
    action_space = int(sys.argv[2])
        #agents_brain.append(DQNSolver(observation_space, action_space))
    #print ("Length of BrainList:    ",len(agents_brain))
    #print(action_space)
    dqn_solver = DQNSolver(observation_space, action_space)
    for x in range (2):
        agents_brain.append(DQNSolver(observation_space, action_space))
    run = 0
    observ = array.array('f' , [float(sys.argv[3]) , float(sys.argv[4]) , float(sys.argv[5]) , float(sys.argv[6]) , float(sys.argv[7]) , float(sys.argv[8])])
    state = observ
    '''for x in state:
        print("observation is ", x)'''
    # print("______INITIAL______")
    # print(state)
    #initialstate = state
    # print("*****************************initial state for unity  envirmonet**************")
    # print(initialstate)
    numagents = 2
    run = 0
    for x in range(2):
        run += 1
        step = 0
        #print("_____Run _______ :", run)
        for y in range(1):
            #print("************Number of agents *********")
            #print(env.number_agents)
            step += 1
            #env.render()
            agents_action =[1] * numagents
            #print(state[0])
            #print("*******************Length of state******************")
            #print(len(state))
            for x in range(2):
                agents_action[x] = agents_brain[x].act(state)


            #sharecount += agents_action.count(5)
            #eatcount += agents_action.count(6)
            print(agents_action)
            #return agents_action




if __name__ == "__main__":
    cartpole()
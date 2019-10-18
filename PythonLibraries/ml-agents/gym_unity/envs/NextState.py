import random
# import copy
import warnings
warnings.filterwarnings('ignore')
# import gym
import numpy as np
import array
from collections import deque

'''import keras
from keras.models import Sequential
from keras.layers import Dense
import tensorflow as tf
from keras.optimizers import Adam'''
# import tensorflow as tf
# from gym.envs.tests.test_envs_semantics import episodes
import keras
from keras.models import Sequential
from keras.optimizers import Adam
# from tensorflow.python.keras.models import Sequential
# from tensorflow.python.keras.optimizers import Adam
# from tensorflow.python.keras._impl.keras.optimizers import Adam
# from tensorflow.python.keras.layers import Dense
from keras.layers import Dense
# import h5py
import sys
def State():
    nextState  = []
    for i in range(1, 20):
        nextState.append(float(sys.argv[i]))
    reward1 = sys.argv[20]
    print("Reward is ", reward1)
    return nextState, reward1

if __name__ == "__main__":
    State()
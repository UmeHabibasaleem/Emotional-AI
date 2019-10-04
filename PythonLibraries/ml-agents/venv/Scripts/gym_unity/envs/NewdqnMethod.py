import random
import gym
import numpy as np
from collections import deque
import matplotlib.pyplot as plt
from gym_unity.envs import UnityEnv
from tensorflow.contrib.factorization.examples import mnist
from tensorflow.python.keras.models import Sequential
from tensorflow.python.keras.layers import Dense , Dropout, Flatten
from tensorflow.python.keras.optimizers import Adam
from tensorflow.python.keras import backend as K
from tensorflow.python.keras.layers import Conv2D, MaxPooling2D
import tensorflow as tf
EPISODES = 100

class DQNAgent:
    def __init__(self, state_size, action_size):
        self.state_size = state_size
        self.action_size = action_size
        self.memory = deque(maxlen=2000)
        self.gamma = 0.95    # discount rate
        self.epsilon = 1.0  # exploration rate
        self.epsilon_min = 0.01
        self.epsilon_decay = 0.99
        self.learning_rate = 0.001
        self.model = self._build_model()
        self.target_model = self._build_model()
        self.update_target_model()

    """Huber loss for Q Learning
    References: https://en.wikipedia.org/wiki/Huber_loss
                https://www.tensorflow.org/api_docs/python/tf/losses/huber_loss
    """

    def _huber_loss(self, y_true, y_pred, clip_delta=1.0):
        error = y_true - y_pred
        cond  = K.abs(error) <= clip_delta

        squared_loss = 0.5 * K.square(error)
        quadratic_loss = 0.5 * K.square(clip_delta) + clip_delta * (K.abs(error) - clip_delta)

        return K.mean(tf.where(cond, squared_loss, quadratic_loss))

    def _build_model(self):
        # Neural Net for Deep-Q learning Model
        model = Sequential()
        '''model.add(Conv2D(32, kernel_size=(3, 3),
                         activation='relu',
                         input_shape=(84, 84, 3)))
        model.add(Conv2D(32, kernel_size=(3, 3),
                         activation='relu',
                         input_shape=(84, 84, 3)))
        model.add(Flatten())
        model.add(Dense(10, activation='softmax'))
        model.compile(loss=tf.keras.losses.categorical_crossentropy,
                      optimizer=tf.keras.optimizers.Adadelta(),
                      metrics=['accuracy'])


        #x_train, y_train), (x_test, y_test) = mnist.load_data()'''

        model.add(Dense(24, input_shape=(self.state_size,self.state_size,), activation='relu'))
        model.add(Dense(24, activation='relu'))
        model.add(Dense(self.action_size, activation='linear'))
        model.compile(loss=self._huber_loss, optimizer=Adam(lr=self.learning_rate))
        return model

    def update_target_model(self):
        # copy weights from model to target_model
        self.target_model.set_weights(self.model.get_weights())

    def remember(self, state, action, reward, next_state, done):
        self.memory.append((state, action, reward, next_state, done))

    def act(self, state):
        if np.random.rand() <= self.epsilon:
            return random.randrange(self.action_size)
        act_values = self.model.predict(state)
        return np.argmax(act_values[0])  # returns action

    def replay(self, batch_size):
        minibatch = random.sample(self.memory, batch_size)
        print("_____________minin batch______________")
        print(minibatch)
        for state, action, reward, next_state, done in minibatch:
            target = self.model.predict(state)
            print("_____________target_________________")
            print(target)
            if done:
                target[0][action] = reward
            else:
                #a = self.model.predict(next_state)[0]
                #print("_____________value of a_________")
                #print(a)
                print("_____value of t_____")

                t = self.target_model.predict(next_state)[0]
                print(t)
                #print("____________target 0 action_____________")
                #print(target[0][action])
                print("____________After taget 0  action______")
                target[0][action] = reward + self.gamma * np.amax(t)
                #print(target[0][action])
                #target[0][action] = reward + self.gamma * t[np.argmax(a)]
            self.model.fit(state, target, epochs=1, verbose=0)
        if self.epsilon > self.epsilon_min:
            self.epsilon *= self.epsilon_decay

    def load(self, name):
        self.model.load_weights(name)

    def save(self, name):
        self.model.save_weights(name)


if __name__ == "__main__":
    env_name = "C:/HinaProgramm/Scenes/GridWorld"  # Name of the Unity environment binary to launch
    env = UnityEnv(environment_filename=env_name, worker_id=0, use_visual=False, multiagent=True)
    print(str(env))
    state_size =env.observation_space.shape[0]
    '''if (env.observation_space.shape[2]==3):
        plt.imshow(env.observation_space[0, :, :, :])'''
    action_size = env.action_space.n
    agent = DQNAgent(state_size, action_size)
    # agent.load("./save/cartpole-ddqn.h5")//
    done = False
    batch_size = 332

    for e in range(EPISODES):
        state = env.reset()
        #print (state.shape)
        state = np.reshape(state, [3, state_size, state_size])
        print("_____________state size______")
        print(state_size)
        for time in range(500):
            # env.render()

            #action = agent.act(state)
            actionlst = []
            i = 0
            while (i<32):
                action = agent.act(state)
                actionlst.append(action)
                i = i+1
            next_state, reward, done, _ = env.step(action)
            print(next_state)
            reward = reward if not done else -10
            print("_______________Reward_____")
            print(reward)
            next_state = np.reshape(next_state, [3, state_size, state_size])
            agent.remember(state, action, reward, next_state, done)
            state = next_state
            if done:
                agent.update_target_model()
                print("episode: {}/{}, score: {}, e: {:.2}"
                      .format(e, EPISODES, time, agent.epsilon))
                break
            if len(agent.memory) > batch_size:
                agent.replay(batch_size)
//using Accord.Math;
//using Accord.Statistics.Kernels;
//using KerasSharp;
//using KerasSharp.Activations;
//using KerasSharp.Initializers;
//using KerasSharp.Losses;
//using KerasSharp.Metrics;
//using KerasSharp.Models;
//using KerasSharp.Optimizers;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using NumSharp;
//using System.IO;

//namespace Tests.Learning
//{
//    class DQNClass
//    {
//        double GAMMA = 0.90;
//        double LEARNING_RATE = 0.0001;
//        int MEMORY_SIZE = 100000;
//        int BATCH_SIZE = 32;
//        double EXPLORATION_MAX = 1.0;
//        double EXPLORATION_MIN = 0.01;
//        double EXPLORATION_DECAY = 0.995;
//        double exploration_rate;
//        int action_space;
//        Queue<objectclass> memory = new Queue<objectclass>();
//        Sequential model = new Sequential(); 
//        public DQNClass(int observation_space, int action_space)
//        {
//            this.exploration_rate = EXPLORATION_MAX;
//            this.action_space = action_space;
//            model.Add(new Dense(128, input_dim: observation_space, activation: new ReLU()));
//            model.Add(new Dense(128, activation: new ReLU()));
//            model.Add(new Dense(this.action_space, activation: new KerasSharp.Activations.Sigmoid()));
//            model.Compile(loss: "mse", optimizer: new Adam(lr: LEARNING_RATE));
//        }


//        public void remember(Array state, int action, int reward, Array next_state, int done)
//        {
//            objectclass oc = new objectclass(state, action, reward, next_state, done);
//            this.memory.Enqueue(oc);
//        }

//        public double act(Array state)
//        {
//            Random random = new Random();
//            if (np.random.rand() < this.exploration_rate)
//            {
//                int actionindex = random.Next(0, this.action_space);
//                return actionindex;
//            }
//            Array[] pred = model.predict(state);
//            return np.argmax(pred[0]);
//        }

//    }
//    class objectclass
//    {
//        Array state;
//        int action;
//        int reward;
//        Array next_state;
//        int done;
//        public objectclass()
//        {

//        }
//        public objectclass(Array state, int action, int reward, Array next_state, int done)
//        {
//            this.state = state;
//            this.action = action;
//            this.reward = reward;
//            this.next_state = next_state;
//            this.done = done;
//        }
//    }
//}
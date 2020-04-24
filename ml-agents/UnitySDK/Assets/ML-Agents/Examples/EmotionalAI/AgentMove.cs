using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using NeuralNetwork.Helpers;
using NeuralNetwork.NetworkModels;
using MLAgents;
using System;
//2

public class AgentMove : Agent
{
    public static List<string[]> rowData = new List<string[]>();
    public Animator AnimZombie;
    public float Timepassed;
    public bool ateFromRes = false;
    public float Timecheck;
    public Lara Lara;
    public int idle, move, Eat, Share,Steal = 0;
    public GameObject Model;
    public int Agentid = 3;
    float speed = 3.0f;
    public float Food;
    public float Health;
    public int action;
    public int iteration;
    public int seconds = 0;
    public int count = 0;

    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;

    public SpriteRenderer FoodFiller;
    public SpriteRenderer HealthFiller;

    public GameObject Hallo;
    public float PrevFood = 5;
    public float Pfood;
    public bool isDead = false;
    public bool healthinc;
    public float dist;
    public MarkoScript Marko;

    public GameObject TopContainer;
    public GameObject BottomContainer;
    public GameObject gun;

    public GameObject AttackParticle;
    public GameObject ParticlesContainer;
    public BulletFire bulletfire;

    public float FoodZerotimeSec = 0;
    public int FoodZerotime = 0;
    public Vector3 AgentStartingPos;
    private GameAcademy academy;
    
    private static int _numInputParameters;
    private static int _numHiddenLayers;
    private static int[] _hiddenNeurons;
    private static int _numOutputParameters;
    private static NeuralNetwork.NetworkModels.Network _network;
    private static List<DataSet> _dataSets;
    double[] obser = new double[22];


    private void Awake()
    {
        this.AttackParticle.SetActive(false);
        Physics.IgnoreLayerCollision(11, 11);
        ImportNetwork();
    }


    // Use this for initialization
    public override void InitializeAgent()
    {
        isDead = false;
        Timepassed = 0;
        Food = 5;
        Health = 5;
        PrevFood = 5;
        count = 1;
        AnimZombie = GetComponent<Animator>();
        Food3.SetActive(false);
        bulletfire = new BulletFire();
        AgentStartingPos = this.transform.position;
        }


    public override void CollectObservations()
    {
        AddVectorObs(Agentid);//1

        AddVectorObs(Timepassed);//1
        AddVectorObs(Lara.transform.position);//3
        AddVectorObs(Marko.transform.position);//3
        AddVectorObs(this.transform.position);//3
        AddVectorObs(Food1.transform.position);//3
        AddVectorObs(Food2.transform.position);//3
        AddVectorObs(Food3.transform.position);//3
        
        AddVectorObs(Food);//1
        AddVectorObs(Health);//1
        AddVectorObs(speed);//1
        obser[0] = Timepassed;
        obser[1] = Lara.transform.position.x;
        obser[2] = Lara.transform.position.y;
        obser[3] = Lara.transform.position.z;
        obser[4] = Marko.transform.position.x;
        obser[5] = Marko.transform.position.y;
        obser[6] = Marko.transform.position.z;
        obser[7] = this.transform.position.x;
        obser[8] = this.transform.position.y;
        obser[9] = this.transform.position.z;
        obser[10] = Food1.transform.position.x;
        obser[11] = Food1.transform.position.y;
        obser[12] = Food1.transform.position.z;
        obser[13] = Food2.transform.position.x;
        obser[14] = Food2.transform.position.y;
        obser[15] = Food2.transform.position.z;
        obser[16] = Food3.transform.position.x;
        obser[17] = Food3.transform.position.y;
        obser[18] = Food3.transform.position.z;
        obser[19] = Food;
        obser[20] = Health;
        obser[21] = speed;  

    }
    // Update is called once per frame
    void Update()
    {


        Vector3 targetPos = AnimZombie.transform.position;
        Timepassed += Time.deltaTime;
        seconds = (int)Timepassed;

        if (Food <= 0)
        {
            FoodZerotimeSec += Time.deltaTime;
            FoodZerotime = (int)FoodZerotimeSec;
            if (FoodZerotime == 2)
            {
                isDead = true;
                Health = 0;
                this.HealthFiller.size = new Vector2(0f, this.HealthFiller.size.y);
                SetReward(-1f);
                Model.SetActive(false);
                TopContainer.SetActive(false);
                BottomContainer.SetActive(false);
                gun.SetActive(false);
                this.enabled = false;
            }
        }



        //Food Level has been decreased
        if (PrevFood - Food > 0)
        {
            float factor = PrevFood - Food;
            Health -= factor / 2;
            if (this.HealthFiller.size.x > 0.2f)
            {
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x - 0.02f, this.HealthFiller.size.y);
            }
            PrevFood = Food;

        }
        else if (Food - PrevFood > 0)//Earned Food
        {
            float factor = Food - PrevFood;
            Health += factor / 2;
            if (this.HealthFiller.size.x < 1)
            {
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            }
            PrevFood = Food;


        }

        if (seconds == count)
        {
            //  PrevSecond = Timepassed;
            
                //Food Decremented by 0.5f after every 2sec
                Food = Food - 0.75f;
                if (FoodFiller.size.x > 0)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.1f, this.FoodFiller.size.y);
                }
            

            //Agent is allowed to eat from res only once in a second
            Food1.SetActive(true);
            Food2.SetActive(true);
            Food3.SetActive(true);
            ateFromRes = false;
            //  onceInSecond = true;
            count += 1;
        }



    }
    public override void AgentAction(float[] vectorAction, string textAction)
    {
        int ANNaction = TestNetwork(obser); //Action from ANN
        action = Mathf.FloorToInt(vectorAction[0]);   //Action From RL
        iteration++;
        //For 20% ANN and 80% RL
        //if(iteration % 5 == 0)
        //{
        //    action = ANNaction;
        //}

        //For 80% ANN and 20% RL

        /*if (iteration % 5 != 0)
        {
            action = ANNaction;
        }*/

        //approx. 30% ANN and 70% RL
        //if (iteration % 3 == 0)
        //{
        //    action = ANNaction; //Action from ANN
        //}

        //approx. 70% ANN and 30% RL

        if (iteration % 3 != 0)
        {
            action = ANNaction; //Action from ANN
        }

        float dist1 = Vector3.Distance(Hallo.transform.position, Food1.transform.position);
        float dist2 = Vector3.Distance(Hallo.transform.position, Food2.transform.position);
        float dist3 = Vector3.Distance(Hallo.transform.position, Food3.transform.position);

        //Eat action

        float distWithMarko = Vector3.Distance(Model.transform.position, Marko.transform.position);
        float distWithLara = Vector3.Distance(Model.transform.position, Lara.transform.position);

        if (action == 0)
        {
            idle++;
            AnimZombie.SetTrigger("idle");
        }
        //Movement
        if (action == 1 || action == 2 || action == 3 || action == 4)
        {
            AnimZombie.SetTrigger("run");
            move++;

        }


        //Eat 
        
        if (action == 5 && (distWithLara < 1.42))
        {
            Steal++;
            Food++;
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Lara.Food--;
            if (Lara.FoodFiller.size.x > 0)
            {
                Lara.FoodFiller.size = new Vector2(Lara.FoodFiller.size.x - 0.02f, Lara.FoodFiller.size.y);
            }
        }
        //Stealing from Marko
        else if (action == 5 && (distWithMarko < 1.42))
        {
            Steal++;
            Food++;
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Marko.Food--;
            if (Marko.FoodFiller.size.x > 0)
            {
                Marko.FoodFiller.size = new Vector2(Marko.FoodFiller.size.x - 0.02f, Marko.FoodFiller.size.y);
            }
        }
        else if (action == 5 && ateFromRes == false && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42))
        {
            Eat++;
            Food++;
            
            ateFromRes = true;

            if (dist1 < 1.42)
            {
                Food1.SetActive(false);

                if (FoodFiller.size.x <= 1)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
                }


            }

            else if (dist2 < 1.42f)
            {

                Food2.SetActive(false);

                if (FoodFiller.size.x <= 1)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
                }


            }

            else if (dist3 < 1.42f)
            {
                Food3.SetActive(false);

                if (FoodFiller.size.x <= 1)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
                }

            }


        }


        //Sharing
       if (action == 6 && distWithMarko <= 1.42f)
        {
            Share++;

            if (Food > 0)
            {
                this.Food -= 0.5f;
                Marko.Food += 0.5f;
               
            }

            if (this.FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            if (Marko.FoodFiller.size.x <= 1)
            {
                Marko.FoodFiller.size = new Vector2(Marko.FoodFiller.size.x + 0.02f, Marko.FoodFiller.size.y);
            }

        }
        else if (action == 6 && distWithLara <= 1.42f)
        {
            Share++;
            if (Food > 0)
            {
                this.Food -= 0.5f;
               Lara.Food += 0.5f;
              
            }

            if (this.FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            if (Lara.FoodFiller.size.x <= 1)
            {
                Lara.FoodFiller.size = new Vector2(Lara.FoodFiller.size.x + 0.02f, Lara.FoodFiller.size.y);
            }
        }

        ////Attacked By Hallo
        //if (action == 7)
        //{
        //    Attack++;
           
        //}


        //if (Lara.action == 7 && Vector3.Distance(this.transform.position, Lara.transform.position) < 3)
        //{
        //    Lara.AddReward(-0.5f);
        //    Lara.Attack++;
        //    if (Food > 0)
        //    {
        //        Food--;
        //    }
        //    Lara.transform.LookAt(this.transform.position);
        //    Lara.AnimZombie.SetTrigger("attack");
        //    Lara.bulletfire.ShootBullet(AttackParticle, Hallo, ParticlesContainer);

        //    //Increment in Rivalary level for Hallo
        //    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
        //}


        //if (Marko.action == 7 && Vector3.Distance(this.transform.position, Marko.transform.position) < 3)
        //{
        //    Marko.transform.LookAt(this.transform.position);
        //    Marko.AnimZombie.SetTrigger("attack");
        //    Marko.bulletfire.ShootBullet(AttackParticle, Hallo, ParticlesContainer);
        //    Food--;
        //    if (FoodFiller.size.x > 0)
        //    {
        //        FoodFiller.size = new Vector2(FoodFiller.size.x - 0.02f, FoodFiller.size.y);
        //    }
        //}

    }
    private void FixedUpdate()
    {

        //Move PLayer


        //Move Player forward
        if (action == 1)
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.left);
            transform.position += Vector3.left * Time.deltaTime * speed;
        }

        //if (Input.GetKey(KeyCode.UpArrow))
        if (action == 2)

        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.forward);
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }

        //Move Player Backward
        //  if (Input.GetKey(KeyCode.DownArrow))
        if (action == 3)
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.back);
            transform.position -= Vector3.forward * Time.deltaTime * speed;
        }

        //Move Player left
        //if (Input.GetKey(KeyCode.LeftArrow))

        //Move Player Right
        if (action == 4)
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.right);
            transform.position -= Vector3.left * Time.deltaTime * speed;
        }
    }

    private static void ImportNetwork()
    {

        _network = ImportHelper.ImportNetwork();
        if (_network == null)
        {
            Debug.Log("\t****Something went wrong while importing your network.****");
            return;
        }

        _numInputParameters = _network.InputLayer.Count;
        _hiddenNeurons = new int[_network.HiddenLayers.Count];
        _numOutputParameters = _network.OutputLayer.Count;

        Debug.Log("\t**Network successfully imported.**");

    }
    private static int TestNetwork(double[] array)
    {
        double max = 0.0;
        int action = 0 ;
        Debug.Log("\tTesting Network");
        var values = array;
        int act1 = -1;
        if (values == null)
        {
            return -1;
        }
      //  Debug.Log("values inserted successfully");
        var results = _network.Compute(values);
        for(int i= 0; i<results.Length; i++)
        {
            if(results[i]> max)
            {
                max = results[i];
                action = i;
            }
        }
        Debug.Log($"\tOutput: {action}");
        return action;

    }

}

﻿using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLAgents;


public class MarkoScript : Agent
{
    Animator AnimZombie;
    public float Timepassed;
    public Lara Lara;
    float speed = 5.0f;
    public int Agentid = 2;
    public float Food;
    public float PrevFood = 10;
    public float Health;
    public int action;
    int seconds = 0;
    public PythonCommunicator py;
    int i = 0;
    int count = 0;
    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;
    Random random = new Random();
    public SpriteRenderer FoodFiller;
    public SpriteRenderer HealthFiller;


    public float prevOxeHallo;
    public float prevOxeLara;
    //public float OxetocinInHalloForMarko;
    //public float OxetocinInLaraForMarko;

    //For dopamin increment in selfish agent
    //float PrevFoodForDopamin = 0;
    public GameObject Marko;

    float Pfood;
    bool once = false;
    bool healthinc;
    public float dist;
    public AgentMove Hallo;
    // public float Dopamin = 1;
    //public float OxetocinForHallo = 2;
    //public float OxetocinForLara = 2;

    ActionList l = new ActionList();

    //For Coin Colection
    public GameObject Coin1;
    public GameObject Coin2;
    public GameObject Coin3;
    public GameObject Coin4;
    public int numberofCoins = 0;
    Coin coin;
    public float Cointime = 0;
    public int Coinseconds;
    //Rivalary Levels
    //public float RLForLara;
    //public float RLForHallo;
    //For health kit collection
    public GameObject AIDkit1;
    public GameObject AIDkit2;
    public int healthKit = 0;
    FirstAidKit Aidkit;
    //bool AttackedByHallo = false;
    //bool AttackedByLara = false;
    //Bullet fire
    public GameObject Player;
    public GameObject AttackParticle;
    public GameObject ParticlesContainer;
    //public PythonCommunicator py;

    float FoodZerotimeSec = 0;
    int FoodZerotime = 0;
    BulletFire bulletfire;

    Vector3 AgentStartingPos;

    private void Awake()
    {
        this.AttackParticle.SetActive(false);
        Physics.IgnoreLayerCollision(11, 11);
    }

    public override void CollectObservations()
    {
        AddVectorObs(Timepassed);
        AddVectorObs(Lara.transform.position);
        AddVectorObs(this.transform.position);
        AddVectorObs(Food1.transform.position);
        AddVectorObs(Food2.transform.position);
        AddVectorObs(Food3.transform.position);
        AddVectorObs(Hallo.transform.position);
        AddVectorObs(Food);
        AddVectorObs(Health);
        AddVectorObs(Coin1.transform.position);
        AddVectorObs(Coin2.transform.position);
        AddVectorObs(Coin3.transform.position);
        AddVectorObs(Coin4.transform.position);
        AddVectorObs(AIDkit1.transform.position);
        AddVectorObs(AIDkit2.transform.position);
        AddVectorObs(AttackParticle.transform.position);
        AddVectorObs(ParticlesContainer.transform.position);
        AddVectorObs(speed);
        AddVectorObs(coin);
        AddVectorObs(Coinseconds);
        AddVectorObs(AnimZombie.transform.position);

    }
    // Use this for initialization
    void Start()
    {
        //   OxetocinInHalloForMarko = 0;
        // OxetocinInLaraForMarko = 0;
        coin = new Coin();
        Timepassed = 0;
        Food = 10;
        //  PrevFoodForDopamin = 10;
        Health = 10;
        healthinc = false;
        AnimZombie = GetComponent<Animator>();
        Aidkit = new FirstAidKit();
        bulletfire = new BulletFire();
        py = new PythonCommunicator();
        AgentStartingPos = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        // py.Communication(Agentid, LaraModel, Marko, HalloModel, this, Hallo, Lara, this.Dopamin, this.OxetocinForHallo, this.OxetocinForLara);
        if (Food == 0)
        {
            FoodZerotimeSec += Time.deltaTime;
            FoodZerotime = (int)FoodZerotimeSec;
            if (FoodZerotime == 3)
            {
                Health = 0;
            }
        }
        if (DieAgent.MarkoLive == true)
        {
            AgentReset();
        }

        // DeadTime
        if (this.Health <= 0)
        {
            DieAgent.MarkoDied = true;
            Player.active = false;
        }
        Vector3 targetPos = AnimZombie.transform.position;
        Timepassed += Time.deltaTime;
        seconds = (int)Timepassed;
        // action = py.AgentAction;
        if (seconds == count)
        {
            count += 1;
            healthinc = false;
            once = false;

        }
        //after two seconds
        if (seconds == i)
        {
            if (Food > 0)
            {
                Food = Food - 0.5f;
                if (FoodFiller.size.x > 0)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
                }
            }
            i += 2;

        }
        if (PrevFood - Food == 1)
        {
            Health -= 0.5f;
            if (HealthFiller.size.x > 0)
            {
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x - 0.02f, this.HealthFiller.size.y);
            }
            PrevFood = Food;
        }





        if (Food - PrevFood == 1 && healthinc == false)
        {
            Health++;
            if (HealthFiller.size.x < 1)
            {
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            }
            healthinc = true;
        }
        //Coin collection

        Cointime = Cointime + Time.deltaTime;
        Coinseconds = (int)Cointime;
        numberofCoins = numberofCoins + coin.coin_production(Coinseconds, Marko, Coin1, Coin2, Coin3, Coin4);

        //Health Kit Collection
        healthKit = Aidkit.AIDKIT(Coinseconds, Marko, AIDkit1, AIDkit2);
        if (healthKit > 0)
        {
            Health += 0.5f;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            healthKit = 0;
        }
        if (Coinseconds == 5)
        {
            Cointime = 0;
            Coinseconds = 0;
        }

    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        action = Mathf.FloorToInt(vectorAction[0]);
        float dist1 = Vector3.Distance(Marko.transform.position, Food1.transform.position);
        dist = dist1;

        //Eat action
        float dist2 = Vector3.Distance(Hallo.transform.position, Food2.transform.position);
        float dist3 = Vector3.Distance(Hallo.transform.position, Food3.transform.position);
        float distWithLara = Vector3.Distance(Hallo.transform.position, Lara.transform.position);
        float distWithHallo = Vector3.Distance(Hallo.transform.position, Hallo.transform.position);

        //Stop Agent Animation
        //    if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A))
        if (action == 0)
        {
            AnimZombie.SetTrigger("idle");
        }
        //Movement
        if (action == 1 || action == 2 || action == 3 || action == 4)
        {
            AnimZombie.SetTrigger("run");

        }

        //Steal
        if (action == 5 && once == false && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42))
        {
            Food++;
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            once = true;
            AddReward(+1.0f);
        }
        else if (action == 5 && once == false && (distWithLara < 1.42))
        {
            Food++;
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Lara.Food--;
            AddReward(+1.0f);
            once = true;
        }
        else if (action == 5 && once == false && (distWithHallo < 1.42))
        {
            Food++;
            Hallo.interactionWithMarko++;
            AddReward(+1.0f);
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Hallo.Food--;
            once = true;
        }
        //Run Agent Animation 

        //Attacked By Hallo
        if (Hallo.action == 7 && Vector3.Distance(this.transform.position, Hallo.transform.position) < 3 /*&& AttackedByHallo && Hallo.OxetocinForMarko < 3*/)
        {
            if (Food > 0)
            {
                Food--;
            }
            transform.LookAt(Hallo.transform.position);
            AnimZombie.SetTrigger("attack");
            bulletfire.ShootBullet(AttackParticle, Player, ParticlesContainer);

            //Increment in Rivalary level for Hallo
            this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
        }


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


    void AgentReset()
    {
        Timepassed = 0;
        Food = 10;
        Health = 10;
        healthinc = false;
        Food3.SetActive(false);
        numberofCoins = 0;
        //Dopamin = 1;
        //OxetocinForHallo = 2;
        //OxetocinForLara = 2;
        healthKit = 0;
        seconds = 0;
        i = 0;
        count = 0;
        Cointime = 0;
        PrevFood = 10;
        once = false;
        //OxetocinInHalloForMarko = 0;
        //OxetocinInLaraForMarko = 0;
        DieAgent.MarkoLive = false;
        //this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
        this.transform.position = AgentStartingPos;
        FoodZerotimeSec = 0;
        FoodZerotime = 0;
    }


}

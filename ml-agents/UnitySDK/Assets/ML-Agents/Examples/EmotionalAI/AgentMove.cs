using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLAgents;
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
    public bool once = false;
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

    

    private void Awake()
    {
        this.AttackParticle.SetActive(false);
        Physics.IgnoreLayerCollision(11, 11);
    }


    // Use this for initialization
    public override void InitializeAgent()
    {

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
        AddVectorObs(Food1.transform.position);//3
        AddVectorObs(Food2.transform.position);//3
        AddVectorObs(Food3.transform.position);//3
        AddVectorObs(this.transform.position);//3
        AddVectorObs(Food);//1
        AddVectorObs(Health);//1
        AddVectorObs(AttackParticle.transform.position);//3
        AddVectorObs(ParticlesContainer.transform.position);//3
        AddVectorObs(speed);//1
    
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
    public override void AgentAction(float[] vectorAction)
    {
        action = Mathf.FloorToInt(vectorAction[0]);

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
        if(action == 5)
        {
            Eat++;
        }
        if (action == 5 && (distWithLara < 1.42))
        {
            Steal++;
            Food++;
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Lara.Food--;
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
        if(action == 6)
        {
            Share++;
        }
        if (action == 6 && distWithMarko <= 1.42f)
        {
          
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
                Marko.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }

        }
        else if (action == 6 && distWithLara <= 1.42f)
        {
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
                Lara.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
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
    /*  void AgentReset()
      {
          Timepassed = 0;
          Timecheck = 0;
          Food = 10;
          Health = 10;
          healthinc = false;
          Food3.SetActive(false);
          numberofCoins = 0;
          Dopamin = 3;
          OxetocinForMarko = 2;
          OxetocinForLara = 2;
          healthKit = 0;
          seconds = 0;
          i = 0;
          count = 0;
          Cointime = 0;
          PrevFood = 10;
          once = false;
          DieAgent.HalloLive = false;
          this.transform.position = AgentStartingPos;
          FoodZerotimeSec = 0;
          FoodZerotime = 0;
          interactionWithLara = 0;
          interactionWithMarko = 0;
      } */
}

using System.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLAgents;
//SELFLESS

public class Lara : Agent
{

    public static List<string[]> rowData = new List<string[]>();
    public int idle, move, Eat, Share, Attack = 0;
    public bool onceInSecond = false;
    public float PrevSecond = 1;


    public MarkoScript Marko;
    public AgentMove Hallo;
    public GameObject LaraModel;
    public Animator AnimZombie;
    public GameObject TopContainer;
    public GameObject BottomContainer;
    public GameObject gun;
    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;
    public SpriteRenderer FoodFiller;
    public SpriteRenderer HealthFiller;


    public float Timepassed;




    public int Agentid = 1;
    public float Food;
    public float Health;
    public int action;
    float speed = 5.0f;


    public int seconds = 0;

    public int count = 0;




    ActionList al = new ActionList();

    //public float OxetocinForHallo;
    public float PrevFood = 5;

    public bool ateFromRes = false;







   
    public GameObject Player;
    public GameObject AttackParticle;
    public GameObject ParticlesContainer;
    public BulletFire bulletfire;

    public float FoodZerotimeSec = 0;
    public int FoodZerotime = 0;
    public Vector3 AgentStartingPos;

    //Rivalary Levels
    //public float RLForMarko;
    //public float RLForHallo;

    private void Awake()
    {
        this.AttackParticle.SetActive(false);
        Physics.IgnoreLayerCollision(11, 11);
    }

    public override void CollectObservations()
    {
        AddVectorObs(Agentid);
        AddVectorObs(Timepassed);
        AddVectorObs(this.transform.position);
        AddVectorObs(Marko.transform.position);
        AddVectorObs(Hallo.transform.position);

        AddVectorObs(Food1.transform.position);
        AddVectorObs(Food2.transform.position);
        AddVectorObs(Food3.transform.position);

        AddVectorObs(Food);
        AddVectorObs(Health);

      AddVectorObs(speed);
     

    }

    public override void AgentAction(float[] vectorAction)
    {
        action = Mathf.FloorToInt(vectorAction[0]);

        float dist1 = Vector3.Distance(LaraModel.transform.position, Food1.transform.position);
        float dist2 = Vector3.Distance(LaraModel.transform.position, Food2.transform.position);
        float dist3 = Vector3.Distance(LaraModel.transform.position, Food3.transform.position);

        //Eat action

        float distWithMarko = Vector3.Distance(LaraModel.transform.position, Marko.transform.position);
        float distWithHallo = Vector3.Distance(LaraModel.transform.position, Hallo.transform.position);

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
        if (action == 5)
        {
        }


        if (action == 5 && ateFromRes == false && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42))
        {
            Eat++;

            Food++;
            //Lara Should avoid eating
            AddReward(-0.05f);

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

        if (action == 6 /*&& */)
        {
            Share++;
            if (distWithMarko <= 1.42f)
            {

                if (Food > 0)
                {
                    this.Food -= 0.5f;
                    Marko.Food += 0.5f;
                    AddReward(1f);
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

        }
        else if (action == 6 /*&&*/)
        {
            Share++;
            if (distWithHallo <= 1.42f)
            {

                if (Food > 0)
                {
                    this.Food -= 0.5f;
                    Hallo.Food += 0.5f;
                    AddReward(1f);
                }

                if (this.FoodFiller.size.x > 0)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
                }
                if (Hallo.FoodFiller.size.x <= 1)
                {
                    Hallo.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
                }
            }
        }

        //Attacked By Hallo



        //if (Hallo.action == 7 && Vector3.Distance(this.transform.position, Hallo.transform.position) < 3)
        //{
        //    if (Food > 0)
        //    {
        //        Food--;
        //    }
        //    Hallo.transform.LookAt(this.transform.position);
        //    Hallo.AnimZombie.SetTrigger("attack");
        //    Hallo.bulletfire.ShootBullet(AttackParticle, Player, ParticlesContainer);

        //    //Increment in Rivalary level for Hallo
        //    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
        //}


        //if (Marko.action == 7 && Vector3.Distance(this.transform.position, Marko.transform.position) < 3)
        //{
        //    Marko.transform.LookAt(this.transform.position);
        //    Marko.AnimZombie.SetTrigger("attack");
        //    Marko.bulletfire.ShootBullet(AttackParticle, Player, ParticlesContainer);
        //    Food--;
        //    if (FoodFiller.size.x > 0)
        //    {
        //        FoodFiller.size = new Vector2(FoodFiller.size.x - 0.02f, FoodFiller.size.y);
        //    }
        //}

    }
    // Use this for initialization
    public override void InitializeAgent()
    {

        Timepassed = 0;
        Food = 5;
        Health = 5;
        PrevFood = 5;
        PrevSecond = 1;
        count = 1;
        AnimZombie = GetComponent<Animator>();
        Food3.SetActive(false);
        //Dopamin = 0;
        //OxetocinInHalloForLara = 0;
        //OxetocinInMarkoForLara = 0;
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
                Hallo.SetReward(-1f);
                // Marko.SetReward(-1f);
                LaraModel.SetActive(false);
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
            PrevSecond = Timepassed;
            if (Food > 0)
            {
                //Food Decremented by 0.5f after every 2sec
                Food = Food - 0.75f;
                if (FoodFiller.size.x > 0)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
                }
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

}
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
    public float PrevFood = 10;

    public bool ateFromRes = false;







    bool AttackedByHallo = false;
    bool AttackedByMarko = false;

    //For Coin Collection
    public GameObject Coin1;
    public GameObject Coin2;
    public GameObject Coin3;
    public GameObject Coin4;
    public int numberofCoins = 0;
    Coin coin;
    public float Cointime = 0;
    public int Coinseconds;

    //For health kit collection
    public GameObject AIDkit1;
    public GameObject AIDkit2;
    public int healthKit = 0;
    FirstAidKit Aidkit;
    //public PythonCommunicator py;
    //Bullet fire
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

    public override void AgentAction(float[] vectorAction, string x)
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



        if (action == 5 && ateFromRes == false && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42))
        {
            Eat++;
            Food++;
            //Lara Should avoid eating
            AddReward(-0.02f);

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
     


        if (Hallo.action == 7 && Vector3.Distance(this.transform.position, Hallo.transform.position) < 3)
        {
            if (Food > 0)
            {
                Food--;
            }
            Hallo.transform.LookAt(this.transform.position);
            Hallo.AnimZombie.SetTrigger("attack");
            Hallo.bulletfire.ShootBullet(AttackParticle, Player, ParticlesContainer);

            //Increment in Rivalary level for Hallo
            this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
        }


        if (Marko.action == 7 && Vector3.Distance(this.transform.position, Marko.transform.position) < 3)
        {
            Marko.transform.LookAt(this.transform.position);
            Marko.AnimZombie.SetTrigger("attack");
            Marko.bulletfire.ShootBullet(AttackParticle, Player, ParticlesContainer);
            Food--;
            if (FoodFiller.size.x > 0)
            {
                FoodFiller.size = new Vector2(FoodFiller.size.x - 0.02f, FoodFiller.size.y);
            }
        }

    }
    // Use this for initialization
    public override void InitializeAgent()
    {

        Timepassed = 0;
        Food = 10;
        Health = 10;
        PrevFood = 10;
        PrevSecond = 1;
        count = 1;
        AnimZombie = GetComponent<Animator>();
        Food3.SetActive(false);
        //Dopamin = 0;
        //OxetocinInHalloForLara = 0;
        //OxetocinInMarkoForLara = 0;
        coin = new Coin();
        Aidkit = new FirstAidKit();
        bulletfire = new BulletFire();
        AgentStartingPos = this.transform.position;
        string[] rowDataTemp = new string[23];
        rowDataTemp[0] = "Agentid";
        rowDataTemp[1] = "TimePassed";
        rowDataTemp[2] = "LaraPosition";
        rowDataTemp[3] = "MarkoPosition";
        rowDataTemp[4] = "Food1Position";
        rowDataTemp[5] = "Food2Position";
        rowDataTemp[6] = "Food3Position";
        rowDataTemp[7] = "HalloPosition";
        rowDataTemp[8] = "Food";
        rowDataTemp[9] = "Health";
        rowDataTemp[10] = "Coin1Position";
        rowDataTemp[11] = "Coin2Position";
        rowDataTemp[12] = "Coin3Position";
        rowDataTemp[13] = "Coin4Position";
        rowDataTemp[14] = "Aidkit1Position";
        rowDataTemp[15] = "Aidkit2Position";
        rowDataTemp[16] = "AttackParticlePosition";
        rowDataTemp[17] = "ParticleContainPosition";
        rowDataTemp[18] = "Speed";
        rowDataTemp[19] = "Coin";
        rowDataTemp[20] = "CoinSeconds";
        rowDataTemp[21] = "AgentAnimePosition";
        rowDataTemp[22] = "Action";
        rowData.Add(rowDataTemp);
        //  py = new PythonCommunicator();
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
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x - 0.2f, this.HealthFiller.size.y);
            }
            PrevFood = Food;

        }
        else if (Food - PrevFood > 0)//Earned Food
        {
            float factor = Food - PrevFood;
            Health += factor / 2;
            if (this.HealthFiller.size.x < 1)
            {
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.2f, this.HealthFiller.size.y);
            }
            PrevFood = Food;


        }

        if (seconds == count )
        {
            PrevSecond = Timepassed;
            if (Food > 0)
            {
                //Food Decremented by 0.5f after every 2sec
                Food = Food - 0.25f;
                if (FoodFiller.size.x > 0)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.1f, this.FoodFiller.size.y);
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



        //Coin collection
        Cointime = Cointime + Time.deltaTime;
        Coinseconds = (int)Cointime;
        numberofCoins = numberofCoins + coin.coin_production(Coinseconds, LaraModel, Coin1, Coin2, Coin3, Coin4);

        //Health Kit Collection
        healthKit = Aidkit.AIDKIT(Coinseconds, LaraModel, AIDkit1, AIDkit2);
        if (healthKit > 0)
        {
            Health += 0.5f;
            if (this.HealthFiller.size.x < 1)
            {
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            }

            healthKit = 0;
        }
        if (Coinseconds == 5)
        {
            Cointime = 0;
            Coinseconds = 0;
        }
        // py.nextState(id, LaraModel, MarkoModel, HalloModel, Marko, Hallo, this, this.Dopamin, this.OxetocinForHallo, this.OxetocinForMarko, this.Reward);
        // Add Data to write into file

        string[] rowDataTemp = new string[23];
        rowDataTemp[0] = Agentid.ToString();
        rowDataTemp[1] = Timepassed.ToString();
        rowDataTemp[2] = this.transform.position.ToString();
        rowDataTemp[3] = Marko.transform.position.ToString();
        rowDataTemp[4] = Food1.transform.position.ToString();
        rowDataTemp[5] = Food2.transform.position.ToString();
        rowDataTemp[6] = Food3.transform.position.ToString();
        rowDataTemp[7] = Hallo.transform.position.ToString();
        rowDataTemp[8] = Food.ToString();
        rowDataTemp[9] = Health.ToString();
        rowDataTemp[10] = Coin1.transform.position.ToString();
        rowDataTemp[11] = Coin2.transform.position.ToString();
        rowDataTemp[12] = Coin3.transform.position.ToString();
        rowDataTemp[13] = Coin4.transform.position.ToString();
        rowDataTemp[14] = AIDkit1.transform.position.ToString();
        rowDataTemp[15] = AIDkit2.transform.position.ToString();
        rowDataTemp[16] = AttackParticle.transform.position.ToString();
        rowDataTemp[17] = ParticlesContainer.transform.position.ToString();
        rowDataTemp[18] = speed.ToString();
        rowDataTemp[19] = coin.ToString();
        rowDataTemp[20] = Coinseconds.ToString();
        rowDataTemp[21] = AnimZombie.transform.position.ToString();
        rowDataTemp[22] = action.ToString();
        rowData.Add(rowDataTemp);
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
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLAgents;


public class MarkoScript : Agent
{

    public static List<string[]> rowData = new List<string[]>();
    public Animator AnimZombie;
    public float Timepassed;

    public Lara Lara;
    public GameObject Marko;
    public AgentMove Hallo;

    float speed = 5.0f;
    public int Agentid = 2;
    public float Food;
    public float PrevFood = 10;
    public float Health;
    public int action;
    public int seconds = 0;
    public PythonCommunicator py;
    public int i = 0;
    public int oneSecondCounter = 1;
    public int idle, move, Eat, Share, Attack = 0;


    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;

    public SpriteRenderer FoodFiller;
    public SpriteRenderer HealthFiller;

    public float dist;

    ActionList l = new ActionList();

    //For Coin Colection
    public GameObject Coin1;
    public GameObject Coin2;
    public GameObject Coin3;
    public GameObject Coin4;

    public int numberofCoins = 0;
    public Coin coin;
    public float Cointime = 0;
    public int Coinseconds;

    public GameObject AIDkit1;
    public GameObject AIDkit2;
    public GameObject TopContainer;
    public GameObject BottomContainer;
    public GameObject gun;


    public int healthKit = 0;
    FirstAidKit Aidkit;

    public GameObject Player;
    public GameObject AttackParticle;
    public GameObject ParticlesContainer;

    public bool MarkoDied = false;
    public float FoodZerotimeSec = 0;
    public int FoodZerotime = 0;
    public BulletFire bulletfire;

    public Vector3 AgentStartingPos;
    private GameAcademy academy;
    private void Awake()
    {
        this.AttackParticle.SetActive(false);
        Physics.IgnoreLayerCollision(11, 11);
    }

    public override void CollectObservations()
    {
        AddVectorObs(Agentid);//1
        AddVectorObs(Timepassed);//1
        AddVectorObs(Lara.transform.position);//3
        AddVectorObs(this.transform.position);//3
        AddVectorObs(Food1.transform.position);//3
        AddVectorObs(Food2.transform.position);//3
        AddVectorObs(Food3.transform.position);//3
        AddVectorObs(Hallo.transform.position);//3
        AddVectorObs(Food);//1
        AddVectorObs(Health);//1
        AddVectorObs(Coin1.transform.position);//3
        AddVectorObs(Coin2.transform.position);//3
        AddVectorObs(Coin3.transform.position);//3
        AddVectorObs(Coin4.transform.position);//3
        AddVectorObs(AIDkit1.transform.position);//3
        AddVectorObs(AIDkit2.transform.position);//3
        AddVectorObs(AttackParticle.transform.position);//3
        AddVectorObs(ParticlesContainer.transform.position);//3
        AddVectorObs(speed);//1
        AddVectorObs(coin);//1
        AddVectorObs(Coinseconds);//1
        AddVectorObs(AnimZombie.transform.position);//3

    }
    // Use this for initialization
    public override void InitializeAgent()
    {
        academy = FindObjectOfType(typeof(GameAcademy)) as GameAcademy;
        coin = new Coin();
        Timepassed = 0;
        Food = 10;
        oneSecondCounter = 1;
        //  PrevFoodForDopamin = 10;
        Health = 10;
        AnimZombie = GetComponent<Animator>();
        Aidkit = new FirstAidKit();
        bulletfire = new BulletFire();
        py = new PythonCommunicator();
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
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = AnimZombie.transform.position;
        Timepassed += Time.deltaTime;
        seconds = (int)Timepassed;

        //Food is zero for 2 seconds then inactive agent
        if (Food <= 0)
        {
            FoodZerotimeSec += Time.deltaTime;
            FoodZerotime = (int)FoodZerotimeSec;
            if (FoodZerotime == 2)
            {
                Health = 0;
                this.HealthFiller.size = new Vector2(0f, this.HealthFiller.size.y);
                Hallo.SetReward(-1f);
                Player.SetActive(false);
                TopContainer.SetActive(false);
                BottomContainer.SetActive(false);
                gun.SetActive(false);
                this.enabled = false;

            }
        }

        //After one second
        if (Timepassed == oneSecondCounter)
        {
            oneSecondCounter += 1;
            if (Food > 0)
            {
                Food = Food - 0.25f;
                if (FoodFiller.size.x > 0)
                {
                    this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
                }
            }
            Food1.SetActive(true);
            Food2.SetActive(true);
            Food3.SetActive(true);
        }

        //Increment in food
        if (PrevFood - Food > 0)
        {
            Health -= (PrevFood - Food) / 2;
            if (HealthFiller.size.x > 0)
            {
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x - 0.02f, this.HealthFiller.size.y);
            }
            PrevFood = Food;
        }

        //Decrement in food
        if (Food - PrevFood > 0)
        {
            Health += (Food - PrevFood) / 2;
            if (HealthFiller.size.x < 1)
            {
                this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            }
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
            if (HealthFiller.size.x < 1)
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

    }


    public override void AgentAction(float[] vectorAction, string textAction)
    {
        action = Mathf.FloorToInt(vectorAction[0]);
        float dist1 = Vector3.Distance(Marko.transform.position, Food1.transform.position);
        dist = dist1;

        //Eat action
        float dist2 = Vector3.Distance(Marko.transform.position, Food2.transform.position);
        float dist3 = Vector3.Distance(Marko.transform.position, Food3.transform.position);
        float distWithLara = Vector3.Distance(Marko.transform.position, Lara.transform.position);
        float distWithHallo = Vector3.Distance(Marko.transform.position, Hallo.transform.position);

        //Stop Agent Animation
        //    if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A))
        if (action == 0)
        {
            idle++;
            AnimZombie.SetTrigger("idle");
        }
        //Movement
        if (action == 1 || action == 2 || action == 3 || action == 4)
        {
            move++;
            AnimZombie.SetTrigger("run");

        }

        //Steal
        if (action == 5 && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42))
        {
            Eat++;
            Food++;
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            if (dist1 < 1.42)
            {
                Food1.SetActive(false);
            }
            else if (dist2 < 1.42f)
            {

                Food2.SetActive(false);
            }

            else if (dist3 < 1.42f)
            {
                Food3.SetActive(false);
            }

            //Less Reward on eating from reservior
            AddReward(+0.5f);
        }
        else if (action == 5 && (distWithLara < 1.42))
        {
            Eat++;
            Food++;
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Lara.Food--;
            //More reward on stealing from other agents
            AddReward(+1.0f);
        }
        //Stealing from Hallo
        else if (action == 5 && (distWithHallo < 1.42))
        {
            Eat++;
            Food++;
            AddReward(+1.0f);
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Hallo.Food--;
        }

        //Share with Hallo
        if (action == 6 && distWithHallo <= 1.42f)
        {
            Share++;
            //Selfish agent should not share food
            AddReward(-1f);
            if (Food > 0)
            {
                this.Food -= 0.5f;
                Hallo.Food += 0.5f;
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



        //Sharing with Lara

        else if (action == 6 && distWithLara <= 1.42f /*&& OxetocinForMarko >= 2*/)
        {
            Share++;
            AddReward(-1f);
            if (Food > 0)
            {
                this.Food -= 0.5f;
                Lara.Food += 0.5f;

            }
            if (this.FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            if (Lara.FoodFiller.size.x < 1)
            {
                Lara.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
        }

        if (action == 7)
        {
            Attack++;
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
            if (FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
        }


        //If Attacked by Lara
        if (Lara.action == 7 && Vector3.Distance(this.transform.position, Lara.transform.position) < 3)
        {
            if (Food > 0)
            {
                Food--;
            }
            Lara.transform.LookAt(this.transform.position);
            Lara.AnimZombie.SetTrigger("attack");
            Lara.bulletfire.ShootBullet(AttackParticle, Player, ParticlesContainer);
            if (FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
        }

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


    /*  void AgentReset()
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
      } */

}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLAgents;
//SELFLESS

public class Lara : Agent
{

    public static List<string[]> rowData = new List<string[]>();
    Animator AnimZombie;

    public float Timepassed;
    public float Timecheck;
    //public float prevOxeHallo;
    //public float prevOxeMarko;
    public int Agentid = 1;
    float speed = 5.0f;
    public float Food;
    public float Health;
    public int action;
    public int seconds = 0;
    public int i = 0;
    public int count = 0;
    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;
    Random random = new Random();
    public SpriteRenderer FoodFiller;
    public SpriteRenderer HealthFiller;
    public float Reward;
    ActionList al = new ActionList();

    //public float OxetocinForHallo;
    public float PrevFood = 10;
    public float Pfood;
    public bool once = false;
    public bool healthinc;
    public float dist;
    public MarkoScript Marko;
    public AgentMove Hallo;
    public GameObject LaraModel;
    public float Dopamin;
    //public float OxetocinForMarko;
    //public float OxetocinInHalloForLara;
    //public float OxetocinInMarkoForLara;



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
    BulletFire bulletfire;

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

    public override void AgentAction(float[] vectorAction, string textAction)
    {
        action = Mathf.FloorToInt(vectorAction[0]);
        float dist1 = Vector3.Distance(Marko.transform.position, Food1.transform.position);
        dist = dist1;

        //Eat action
        float dist2 = Vector3.Distance(LaraModel.transform.position, Food2.transform.position);
        float dist3 = Vector3.Distance(LaraModel.transform.position, Food3.transform.position);
        float distWithMarko = Vector3.Distance(LaraModel.transform.position, Marko.transform.position);
        float distWithHallo = Vector3.Distance(LaraModel.transform.position, Hallo.transform.position);

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
        if (action == 5 && once == false && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42))
        {
            Food++;
            if (FoodFiller.size.x <= 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            once = true;
            //if (dist1 < 1.42)
            //{
            //    Food1.SetActive(false);
            //    Timecheck = 0;

            //}
            //else if (dist2 < 1.42f)
            //{
            //    Food2.SetActive(false);
            //    Timecheck = 0;

            //}
            //else if (dist3 < 1.42f)
            //{
            //    Food3.SetActive(false);
            //    Timecheck = 0;

            //}


        }
        //if ((int)Timecheck == 5)
        //{
        //    Food1.SetActive(true);
        //    Food2.SetActive(true);
        //    Food3.SetActive(true);

        //}

        if (Food - PrevFood == 1 && healthinc == false)
        {
            Health += 0.5f;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            healthinc = true;
        }

        //Run Agent Animation 
        //Sharing with Marko
        if (action == 6 && distWithMarko <= 1.42f /*&& OxetocinForMarko >= 2*/)
        {
            if (Food > 0)
            {
                this.Food -= 0.5f;
                Marko.Food += 0.5f;
                //  OxetocinInMarkoForLara += 0.5f;
                AddReward(1f);
            }
            //if (Marko.RLForLara > 0)
            //{
            //    Marko.RLForLara -= 0.5f;
            //}
            if (this.FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            if (Marko.FoodFiller.size.x <= 1)
            {
                Marko.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
        }
        //Attacked By Hallo
        if (Hallo.action == 7 && Vector3.Distance(this.transform.position, Hallo.transform.position) < 3 /*&& AttackedByHallo && Hallo.OxetocinForMarko < 3*/)
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
    void Start()
    {
        Reward = 0;
        Timepassed = 0;
        Timecheck = 0;
        Food = 10;
        Health = 10;
        healthinc = false;
        AnimZombie = GetComponent<Animator>();
        Food3.SetActive(false);
        //Dopamin = 0;
        //OxetocinInHalloForLara = 0;
        //OxetocinInMarkoForLara = 0;
        coin = new Coin();
        Aidkit = new FirstAidKit();
        bulletfire = new BulletFire();
        AgentStartingPos = this.transform.position;
        string[] rowDataTemp = new string[22];
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
        rowData.Add(rowDataTemp);
        //  py = new PythonCommunicator();
    }

    // Update is called once per frame
    void Update()
    {
        //this.action = 0;
        // py.Communication(id, LaraModel, MarkoModel, HalloModel, Marko, Hallo, this, this.Dopamin, this.OxetocinForHallo, this.OxetocinForMarko);
        if (Food == 0)
        {
            FoodZerotimeSec += Time.deltaTime;
            FoodZerotime = (int)FoodZerotimeSec;
            if (FoodZerotime == 3)
            {
                Health = 0;
            }
        }


        // DeadTime
        if (this.Health <= 0)
        {
            LaraModel.SetActive(false);
        }
        Vector3 targetPos = AnimZombie.transform.position;
        Timepassed += Time.deltaTime;
        Timecheck += Time.deltaTime;
        seconds = (int)Timepassed;
        //action = py.AgentAction;
        if (seconds == count)
        {
            count += 1;
            //action = Random.Range(1, 7);
            healthinc = false;
            once = false;
        }
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
            //this.HealthFiller.size = new Vector2(this.HealthFiller.size.x - 0.02f, this.HealthFiller.size.y);
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
        numberofCoins = numberofCoins + coin.coin_production(Coinseconds, LaraModel, Coin1, Coin2, Coin3, Coin4);

        //Health Kit Collection
        healthKit = Aidkit.AIDKIT(Coinseconds, LaraModel, AIDkit1, AIDkit2);
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
        // py.nextState(id, LaraModel, MarkoModel, HalloModel, Marko, Hallo, this, this.Dopamin, this.OxetocinForHallo, this.OxetocinForMarko, this.Reward);
        // Add Data to write into file
        
        string[] rowDataTemp = new string[22];
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
    //void AgentReset()
    //{
    //    Tiollmepassed = 0;
    //    Timecheck = 0;
    //    Food = 10;
    //    Health = 10;
    //    healthinc = false;
    //    Food3.SetActive(false);
    //    numberofCoins = 0;
    //    Dopamin = 0;
    //    OxetocinInHalloForLara = 0;
    //    OxetocinInMarkoForLara = 0;
    //    healthKit = 0;
    //    seconds = 0;
    //    i = 0;
    //    count = 0;
    //    Cointime = 0;
    //    PrevFood = 10;
    //    once = false;
    //    DieAgent.LaraLive = false;
    //    this.transform.position = AgentStartingPos;
    //    FoodZerotimeSec = 0;
    //    FoodZerotime = 0;
    //}
}

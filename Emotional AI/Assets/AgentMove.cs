using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLAgents;
//2

public class AgentMove : Agent
{
    public Animator AnimZombie;
    public int interactionWithLara;
    public int interactionWithMarko;
    public float Timepassed;
    public float Timecheck;
    public Lara Lara;
    public int Agentid = 3;
    float speed = 3.0f;
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
    public GameObject Hallo;
    public float PrevFood = 10;
    public float Pfood;
    public bool once = false;
    public bool healthinc;
    public float dist;
    public MarkoScript Marko;
    public GameObject Coin1;
    public GameObject Coin2;
    public GameObject Coin3;
    public GameObject Coin4;
    bool Check = false;
    public GameObject AIDkit1;
    public GameObject AIDkit2;
    public float Cointime = 0;
    public int Coinseconds;
    Coin coin;
    bool Attack = false;


    FirstAidKit Aidkit;
    public int numberofCoins = 0;
    public float Dopamin = 3;
    public float OxetocinForMarko = 2;
    public float OxetocinForLara = 2;
    public int healthKit = 0;


    bool AttackedByMarko = false;
    bool AttackedByLara = false;
    //Bullet fire
    public GameObject Player;
    public GameObject AttackParticle;
    public GameObject ParticlesContainer;
    public BulletFire bulletfire;

    public float FoodZerotimeSec = 0;
    public int FoodZerotime = 0;
    public Vector3 AgentStartingPos;
    private GameAcademy academy;

    //Rivalary Levels
    public float RLForMarko;
    public float RLForLara;


    private void Awake()
    {
        this.AttackParticle.SetActive(false);
        Physics.IgnoreLayerCollision(11, 11);
    }


    // Use this for initialization
    void Start()
    {
        academy = FindObjectOfType(typeof(GameAcademy)) as GameAcademy;
        Timepassed = 0;
        Timecheck = 0;
        Food = 10;
        Health = 10;
        healthinc = false;
        AnimZombie = GetComponent<Animator>();
        Food3.SetActive(false);
        coin = new Coin();
        bulletfire = new BulletFire();
        Aidkit = new FirstAidKit();
        AgentStartingPos = this.transform.position;
    }


    public override void CollectObservations()
    {
        AddVectorObs(Agentid);
        AddVectorObs(Timepassed);
        AddVectorObs(Lara.transform.position);
        AddVectorObs(Marko.transform.position);
        AddVectorObs(Food1.transform.position);
        AddVectorObs(Food2.transform.position);
        AddVectorObs(Food3.transform.position);
        AddVectorObs(this.transform.position);
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
    // Update is called once per frame
    void Update()
    {
        if (Food == 0)
        {
            FoodZerotimeSec += Time.deltaTime;
            FoodZerotime = (int)FoodZerotimeSec;
            if (FoodZerotime == 3)
            {
                Health = 0;
            }
        }
       /* if (DieAgent.HalloLive == true)
        {
            AgentReset();
        } */

        // DeadTime
        if (this.Health <= 0)
        {
            Hallo.SetActive(false);
          //  DieAgent.HalloDied = true;
          //  Player.active = false;
        }

        Vector3 targetPos = Hallo.transform.position;
        targetPos.y = 0;
        Timepassed += Time.deltaTime;
        Timecheck += Time.deltaTime;
        seconds = (int)Timepassed;

        if (seconds == count)
        {
            count += 1;
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
            PrevFood = Food;
        }
        if ((int)Timecheck == 5)
        {
            Food1.SetActive(true);
            Food2.SetActive(true);
            Food3.SetActive(true);

        }

        if (Food - PrevFood == 1 && healthinc == false)
        {
            Health += 0.5f;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            healthinc = true;
        }
        //Run Agent Animation 
        //  if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A))
        Cointime = Cointime + Time.deltaTime;
        Coinseconds = (int)Cointime;
        numberofCoins = numberofCoins + coin.coin_production(Coinseconds, Hallo, Coin1, Coin2, Coin3, Coin4);
        healthKit = Aidkit.AIDKIT(Coinseconds, Hallo, AIDkit1, AIDkit2);
        if (healthKit > 0)
        {
            Health += 0.5f;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            healthKit = 0;
        }
        if (Coinseconds == 10)
        {
            Cointime = 0;
            Coinseconds = 0;
        }

    }

    public override void AgentAction(float[] vectorAction, string textAction)
    {

        action = Mathf.FloorToInt(vectorAction[0]);
        float CoinDist = Vector3.Distance(Hallo.transform.position, Coin1.transform.position);
        float dist1 = Vector3.Distance(Hallo.transform.position, Food1.transform.position);
        float dist2 = Vector3.Distance(Hallo.transform.position, Food2.transform.position);
        float dist3 = Vector3.Distance(Hallo.transform.position, Food3.transform.position);
        float DistanceWithMarko = Vector3.Distance(this.transform.position, Marko.transform.position);
        float distanceWithLara = Vector3.Distance(this.transform.position, Lara.transform.position);

        if (action == 0)
        {
            AnimZombie.SetTrigger("idle");
        }

        //Movement
        if (action == 1 || action == 2 || action == 3 || action == 4)
        {
            AnimZombie.SetTrigger("run");

        }


        //Eat if interaction with Marko is more
        if (action == 5 && once == false && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42) /*&& seconds >= 5
          && interactionWithMarko >= interactionWithLara
           && interactionWithMarko > 0*/)
        {
            Food++;
            this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            once = true;
            if (dist1 < 1.42)
            {
                Food1.SetActive(false);
                Timecheck = 0;

            }
            else if (dist2 < 1.42f)
            {
                Food2.SetActive(false);
                Timecheck = 0;

            }
            else if (dist3 < 1.42f)
            {
                Food3.SetActive(false);
                Timecheck = 0;

            }


        }
        else if (action == 5 && once == false && (distanceWithLara < 1.42) /*&& seconds >= 5
            && interactionWithMarko >= interactionWithLara
            && interactionWithMarko > 0*/)
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
        else if (action == 5 && once == false && (DistanceWithMarko < 1.42) /*&& seconds >= 5
            && interactionWithMarko >= interactionWithLara
            && interactionWithMarko > 0*/)
        {
            Food++;

            AddReward(+1.0f);
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Marko.Food--;
            once = true;
        }

        //Sharing Action
        if (action == 6 && DistanceWithMarko <= 1.42f && seconds >= 5 && interactionWithMarko <= interactionWithLara)
        {
            this.Food -= 0.5f;
            Marko.Food += 0.5f;
            AddReward(1f);
            if (this.FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            if (Marko.FoodFiller.size.x <= 1)
            {
                Marko.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }

        }
        else if (action == 6 && distanceWithLara <= 1.42f && seconds >= 5 && interactionWithMarko <= interactionWithLara)
        {
            this.Food -= 0.5f;
            Marko.Food += 0.5f;
            AddReward(1f);
            if (this.FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            if (Marko.FoodFiller.size.x <= 1)
            {
                Marko.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
        }

        //Attack Agent Animation
        //if (Input.GetKeyDown(KeyCode.A))
        if (Marko.action == 7 && Vector3.Distance(this.transform.position, Marko.transform.position) < 3)
        {
            transform.LookAt(Marko.transform.position);
            AnimZombie.SetTrigger("attack");
            bulletfire.ShootBullet(AttackParticle, Player, ParticlesContainer);
            Food--;
            if (FoodFiller.size.x > 0)
            {
                FoodFiller.size = new Vector2(FoodFiller.size.x - 0.02f, FoodFiller.size.y);
            }


        }

    }
    private void FixedUpdate()
    {

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

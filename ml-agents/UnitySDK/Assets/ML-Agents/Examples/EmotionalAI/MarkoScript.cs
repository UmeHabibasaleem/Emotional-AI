using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLAgents;
using System.Text;
using System.IO;

public class MarkoScript : Agent
{

    public static List<string[]> rowData = new List<string[]>();
    public static int markofile = 1;
    public Animator AnimZombie;
    public float Timepassed;
    public int idle, move, Eat, Share, Steal = 0;

    public Lara Lara;
    public GameObject Marko;
    public AgentMove Hallo;

    float speed = 5.0f;
    public float Food;
    public float PrevFood = 5;
    public int action;
    public int seconds = 0;
    public int i = 0;
    public int oneSecondCounter = 1;
    public bool FoodEaten = true;
    

    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;

    public SpriteRenderer FoodFiller;
    public SpriteRenderer HealthFiller;

    public float dist;
    public float Health;

    
   
    public GameObject TopContainer;
    public GameObject BottomContainer;
    public GameObject gun;
    public int Agentid = 2;
    
    public float FoodZerotimeSec = 0;
    public int FoodZerotime = 0;
    public BulletFire bulletfire;

    public Vector3 AgentStartingPos;
    private GameAcademy academy;
    private void Awake()
    {
        Physics.IgnoreLayerCollision(11, 11);
    }

    public override void CollectObservations()
    {
        AddVectorObs(Agentid);//1

        AddVectorObs(Timepassed);//1
        AddVectorObs(Lara.transform.position);//3
        AddVectorObs(this.transform.position);//3
        AddVectorObs(Hallo.transform.position);//3
        AddVectorObs(Food1.transform.position);//3
        AddVectorObs(Food2.transform.position);//3
        AddVectorObs(Food3.transform.position);//3

        AddVectorObs(Food);//1
        AddVectorObs(Health);//1
         AddVectorObs(speed);//1
    }
    // Use this for initialization
    public override void InitializeAgent()
    {
        academy = FindObjectOfType(typeof(GameAcademy)) as GameAcademy;
        Timepassed = 0;
        Food = 5;
        oneSecondCounter = 1;
        Health = 5;
        PrevFood = 5;
        AnimZombie = GetComponent<Animator>();
        bulletfire = new BulletFire();
        AgentStartingPos = this.transform.position;
        //string[] rowDataTemp = new string[23];
        //rowDataTemp[0] = "Agentid";
        //rowDataTemp[1] = "TimePassed";
        //rowDataTemp[2] = "LaraPosition";
        //rowDataTemp[3] = "MarkoPosition";
        //rowDataTemp[4] = "Food1Position";
        //rowDataTemp[5] = "Food2Position";
        //rowDataTemp[6] = "Food3Position";
        //rowDataTemp[7] = "HalloPosition";
        //rowDataTemp[8] = "Food";
        //rowDataTemp[9] = "Health";
        //rowDataTemp[10] = "Coin1Position";
        //rowDataTemp[11] = "Coin2Position";
        //rowDataTemp[12] = "Coin3Position";
        //rowDataTemp[13] = "Coin4Position";
        //rowDataTemp[14] = "Aidkit1Position";
        //rowDataTemp[15] = "Aidkit2Position";
        //rowDataTemp[16] = "AttackParticlePosition";
        //rowDataTemp[17] = "ParticleContainPosition";
        //rowDataTemp[18] = "Speed";
        //rowDataTemp[19] = "Coin";
        //rowDataTemp[20] = "CoinSeconds";
        //rowDataTemp[21] = "AgentAnimePosition";
        //rowDataTemp[22] = "Action";
        //rowData.Add(rowDataTemp);
        string[] rowDataTemp = new string[30];
        rowDataTemp[0] = "Agentid";
        rowDataTemp[1] = "TimePassed";
        rowDataTemp[2] = "LaraPosition.x";
        rowDataTemp[3] = "LaraPosition.y";
        rowDataTemp[4] = "LaraPosition.z";
        rowDataTemp[5] = "MarkoPosition.x";
        rowDataTemp[6] = "MarkoPosition.y";
        rowDataTemp[7] = "MarkoPosition.z";
        rowDataTemp[8] = "HalloPosition.x";
        rowDataTemp[9] = "HalloPosition.y";
        rowDataTemp[10] = "HalloPosition.z";
        rowDataTemp[11] = "Food1Position.x";
        rowDataTemp[12] = "Food1Position.y";
        rowDataTemp[13] = "Food1Position.z";
        rowDataTemp[14] = "Food2Position.x";
        rowDataTemp[15] = "Food2Position.y";
        rowDataTemp[16] = "Food2Position.z";
        rowDataTemp[17] = "Food3Position.x";
        rowDataTemp[18] = "Food3Position.y";
        rowDataTemp[19] = "Food3Position.z";
        rowDataTemp[20] = "Food";
        rowDataTemp[21] = "Health";
        rowDataTemp[22] = "AttackParticlePosition.x";
        rowDataTemp[23] = "AttackParticlePosition.y";
        rowDataTemp[24] = "AttackParticlePosition.z";
        rowDataTemp[25] = "ParticleContainPosition.x";
        rowDataTemp[26] = "ParticleContainPosition.y";
        rowDataTemp[27] = "ParticleContainPosition.z";
        rowDataTemp[28] = "Speed";
        rowDataTemp[29] = "Action";
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

            Food = 0;
            Health = 0;
            this.HealthFiller.size = new Vector2(0f, this.HealthFiller.size.y);
            Hallo.SetReward(-1f);
            // Lara.SetReward(-1f);
            Marko.SetActive(false);
            TopContainer.SetActive(false);
            BottomContainer.SetActive(false);
            gun.SetActive(false);
            this.enabled = false;
            FoodEaten = true;

            //FoodZerotimeSec += Time.deltaTime;
            //FoodZerotime = (int)FoodZerotimeSec;
            //if (FoodZerotime == 2)
            //{
   
            //}
        }

        //After one second
        if (seconds == oneSecondCounter)
        {
            oneSecondCounter += 1;
           
            Food = Food - 0.75f;
            if (FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            
            FoodEaten = false;
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

        if (action == 5 && (distWithLara < 1.42))
        {
            AddReward(+1.0f);
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
            //More reward on stealing from other agents
        }
        //Stealing from Hallo
        else if (action == 5 && (distWithHallo < 1.42))
        {
            Steal++;
            Food++;
            AddReward(+1.0f);
            if (FoodFiller.size.x < 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            Hallo.Food--;
            if (Hallo.FoodFiller.size.x > 0)
            {
                Hallo.FoodFiller.size = new Vector2(Hallo.FoodFiller.size.x - 0.02f, Hallo.FoodFiller.size.y);
            }
        }

        else if (action == 5 && !FoodEaten && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42))
        {
            AddReward(+0.2f);
            Eat++;
            FoodEaten = true;
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
        }
        //Share with Hallo
      
       
        if (action == 6 && distWithHallo <= 1.42f)
        {
            //Selfish agent should not share food
        
            if (Food > 0)
            {
                this.Food -= 0.5f;
                Hallo.Food += 0.5f;
                SetReward(-1f);
                Share++;
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
             if (Food > 0)
            {
                SetReward(-1f);
                Share++;
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
        //if (Hallo.action == 7 && Vector3.Distance(this.transform.position, Hallo.transform.position) < 3)
        //{
        //    if (Food > 0)
        //    {
        //        Food--;
        //    }
        //    Hallo.Attack++;
        //    Hallo.transform.LookAt(this.transform.position);
        //    Hallo.AnimZombie.SetTrigger("attack");
        //    Hallo.bulletfire.ShootBullet(AttackParticle, Marko, ParticlesContainer);
        //    if (FoodFiller.size.x > 0)
        //    {
        //        this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
        //    }
        //}


        ////If Attacked by Lara
        //if (Lara.action == 7 && Vector3.Distance(this.transform.position, Hallo.transform.position) < 3)
        //{
        //    Lara.AddReward(-0.5f);
        //    Lara.Attack++;
        //    if (Food > 0)
        //    {
        //        Food--;
        //    }
        //    Lara.transform.LookAt(this.transform.position);
        //    Lara.AnimZombie.SetTrigger("attack");
        //    Lara.bulletfire.ShootBullet(AttackParticle, Marko, ParticlesContainer);
        //    if (FoodFiller.size.x > 0)
        //    {
        //        this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
        //    }
        //}

        //string[] rowDataTemp = new string[23];
        //rowDataTemp[0] = Agentid.ToString();
        //rowDataTemp[1] = Timepassed.ToString();
        //rowDataTemp[2] = this.transform.position.ToString();
        //rowDataTemp[3] = Marko.transform.position.ToString();
        //rowDataTemp[4] = Food1.transform.position.ToString();
        //rowDataTemp[5] = Food2.transform.position.ToString();
        //rowDataTemp[6] = Food3.transform.position.ToString();
        //rowDataTemp[7] = Hallo.transform.position.ToString();
        //rowDataTemp[8] = Food.ToString();
        //rowDataTemp[9] = Health.ToString();
        //  rowDataTemp[16] = AttackParticle.transform.position.ToString();
        //rowDataTemp[17] = ParticlesContainer.transform.position.ToString();
        //rowDataTemp[18] = speed.ToString();
        // rowDataTemp[21] = AnimZombie.transform.position.ToString();
        //rowDataTemp[22] = action.ToString();
        //rowData.Add(rowDataTemp);
        string[] rowDataTemp = new string[30];
        rowDataTemp[0] = Agentid.ToString();
        rowDataTemp[1] = Timepassed.ToString();
        rowDataTemp[2] = this.transform.position.x.ToString();
        rowDataTemp[3] = this.transform.position.y.ToString();
        rowDataTemp[4] = this.transform.position.z.ToString();
        rowDataTemp[5] = Marko.transform.position.x.ToString();
        rowDataTemp[6] = Marko.transform.position.y.ToString();
        rowDataTemp[7] = Marko.transform.position.z.ToString();
        rowDataTemp[8] = Hallo.transform.position.x.ToString();
        rowDataTemp[9] = Hallo.transform.position.y.ToString();
        rowDataTemp[10] = Hallo.transform.position.z.ToString();
        rowDataTemp[11] = Food1.transform.position.x.ToString();
        rowDataTemp[12] = Food1.transform.position.y.ToString();
        rowDataTemp[13] = Food1.transform.position.z.ToString();
        rowDataTemp[14] = Food2.transform.position.x.ToString();
        rowDataTemp[15] = Food2.transform.position.y.ToString();
        rowDataTemp[16] = Food2.transform.position.z.ToString();
        rowDataTemp[17] = Food3.transform.position.x.ToString();
        rowDataTemp[18] = Food3.transform.position.y.ToString();
        rowDataTemp[19] = Food3.transform.position.z.ToString();

        rowDataTemp[20] = Food.ToString();
        rowDataTemp[21] = Health.ToString();
        //rowDataTemp[22] = AttackParticle.transform.position.x.ToString();
        //rowDataTemp[23] = AttackParticle.transform.position.y.ToString();
        //rowDataTemp[24] = AttackParticle.transform.position.z.ToString();
        //rowDataTemp[25] = ParticlesContainer.transform.position.x.ToString();
        //rowDataTemp[26] = ParticlesContainer.transform.position.y.ToString();
        //rowDataTemp[27] = ParticlesContainer.transform.position.z.ToString();
        rowDataTemp[28] = speed.ToString();
        rowDataTemp[29] = action.ToString();
        rowData.Add(rowDataTemp);

     //   SaveData(rowData, markofile);

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
    public void SaveData(List<string[]> rowData, int counter)
    {
        string[][] output = new string[rowData.Count][];
        Debug.Log(rowData.Count);
        for (int i = 0; i < output.Length; i++)
        {
            output[i] = rowData[i];
        }

        int length = output.GetLength(0);
        Debug.Log(length);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));

        string filePath = "E:/" + "/DATACSV1/" + counter + "MarkoSaved_data.csv";
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();

        counter++;
        markofile = counter;

    }

}

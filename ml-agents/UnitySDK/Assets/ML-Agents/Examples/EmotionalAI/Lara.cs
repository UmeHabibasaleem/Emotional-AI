using System.Collections;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using MLAgents;
using System.Text;
using System.IO;
//SELFLESS

public class Lara : Agent
{

    public static List<string[]> rowData = new List<string[]>();
    public static int larafile = 1;
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
        AddVectorObs(Agentid);//1
        AddVectorObs(Timepassed);//1
        AddVectorObs(this.transform.position);//3
        AddVectorObs(Marko.transform.position);//3
        AddVectorObs(Hallo.transform.position);//3
        AddVectorObs(Food1.transform.position);//3
        AddVectorObs(Food2.transform.position);//3
        AddVectorObs(Food3.transform.position);//3
       
        AddVectorObs(Food);//1
        AddVectorObs(Health);//1
        AddVectorObs(AttackParticle.transform.position);//3
        AddVectorObs(ParticlesContainer.transform.position);//3
        AddVectorObs(speed);//1

    }

    public override void AgentAction(float[] vectorAction, string txt ) 
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
            AddReward(-0.05f);
            string[] rowDataTemp1 = new string[30];
            rowDataTemp1[0] = Agentid.ToString();
            rowDataTemp1[1] = Timepassed.ToString();
            rowDataTemp1[2] = this.transform.position.x.ToString();
            rowDataTemp1[3] = this.transform.position.y.ToString();
            rowDataTemp1[4] = this.transform.position.z.ToString();
            rowDataTemp1[5] = Marko.transform.position.x.ToString();
            rowDataTemp1[6] = Marko.transform.position.y.ToString();
            rowDataTemp1[7] = Marko.transform.position.z.ToString();
            rowDataTemp1[8] = Hallo.transform.position.x.ToString();
            rowDataTemp1[9] = Hallo.transform.position.y.ToString();
            rowDataTemp1[10] = Hallo.transform.position.z.ToString();
            rowDataTemp1[11] = Food1.transform.position.x.ToString();
            rowDataTemp1[12] = Food1.transform.position.y.ToString();
            rowDataTemp1[13] = Food1.transform.position.z.ToString();
            rowDataTemp1[14] = Food2.transform.position.x.ToString();
            rowDataTemp1[15] = Food2.transform.position.y.ToString();
            rowDataTemp1[16] = Food2.transform.position.z.ToString();
            rowDataTemp1[17] = Food3.transform.position.x.ToString();
            rowDataTemp1[18] = Food3.transform.position.y.ToString();
            rowDataTemp1[19] = Food3.transform.position.z.ToString();

            rowDataTemp1[20] = Food.ToString();
            rowDataTemp1[21] = Health.ToString();
            rowDataTemp1[22] = AttackParticle.transform.position.x.ToString();
            rowDataTemp1[23] = AttackParticle.transform.position.y.ToString();
            rowDataTemp1[24] = AttackParticle.transform.position.z.ToString();
            rowDataTemp1[25] = ParticlesContainer.transform.position.x.ToString();
            rowDataTemp1[26] = ParticlesContainer.transform.position.y.ToString();
            rowDataTemp1[27] = ParticlesContainer.transform.position.z.ToString();
            rowDataTemp1[28] = speed.ToString();
            rowDataTemp1[29] = action.ToString();
            rowData.Add(rowDataTemp1);
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
         
            if (distWithMarko <= 1.42f)
            {
                Share++;
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
          
            if (distWithHallo <= 1.42f)
            {
                Share++;

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
        if (action!=5)
        { 
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
        rowDataTemp[22] = AttackParticle.transform.position.x.ToString();
        rowDataTemp[23] = AttackParticle.transform.position.y.ToString();
        rowDataTemp[24] = AttackParticle.transform.position.z.ToString();
        rowDataTemp[25] = ParticlesContainer.transform.position.x.ToString();
        rowDataTemp[26] = ParticlesContainer.transform.position.y.ToString();
        rowDataTemp[27] = ParticlesContainer.transform.position.z.ToString();
        rowDataTemp[28] = speed.ToString();
        rowDataTemp[29] = action.ToString();
        rowData.Add(rowDataTemp);
        if (larafile == 100)
        {
           SaveData(rowData, larafile);
        }
     }
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

        if (Food <= 0)
        {
            Food = 0;
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

        string filePath = "E:/" + "/DATACSV1/" + counter + "LaraSaved_data.csv";
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();

        counter++;
        larafile = counter;

    } 

}
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AgentMove : MonoBehaviour {
    Animator AnimZombie;
    public float Timepassed;
    public float Timecheck;
    public Lara Lara;
    float speed = 5.0f;
    public float Food;
    public float Health;
    public int action;
    int seconds = 0;
    int i = 0;
    int count = 0;
    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;
    Random random = new Random();
    public SpriteRenderer FoodFiller;
    public SpriteRenderer HealthFiller;
    public GameObject Hallo;
    float PrevFood = 10;
    float Pfood;
    bool once = false;
    bool healthinc;
    public float dist;
    public MarkoScript Marko;
    public GameObject Coin1;
    public GameObject Coin2;
    public GameObject Coin3;
    public GameObject Coin4;
    public GameObject AIDkit1;
    public GameObject AIDkit2;
    public float Cointime = 0;
    public int Coinseconds;
    Coin coin;
    bool Attack= false;
    BulletFire bulletfire;
    
    FirstAidKit Aidkit;
    public int numberofCoins = 0;
    
   
    // Use this for initialization
    void Start()
    {
        Timepassed = 0;
        Timecheck = 0;
        Food = 10;
        Health = 15;
        healthinc = false;
        AnimZombie = GetComponent<Animator>();
        Food3.SetActive(false);
        coin = new Coin();
        bulletfire = new BulletFire();
        Aidkit = new FirstAidKit();
    }

    // Update is called once per frame
    void Update()
    {

        Vector3 targetPos = AnimZombie.transform.position;
        targetPos.y = 0;
        Timepassed += Time.deltaTime;
        Timecheck += Time.deltaTime;
        seconds = (int)Timepassed;

        if (seconds == count)
        {
            count += 1;
            action = Random.Range(0, 7);
            healthinc = false;
            once = false;

        }
        if (seconds == i)
        {
            Food = Food - 0.5f;
            this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            i += 2;

        }
        if (PrevFood - Food == 1)
        {
            Health -= 0.5f;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x - 0.02f, this.HealthFiller.size.y);
            PrevFood = Food;
          }
        

        float dist1 = Vector3.Distance(Hallo.transform.position, Food1.transform.position);
        float dist2 = Vector3.Distance(Hallo.transform.position, Food2.transform.position);
        float dist3 = Vector3.Distance(Hallo.transform.position, Food3.transform.position);

        if (action == 5 && once == false && (dist1 < 1.42 || dist2 < 1.42 || dist3 < 1.42))
        {
            Food++;
            this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            once = true;
            if(dist1 < 1.42)
            {
                Food1.SetActive(false);
                Timecheck = 0;

            }
            else if( dist2 < 1.42f)
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
        if((int)Timecheck == 5)
        {
            Food1.SetActive(true);
            Food2.SetActive(true);
            Food3.SetActive(true);

        }
       
        if(Food - PrevFood == 1 && healthinc == false)
        {
            Health += 0.5f;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            healthinc = true;
        }
        //Run Agent Animation 
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.A))
        // if (action == 1 || action == 2 || action == 3 || action == 4)
        {
            AnimZombie.SetTrigger("run");
            
        }

        //Stop Agent Animation
       if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A))
       // if(action == 0)
        {
            AnimZombie.SetTrigger("idle");
        }

        //Attack Agent Animation
        if (Input.GetKeyDown(KeyCode.A))
       // if (action == 7)
        {
            AnimZombie.SetTrigger("attack");
            
        }
        if (Vector3.Distance(this.transform.position, Marko.transform.position) < 3)
        {
            Debug.Log("Marko calling");
            AnimZombie.SetTrigger("attack");
           // bulletfire.fire();
            // bulletfire.check = true;

            Marko.Food--;
            Marko.FoodFiller.size = new Vector2(Marko.FoodFiller.size.x - 0.02f, Marko.FoodFiller.size.y);

        }
        if (Vector3.Distance(this.transform.position, Lara.transform.position) < 3)
        {
            AnimZombie.SetTrigger("attack");
            //bulletfire.check = true;
           // bulletfire.fire();
            Lara.Food--;
            Lara.FoodFiller.size = new Vector2(Lara.FoodFiller.size.x - 0.02f, Lara.FoodFiller.size.y);
        }
        float DistanceWithMarko = Vector3.Distance(this.transform.position, Marko.transform.position);

        if (action == 6 && DistanceWithMarko <= 1.42f)
        {
            this.Food -= 0.5f;
            Marko.Food += 0.5f;
            if(this.FoodFiller.size.x > 0)
            { 
            this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            if (Marko.FoodFiller.size.x <= 1)
            {
                Marko.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
        }
        Cointime = Cointime + Time.deltaTime;
        Coinseconds = (int)Cointime;
        bool Coincheck = coin.coin_production(Coinseconds,Hallo, Coin1, Coin2, Coin3, Coin4);
        bool check = Aidkit.AIDKIT(Coinseconds, Hallo, AIDkit1, AIDkit2);
        if(check == true)
        {
            Health += 0.5f;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.004f, this.HealthFiller.size.y);
        }
        if (Coincheck == true)
        {
            ++numberofCoins;
        }

        if (Coinseconds == 5)
        {
            Cointime = 0;
            Coinseconds = 0;
        }

    }
    private void FixedUpdate()
    {

        //Move PLayer

        if (Input.GetKey(KeyCode.RightArrow))
            //Move Player forward
      //  if (action == 1)
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.left);
            transform.position += Vector3.left * Time.deltaTime * speed;
        }

        if (Input.GetKey(KeyCode.UpArrow))
       // if (action == 2)

        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.forward);
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }

        //Move Player Backward
         if (Input.GetKey(KeyCode.DownArrow))
       // if (action == 3)
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.back);
            transform.position -= Vector3.forward * Time.deltaTime * speed;
        }

        //Move Player left
        if (Input.GetKey(KeyCode.LeftArrow))
        
        //Move Player Right
        //if (action == 4)
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.right);
            transform.position -= Vector3.left * Time.deltaTime * speed;
        }
    }

}

using System.Collections;
using System.Collections.Generic;
using TMPro;

using UnityEngine;


public class MarkoScript : MonoBehaviour
{
    Animator AnimZombie;
    public float Timepassed;
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
    public GameObject Marko;
    float PrevFood = 10;
    float Pfood;
    bool once = false;
    bool healthinc;
    public float dist;
    public AgentMove Hallo;
    public float Dopamin = 1;
    public float OxetocinForHallo = 2;
    public float OxetocinForLara = 2;
    public float Reward = 0;
    ActionList l = new ActionList();
    // Use this for initialization
    void Start()
    {
        Timepassed = 0;
        Food = 10;
        Health = 15;
        healthinc = false;
        AnimZombie = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 targetPos = AnimZombie.transform.position;
        Timepassed += Time.deltaTime;
        seconds = (int)Timepassed;

        if (seconds == count)
        {
            count += 1;

            action = Random.Range(0, 7);
            healthinc = false;
            once = false;

        }
        //after two seconds
        if (seconds == i)
        {
            Food = Food - 0.5f;
            if (FoodFiller.size.x > 0 )
            {
               this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            i += 2;

        }
        if (PrevFood - Food == 1)
        {
            Health -= 0.5f;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x - 0.02f, this.HealthFiller.size.y);
            PrevFood = Food;
        }


        float dist1 = Vector3.Distance(Marko.transform.position, Food1.transform.position);
        dist = dist1;

        if (action == 5 && once == false /*&& dist1 < 1.42*/)
        {
            Food++;
            this.Dopamin += 0.5f;
            if (FoodFiller.size.x <= 1)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
            once = true;


        }
        if (Food - PrevFood == 1 && healthinc == false)
        {
            Health++;
            this.HealthFiller.size = new Vector2(this.HealthFiller.size.x + 0.02f, this.HealthFiller.size.y);
            healthinc = true;
        }
        //Run Agent Animation 

        if (action == 1 || action == 2 || action == 3 || action == 4)
        {
            AnimZombie.SetTrigger("run");

        }

        //Stop Agent Animation
        //    if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A))
        if (action == 0)
        {
            AnimZombie.SetTrigger("idle");
        }

        //Attack Agent Animation
        if (action == 7)
        {
            AnimZombie.SetTrigger("attack");
        }

        if (Hallo.action == 7 && Vector3.Distance(this.transform.position, Hallo.transform.position) < 3)
        {
            Food--;
            this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            
        }
        if (Lara.action == 7 && Vector3.Distance(this.transform.position, Lara.transform.position) < 3)
        {
            Food--;
            this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
        }
        
        float DistanceWithMarko = Vector3.Distance(this.transform.position, Hallo.transform.position);

        if (action == 6 && DistanceWithMarko <= 1.42f)
        {
            this.Food -= 0.5f;
            Hallo.Food += 0.5f;
            Hallo.OxetocinForMarko += 0.5f;
            if (this.FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);
            }
            if (Hallo.FoodFiller.size.x <= 1)
            {
                Hallo.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
        }
        float DistanceWithLara = Vector3.Distance(this.transform.position, Lara.transform.position);

        if (action == 6 && DistanceWithLara <= 1.42f)
        {
            this.Food -= 0.5f;
            Lara.Food += 0.5f;
            Lara.OxetocinForMarko += 0.5f;
            if (this.FoodFiller.size.x > 0)
            {
                this.FoodFiller.size = new Vector2(this.FoodFiller.size.x - 0.02f, this.FoodFiller.size.y);

            }
            if (Lara.FoodFiller.size.x <= 1)
            {
                Lara.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
            }
        }
        AddReward(Selfish());

    }

    public void AddReward(float Reward)
    {
        this.Reward += Reward;
    }

    public void SetReward(float Reward)
    {
        this.Reward = Reward;
    }

    public int Selfish()
    {
        if (this.Dopamin > 5 && Lara.OxetocinForMarko < 4 && Hallo.OxetocinForMarko < 4)
        {
            return 1;
        }
        return 0;
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

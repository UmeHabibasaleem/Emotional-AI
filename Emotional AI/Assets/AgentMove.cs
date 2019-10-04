using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AgentMove : MonoBehaviour {
    Animator AnimZombie;
    public float Timepassed;
    float speed = 5.0f;
    public float Food;
    public float Health;
    int action;
    int seconds = 0;
    int i = 0;
    int count = 0;
    Random random = new Random();
    public SpriteRenderer FoodFiller;
    public SpriteRenderer HealthFiller;
    float PrevFood = 10;
    // Use this for initialization
    void Start()
    {
        Timepassed = 0;
        Food = 10;
        Health = 15;
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

            action = Random.Range(0, 4);

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
        //Run Agent Animation 
        
        if (action == 1 || action == 2 || action == 3 || action == 4)
        {
            AnimZombie.SetTrigger("run");
            
        }

        //Stop Agent Animation
        //    if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A))
        if(action == 0)
        {
            AnimZombie.SetTrigger("idle");
        }

        //Attack Agent Animation
        if (Input.GetKeyDown(KeyCode.A))
        {
            AnimZombie.SetTrigger("attack");
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

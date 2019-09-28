using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AgentMove : MonoBehaviour {
    Animator AnimZombie;
    float speed = 5.0f;
    public float food;
  
    // Use this for initialization
    void Start()
    {
        food = 10;
        AnimZombie = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        //Run Agent Animation 
        food -= 0.5f;
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            AnimZombie.SetTrigger("run");
            
        }

        //Stop Agent Animation
        if (Input.GetKeyUp(KeyCode.UpArrow) || Input.GetKeyUp(KeyCode.DownArrow) || Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow) || Input.GetKeyUp(KeyCode.A))
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
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.forward);
            transform.position += Vector3.forward * Time.deltaTime * speed;
        }

        //Move Player Backward
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.back);
            transform.position -= Vector3.forward * Time.deltaTime * speed;
        }

        //Move Player left
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.left);
            transform.position += Vector3.left * Time.deltaTime * speed;
        }

        //Move Player Right
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.GetComponent<Rigidbody>().rotation = Quaternion.LookRotation(Vector3.right);
            transform.position -= Vector3.left * Time.deltaTime * speed;
        }
    }

}

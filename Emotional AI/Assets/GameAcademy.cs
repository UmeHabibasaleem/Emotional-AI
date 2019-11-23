using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
public class GameAcademy : Academy {

    Animator AnimZombie;
    public float Timepassed;
    public Lara Lara;
    public MarkoScript Marko;
    public float MarkoFood;
    public float MarkoHealth;
    public AgentMove Hallo;
    public float HalloFood;
    public float HalloHealth;
    public float LaraFood;
    public float LaraHealth;
    int seconds = 0;
   // public PythonCommunicator py;
    int i = 0;
    int count = 0;
    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;
   
    public override void InitializeAcademy() {
        MarkoFood = Marko.Food;
        MarkoHealth = Marko.Health;
        LaraFood = Lara.Food;
        LaraHealth = Lara.Health;
        HalloFood = Hallo.Food;
        HalloHealth = Hallo.Health;
    }
    public void SetEnvironment()
    { }
    public override void AcademyReset()
    { }

    public override void AcademyStep()
    {
        Timepassed += Time.deltaTime;
        seconds = (int)Timepassed;

        if (seconds == 10)
        {
           Done();        
        }
    }
    }


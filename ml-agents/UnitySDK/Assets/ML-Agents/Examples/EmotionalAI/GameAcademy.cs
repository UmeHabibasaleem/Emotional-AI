﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System.Text;
using System.IO;
public class GameAcademy : Academy
{
    Animator AnimZombie;
    public static int larafile = 1;
    public static int markofile = 11;
    public static int moveagentfile = 21;
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
    public GameObject MarkoAgent;
    public GameObject LaraAgent;
    public GameObject HalloAgent;
    int seconds = 0;
    // public PythonCommunicator py;
    int i = 0;
    int count = 0;
    public GameObject Food1;
    public GameObject Food2;
    public GameObject Food3;

    public override void InitializeAcademy()
    {
        MarkoFood = Marko.Food;
        MarkoHealth = Marko.Health;
        LaraFood = Lara.Food;
        LaraHealth = Lara.Health;
        HalloFood = Hallo.Food;
        HalloHealth = Hallo.Health;
    }
    /*public void SetEnvironment()
    { } */
    public override void AcademyReset()
    {
        this.Timepassed = 0;
        this.seconds = 0;
        MarkoAgent.SetActive(true);
        LaraAgent.SetActive(true);
        HalloAgent.SetActive(true);

        Marko.enabled = true;
        Lara.enabled = true;
        Hallo.enabled = true;

        Marko.FoodFiller.size = new Vector2(1f, Marko.FoodFiller.size.y);
        Marko.HealthFiller.size = new Vector2(1f, Marko.HealthFiller.size.y);
        Marko.TopContainer.SetActive(true);
        Marko.BottomContainer.SetActive(true);
        Marko.gun.SetActive(true);


        Lara.FoodFiller.size = new Vector2(1f, Lara.FoodFiller.size.y);
        Lara.HealthFiller.size = new Vector2(1f, Lara.HealthFiller.size.y);
        Lara.TopContainer.SetActive(true);
        Lara.BottomContainer.SetActive(true);
        Lara.gun.SetActive(true);

        Hallo.FoodFiller.size = new Vector2(1f, Hallo.FoodFiller.size.y);
        Hallo.HealthFiller.size = new Vector2(1f, Hallo.HealthFiller.size.y);
        Hallo.TopContainer.SetActive(true);
        Hallo.BottomContainer.SetActive(true);
        Hallo.gun.SetActive(true);


        Lara.count = 1;
        Marko.oneSecondCounter = 1;
        Hallo.count = 0;

        Marko.Timepassed = 0;
        Lara.Timepassed = 0;
        Hallo.Timepassed = 0;

        Marko.Food = 10;
        Lara.Food = 10;
        Hallo.Food = 10;

        Marko.Health = 10;
        Lara.Health = 10;
        Hallo.Health = 10;


        Hallo.healthinc = false;

        //Food3.SetActive(false);

        Marko.numberofCoins = 0;
        Lara.numberofCoins = 0;
        Hallo.numberofCoins = 0;
        //Dopamin = 1;
        //OxetocinForHallo = 2;
        //OxetocinForLara = 2;
        Marko.healthKit = 0;
        Lara.healthKit = 0;
        Hallo.healthKit = 0;

        Marko.seconds = 0;
        Lara.seconds = 0;
        Hallo.seconds = 0;

        //  Marko.i = 0;
        //   Lara.i = 0;
        Hallo.i = 0;


        Hallo.count = 0;

        Marko.Cointime = 0;
        Lara.Cointime = 0;
        Hallo.Cointime = 0;

        Marko.PrevFood = 10;
        Lara.PrevFood = 10;
        Hallo.PrevFood = 10;


        Hallo.once = false;
        //OxetocinInHalloForMarko = 0;
        //OxetocinInLaraForMarko = 0;

        //this.FoodFiller.size = new Vector2(this.FoodFiller.size.x + 0.02f, this.FoodFiller.size.y);
        Marko.transform.position = Marko.AgentStartingPos;
        Lara.transform.position = Lara.AgentStartingPos;
        Hallo.transform.position = Hallo.AgentStartingPos;

        Marko.FoodZerotimeSec = 0;
        Lara.FoodZerotimeSec = 0;
        Hallo.FoodZerotimeSec = 0;

        Marko.FoodZerotime = 0;
        Lara.FoodZerotime = 0;
        Hallo.FoodZerotime = 0;



    }

    public override void AcademyStep()
    {
        Timepassed += Time.deltaTime;
        seconds = (int)Timepassed;

        if (seconds == 10)
        {
            Lara.Done();
            Marko.Done();
            Hallo.Done();
            AcademyReset();
            markofile++;
            larafile++;

        }


    }

}


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MLAgents;
using System.Text;
using System.IO;
public class GameAcademy : Academy {

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
   
    public override void InitializeAcademy() {
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
        MarkoAgent.SetActive(true);
        LaraAgent.SetActive(true);
        HalloAgent.SetActive(true);

        Marko.Timepassed = 0;
        Lara.Timepassed = 0;
        Hallo.Timepassed = 0;

        Marko.Food = 10;
        Lara.Food = 10;
        Hallo.Food = 10;

        Marko.Health = 10;
        Lara.Health = 10;
        Hallo.Health = 10;

        Marko.healthinc = false;
        Lara.healthinc = false;
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

        Marko.i = 0;
        Lara.i = 0;
        Hallo.i = 0;

        Marko.count = 0;
        Lara.count = 0;
        Hallo.count = 0;

        Marko.Cointime = 0;
        Lara.Cointime = 0;
        Hallo.Cointime = 0;

        Marko.PrevFood = 10;
        Lara.PrevFood = 10;
        Hallo.PrevFood = 10;

        Marko.once = false;
        Lara.once = false;
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
        SaveData(MarkoScript.rowData, markofile);
        SaveData(Lara.rowData , larafile);
        markofile++;
        larafile++;


    }

    public override void AcademyStep()
    {
        Timepassed += Time.deltaTime;
        seconds = (int)Timepassed;

        if (seconds == 10)
        {
           Done();        
        }
    }

    public void SaveData(List<string[]> rowData , int counter)
    {
        string[][] output = new string[rowData.Count][];

        for (int i = 0; i < output.Length; i++)
        {
            output[i] = Lara.rowData[i];
        }

        int length = output.GetLength(0);
        string delimiter = ",";

        StringBuilder sb = new StringBuilder();

        for (int index = 0; index < length; index++)
            sb.AppendLine(string.Join(delimiter, output[index]));


        string filePath = "C:/" + "/CSV/" + counter+"Saved_data.csv";
        StreamWriter outStream = System.IO.File.CreateText(filePath);
        outStream.WriteLine(sb);
        outStream.Close();

    }
 }


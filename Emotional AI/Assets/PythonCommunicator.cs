using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
public class PythonCommunicator : MonoBehaviour
{
  /*  // Start is called before the first frame update
    public List<float> vectorObservation;
    private const int NoAction = 0;  // do nothing!
    private const int Up = 1;
    private const int Down = 2;
    private const int Left = 3;
    private const int Right = 4;

    private const int Share = 5;
    // public static GameObject Cube;
    public GameObject Cube1;
    
    // Start is called before the first frame update
    /* static void Main(string[] args)
     {
         Console.WriteLine("Execute python process...");
         Update();


     }
    void Start()
    {


    }


    // Update is called once per frame
    string  Update()
    {
        Vector3 targetPos = this.transform.position;
        Vector3 targetPos1 = Cube1.transform.position;
        Vector3 position1 = this.transform.position;
        vectorObservation.Add(position1.x);
        vectorObservation.Add(position1.y);
        vectorObservation.Add(position1.z);
        vectorObservation.Add(Cube1.transform.position.x);
        vectorObservation.Add(Cube1.transform.position.y);
        vectorObservation.Add(Cube1.transform.position.z);
        float var1 = position1.x;
        float var2 = position1.y;
        float var3 = position1.z;
        float var4 = Cube1.transform.position.x;
        float var5 = Cube1.transform.position.y;
        float var6 = Cube1.transform.position.z;


        //vectorObservation.Add(Target.position.x);
        //vectorObservation.Add(Target.position.y);
        //vectorObservation.Add(Target.position.z);
        //vectorObservation.Add(Needy.position.x);
        //vectorObservation.Add(Needy.position.y);
        //vectorObservation.Add(Needy.position.z);
        //vectorObservation.Add(Selfish.position.x);
        //vectorObservation.Add(Selfish.position.y);
        //vectorObservation.Add(Selfish.position.z);
        //// Agent velocity
        //vectorObservation.Add(rBody.velocity.x);
        //vectorObservation.Add(rBody.velocity.y);
        //vectorObservation.Add(rBody.velocity.z);

        //switch (action)
        //{
        //    case NoAction:

        //        break;
        //    case Right:
        //        targetPos = transform.position + new Vector3(1f, 0, 0f);
        //        break;
        //    case Left:
        //        targetPos = transform.position + new Vector3(-1f, 0, 0f);
        //        break;
        //    case Up:
        //        targetPos = transform.position + new Vector3(0f, 0, 1f);
        //        break;
        //    case Down:
        //        targetPos = transform.position + new Vector3(0f, 0, -1f);

        //        break;

        //    default:
        //        throw new ArgumentException("Invalid action value");

        var psi = new ProcessStartInfo();
        psi.FileName = @"C:\HinaProgramm\ml-agents\venv\Scripts\python.exe";

        // 2) Provide script and arguments
        var script = @"C:\HinaProgramm\ml-agents\gym_unity\envs\TestDQNMethod.py";
        // Vector observations
        var  vectorobserno = 7;
        var actionno = 5;
        //int end1 = 7;

        //psi.Arguments = $"\"{script}\" \"{var1}\" \"{var2}\" \"{var3}\" \"{var4}\" \"{var5}\" \"{var6}\"";
        psi.Arguments = $"\"{script}\" \"{vectorobserno}\" \"{actionno}\" \"{var1}\" \"{var2}\" \"{var3}\" \"{var4}\" \"{var5}\" \"{var6}\"";

        // 3) Process configuration
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;
        psi.RedirectStandardOutput = true;
        psi.RedirectStandardError = true;

        // 4) Execute process and get output
        var errors = "";
        var results = "";
        using (var process = Process.Start(psi))
        {
            errors = process.StandardError.ReadToEnd();
            results = process.StandardOutput.ReadToEnd();
            
            
        }
       
        int agent1act;
        int agent2act;
        int myInt;
        var array = results.ToCharArray().Where(x =>
        int.TryParse(x.ToString(), out myInt)).Select(x =>
        int.Parse(x.ToString())).ToArray();
        //result1 = Int32.Parse(results);
        // 5) Display output
        agent1act = array[0];
        agent2act = array[1];
        if (agent1act == 1)
        {
            targetPos = this.transform.position + new Vector3(1f, 0, 0f);
        }
        else if (agent1act == 2)
        {
            targetPos = this.transform.position + new Vector3(-1f, 0, 0f);
        }
        else if (agent1act == 3)
        {
            targetPos = this.transform.position + new Vector3(0f, 0, 1f);
        }
        else if (agent1act == 4)
        {
            targetPos = this.transform.position + new Vector3(0f, 0, -1f);
        }
        else
        {
            targetPos = this.transform.position;
        }

        Console.WriteLine("ERRORS:");
        Console.WriteLine(errors);
        Console.WriteLine();
        Console.WriteLine("Results:");
        Console.WriteLine(results);
        return results;

    } */
}

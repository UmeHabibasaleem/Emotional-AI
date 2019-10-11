using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

  
    
    
   public bool coin_production(int time,GameObject Hallo, GameObject Coin1, GameObject Coin2, GameObject Coin3, GameObject Coin4)
    {
        bool check = false;
        float CoinDist1 = Vector3.Distance(Hallo.transform.position, Coin1.transform.position);
        float CoinDist2 = Vector3.Distance(Hallo.transform.position, Coin2.transform.position);
        float CoinDist3 = Vector3.Distance(Hallo.transform.position, Coin3.transform.position);
        float CoinDist4 = Vector3.Distance(Hallo.transform.position, Coin4.transform.position);
        if (CoinDist1 < 1)
        {
            Coin1.SetActive(false);
            check = true;
        }
        if (CoinDist2 < 1)
        {
            Coin2.SetActive(false);
            check = true;
        }
        if (CoinDist3 < 1)
        {
            Coin3.SetActive(false);
            check = true;
        }
        if (CoinDist4 < 1)
        {
           Coin4.SetActive(false);
            check = true;
        }
        if (time == 5)
        {
            if (Coin1.active == false)
            {
               Coin1.SetActive(true);
            }
            if (Coin2.active == false)
            {
                Coin2.SetActive(true);
            }
            if (Coin3.active == false)
            {
                Coin3.SetActive(true);
            }
            if (Coin4.active == false)
            {
                Coin4.SetActive(true);
            }
        }
        return check;
      }
}
public class FirstAidKit : MonoBehaviour
{
    AgentMove agent = new AgentMove();
    public bool AIDKIT(int time, GameObject Hallo, GameObject AidKit1, GameObject AidKit2)
    {
        bool check = false;
        float KITDist1 = Vector3.Distance(Hallo.transform.position, AidKit1.transform.position);
        float KITDist2 = Vector3.Distance(Hallo.transform.position, AidKit2.transform.position);
        if (KITDist1 < 1)
        {
            AidKit1.SetActive(false);
            check = true;
        }
        if (KITDist2 < 1)
        {
            AidKit2.SetActive(false);
            check = true;
        }

        if (time == 5)
        {
            if (AidKit1.active == false)
            {
                AidKit1.SetActive(true);
            }
            if (AidKit2.active == false)
            {
                AidKit2.SetActive(true);
            }

        }
        return check;
    }
}

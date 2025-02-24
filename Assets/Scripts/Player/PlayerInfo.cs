using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int playerID;
    public string playerName;
    public int Deaths;
    public int Kills;
    public float KDRatio;
    public bool Winner;

    public void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        playerID = playerController.GetPlayerIndex();
        Deaths = 0;
        Kills = 0;
        KDRatio = 0;
        Winner = false;
        //el nombre del platyer sera el nombre del padre sin la palabra (Clone)
        playerName = transform.parent.name.Replace("(Clone)", "");

    }

    public void AddDeath()
    {
        Deaths++;
        CalculateKDRatio();
    }

    public void AddKill()
    {
        Kills++;
        CalculateKDRatio();
    }

    public void CalculateKDRatio()
    {
        if (Deaths == 0)
        {
            KDRatio = Kills;
        }
        else
        {
            KDRatio = Kills / Deaths;
        }
    }
}

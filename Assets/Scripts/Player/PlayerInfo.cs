using ProyectXAPI.Models;
using UnityEngine;

public class PlayerInfo : MonoBehaviour
{
    public int playerID;
    public Profile assignedProfile;
    public string playerName;
    public int Deaths;
    public int Kills;
    public float KDRatio;
    public bool Winner;

    public void Start()
    {
        PlayerController playerController = GetComponent<PlayerController>();
        playerID = playerController.GetPlayerIndex();
        if (PickProfileManager.SelectedProfiles.ContainsKey(playerID))
        {
            assignedProfile = PickProfileManager.SelectedProfiles[playerID];
        }
        
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
    public void FillSessionDataInfo(ref SessionData data)
    {
        data.Kills = Kills;
        data.Deaths = Deaths;
        data.Profile = assignedProfile;
        data.Profile.Creator = AcountManager.Session;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCExit : FCBehaviour
{
    public const string NAME = "Exit";
    List<GameObject> crewMembers;
    private bool exploring;
    private ShipBehaviour shipScript;
    private FCKitchenBehaviour kitchenScript;
    private FCNavigation navigationScript;
    private int timeInCurrentExpedition;


    // Start is called before the first frame update
    void Start()
    {
        crewMembers = new List<GameObject>();
        exploring = false;
        timeInCurrentExpedition = 0;
    }

    // Update is called once per frame
    void Update()
    {
      
    }

    public void setShip(ShipBehaviour shipBehaviour)
    {
        shipScript = shipBehaviour;
    }
    public void setKitchen(FCKitchenBehaviour fCKitchenBehaviour)
    {
        kitchenScript = fCKitchenBehaviour;
    }
    public void setNavigation(FCNavigation navigationScript)
    {
        this.navigationScript = navigationScript;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        CMBehaviour crewScript = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScript.getCurrentAction();
        if (exploring)
        {
            crewMembers.Add(crewMember);
            timeInCurrentExpedition = 0;

            navigationScript.setExpeditionStarted();

            InvokeRepeating("explore", 1f, 1f);

        }
        else
        {
            crewScript.orderDone();
            crewScript.setDoingAction(false);
        }
        
        

    }

    private void explore()
    {
        timeInCurrentExpedition += 1;
    }

    public void claimRewards()
    {
        exploring = false;
        Planet currentPlanet = navigationScript.getCurrentPlanet();
        float percentageOfRewardMax = (timeInCurrentExpedition * 1f) / 10 < 1 ? (timeInCurrentExpedition * 1f) / 10 : 1;
        float percentageOfRewardMin = percentageOfRewardMax - 0.3f > 0 ? percentageOfRewardMax - 0.3f : 0;

        float percentageOfReward = Random.Range(percentageOfRewardMin, percentageOfRewardMax);
        Dictionary<Resource, int> resources = shipScript.GetInventoryResources();

        foreach (var reward in currentPlanet.GetObtainableResources())
        {
            int totalReward = (int)(reward.Value * percentageOfReward);
            Resource resource = reward.Key;
            resources[resource] = resources[resource] + totalReward;

        }
        int foodReward = (int)(currentPlanet.GetFoodAvailable() * percentageOfReward);
        kitchenScript.addFood(foodReward);
        List<bool> willDie = new List<bool>(crewMembers.Count);
        int i = 0;
        foreach(var crewMember in crewMembers)
        {
            //int rand = Random.Range(0, 100);
            int rand = 100;
            int succes = timeInCurrentExpedition;
           
            if(rand < succes)
            {
                willDie[i] = true;
                crewMembers.Remove(crewMember);
                Destroy(crewMember);
            }
            i++;
        }
        i = 0;
        foreach(var die in willDie)
        {
            if (die)
            {
                var cM = crewMembers[i];
                crewMembers.RemoveAt(i);
                Destroy(cM);
            }
            i++;
        }
        CancelInvoke("explore");
    }

    public void startExpedition()
    {
        exploring = true;
        
    }

    public int getTime()
    {
        return timeInCurrentExpedition;
    }

    public string getContext()
    {
        
        return "";
    }
}

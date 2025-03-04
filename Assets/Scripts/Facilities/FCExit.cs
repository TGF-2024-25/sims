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

    private void claimRewards()
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
        foreach(var crewMember in crewMembers)
        {
            //int rand = Random.Range(0, 100);
            int rand = 0;
            int succes = timeInCurrentExpedition;
            if(rand < succes)
            {
                crewMembers.Remove(crewMember);
                Destroy(crewMember);
            }
        }
        CancelInvoke("explore");
    }

    public void startExpedition()
    {
        exploring = true;
        timeInCurrentExpedition = 0;
        InvokeRepeating("explore", 1f, 1f);

        Invoke("claimRewards", 30f);
    }

    public string getContext()
    {
        
        return "";
    }
}

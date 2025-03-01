using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCEngineBehaviour : FCBehaviour
{
    public const string NAME = "Engine";
    private const int MAX_FUEL_LEVEL = 150;
    private int fuelLevel;
    private int toInsert;
    private CMBehaviour crewScript;
    private bool colliding;

    // Start is called before the first frame update
    void Start()
    {
        fuelLevel = MAX_FUEL_LEVEL;
        toInsert = 0;
        colliding = false;
        InvokeRepeating(nameof(SubtractValue), 30f, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding && crewScript.getInFacility() && crewScript.getCurrentAction() != null && crewScript.getCurrentAction().correctFacility(NAME))
            startRefill();
    }

    void SubtractValue()
    {
        fuelLevel -= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        CMBehaviour crewScriptAux = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScriptAux.getCurrentAction();

        if (action.correctFacility(NAME))
        {
            crewScript = crewScriptAux;
            colliding = true;
            crewScript.setInFacility(true);

        }

    }

    void startRefill()
    {
        Debug.Log("started refill");
        RefillAction refillAction = (RefillAction)crewScript.getCurrentAction();
        string justEnough = refillAction.getJustEnough();
        int percentage = refillAction.getPercentage();
        if (justEnough.Equals("yes"))
        {
            toInsert = -1;
        }
        else
        {
            toInsert = percentage;
        }
        Invoke(nameof(refillMotor), 5f);
        crewScript.setInFacility(false);

    }
    void refillMotor()
    {
        Debug.Log("finish refill");
        if (toInsert >= 0)
        {
            fuelLevel = toInsert;
        }
        else
        {
            fuelLevel = MAX_FUEL_LEVEL;
        }
        crewScript.orderDone();
        crewScript.setDoingAction(false);
        crewScript.setInFacility(true);

    }

    public string getContext()
    {
        string context = "In the engine, the fuel level is at " + fuelLevel + " out of " + MAX_FUEL_LEVEL;
        return context;
    }

    public bool reduceFuel(int reduction)
    {
        if(reduction < fuelLevel)
        {
            fuelLevel -= reduction;
            return true;
        }
        else
        {
            return false;
        }

    }

    public override void OnClick()
    {
        Debug.Log("Click on Engine");
    }

    public int getFuel()
    {
        return fuelLevel;
    }
}

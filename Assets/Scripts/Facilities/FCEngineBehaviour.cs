using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCEngineBehaviour : FCBehaviour
{
    public const string NAME = "Engine";
    private const int MAX_FUEL_LEVEL = 100;
    private int fuelLevel;
    private int toInsert;
    private CMBehaviour crewScript;
    // Start is called before the first frame update
    void Start()
    {
        fuelLevel = 24;
        toInsert = 0;
        InvokeRepeating(nameof(SubtractValue), 30f, 30f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SubtractValue()
    {
        fuelLevel -= 1;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        crewScript = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScript.getCurrentAction();

        if (action.correctFacility(NAME))
        {
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
            Invoke(nameof(refillMotor), 30f);
            
        }

    }
    void refillMotor()
    {
        if(toInsert >= 0)
        {
            fuelLevel = toInsert;
        }
        else
        {
            fuelLevel = MAX_FUEL_LEVEL;
        }
        crewScript.orderDone();
        crewScript.setDoingAction(false);
    }
}

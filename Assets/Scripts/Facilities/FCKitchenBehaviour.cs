using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCKitchenBehaviour : FCBehaviour
{
    private const int FOOD_RESTAURATION = 25;
    public const string NAME = "Kitchen";
    int avaibleFood;
    int foodEaten;
    CMBehaviour crewScript;
    private bool colliding;
    // Start is called before the first frame update
    void Start()
    {
        avaibleFood = 123;
        foodEaten = 0;
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding && crewScript.getInFacility() && crewScript.getCurrentAction() != null && crewScript.getCurrentAction().correctFacility(NAME))
            startEating();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
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

    public void startEating()
    {
        crewScript.setInFacility(false);
        Debug.Log("started eating");
        EatAction eatAction = (EatAction)crewScript.getCurrentAction();
        int quantity = eatAction.getQuantity();
        foodEaten = 0;
        if (avaibleFood != 0)
        {
            if (avaibleFood >= quantity)
            {
                foodEaten = quantity;
            }
        }

        Invoke(nameof(eat), 5f);
    }

    private void eat()
    {
        Debug.Log("finish eating " + foodEaten + ". Current Hunger: " + crewScript.getHunger());
        crewScript.setHunger(crewScript.getHunger() + foodEaten * FOOD_RESTAURATION < 100 ? crewScript.getHunger() + foodEaten * FOOD_RESTAURATION : 100);
        avaibleFood -= foodEaten;
        crewScript.orderDone();
        crewScript.setDoingAction(false);
        crewScript.setInFacility(true);

    }

    internal void addFood(int foodReward)
    {
        avaibleFood += foodReward;
    }

    public string getContext()
    {
        string context = "In the kitchen, there are " + avaibleFood + " rations available, each one give " + FOOD_RESTAURATION + " food points";
        return context;
    }

    public override void OnClick()
    {
        Debug.Log("Click on Kitchen");
    }
}

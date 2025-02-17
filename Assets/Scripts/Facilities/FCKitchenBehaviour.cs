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
    // Start is called before the first frame update
    void Start()
    {
        avaibleFood = 123;
        foodEaten = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        crewScript = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScript.getCurrentAction();

        if (action.correctFacility(NAME))
        {
            EatAction eatAction = (EatAction)crewScript.getCurrentAction();
            string fullness = eatAction.getFullness();
            int quantity = eatAction.getQuantity();
            foodEaten = 0;
            if (fullness.Equals("doesnt apply"))
            {
                if(avaibleFood != 0)
                {
                    if(avaibleFood >= quantity)
                    {
                        foodEaten = quantity;
                    }
                }
            }
            else
            {
                int currentHunger = crewScript.getHunger();
                while(currentHunger < int.Parse(fullness) && avaibleFood > 0)
                {
                    foodEaten += 1;
                    currentHunger += FOOD_RESTAURATION;
                }
            }
            Invoke(nameof(eat), 30f);
        }

    }

    private void eat()
    {
        crewScript.setHunger(crewScript.getHunger() + foodEaten * FOOD_RESTAURATION < 100 ? crewScript.getHunger() + foodEaten * FOOD_RESTAURATION : 100);
        crewScript.orderDone();
        crewScript.setDoingAction(false);
    }
}

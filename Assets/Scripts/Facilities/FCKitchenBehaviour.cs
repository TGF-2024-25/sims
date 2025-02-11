using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCKitchenBehaviour : FCBehaviour
{
    public const string NAME = "Kitchen";
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        CMBehaviour crewScript = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScript.getCurrentAction();

        if (action.correctFacility(NAME))
        {
            EatAction eatAction = (EatAction)crewScript.getCurrentAction();
            string fullness = eatAction.getFullness();
            int quantity = eatAction.getQuantity();
            if (fullness.Equals("doesnt apply"))
            {
                Debug.Log("Eating " + quantity + " rations");
            }
            else
            {
                Debug.Log("Eating until I am " + fullness + "% full");
            }
            crewScript.orderDone();
            crewScript.setDoingAction(false);


            crewScript.setDoingAction(false);
        }

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCWorkshopBehaviour : FCBehaviour
{
    public const string NAME = "Workshop";
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
            CraftAction craftAction = (CraftAction)crewScript.getCurrentAction();
            int quantity = craftAction.getQuantity();
            string allPossible = craftAction.getAllPossible();
            string material = craftAction.getMaterial();
            
            if (allPossible.Equals("yes"))
            {
                Debug.Log("Crafting all possible of " + material);
            }
            else
            {
                Debug.Log("Crafting " + quantity + " of " + material);
            }
            crewScript.orderDone();
            crewScript.setDoingAction(false);
        }

    }

    public string getContext()
    {
        string context = "";
        return context;
    }

}

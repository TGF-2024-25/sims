using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCExit : FCBehaviour
{
    public const string NAME = "Exit";
    CMBehaviour crewScript;
    private bool colliding;
    // Start is called before the first frame update
    void Start()
    {
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding && crewScript.getInFacility() && crewScript.getCurrentAction() != null && crewScript.getCurrentAction().correctFacility(NAME))
            goExplore();
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

    public void goExplore()
    {
        
    }

    private void explore()
    {
       

    }

    public string getContext()
    {
        
        return "";
    }
}

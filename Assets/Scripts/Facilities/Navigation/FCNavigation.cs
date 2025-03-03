using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCNavigation : FCBehaviour
{
    [SerializeField]
    private GameObject navigationPanel;

    public const string NAME = "Navigation";
    private ShipBehaviour shipScript;
    private FCEngineBehaviour engineScript;
    private Planet currentPlanet;
    private bool colliding;
    private CMBehaviour crewScript;


    // Start is called before the first frame update
    void Start()
    {
        colliding = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (colliding && crewScript.getInFacility() && crewScript.getCurrentAction() != null && crewScript.getCurrentAction().correctFacility(NAME))
            startLevelUp();
    }

    private void startLevelUp()
    {
        Invoke("levelUp",5f);
    }

    private void levelUp()
    {
        shipScript.setLevel(shipScript.getLevel() + 1);
        crewScript.orderDone();
        crewScript.setDoingAction(false);
        crewScript.setInFacility(true);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameObject crewMember = collision.gameObject;
        CMBehaviour crewScriptAux = crewMember.GetComponent<CMBehaviour>();
        GameAction action = crewScriptAux.getCurrentAction();
        //Debug.Log(crewScript.getInFacility());
        if (action.correctFacility(NAME))
        {
            crewScript = crewScriptAux;
            colliding = true;
            crewScript.setInFacility(true);
        }

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        colliding = false;
    }

    public override void OnClick()
    {
        navigationPanel.GetComponent<NavigationUI>().ShowNavigationUI();
    }

    public Planet generatePlanet()
    {
        if (engineScript.reduceFuel(50))
        {
            currentPlanet = new Planet(shipScript.getLevel());
            return currentPlanet;
        }
        else
        {
            Debug.Log("NOT FUEL TO GOOOO");
            return null;
        }

    }

    public void setShip(ShipBehaviour shipBehaviour)
    {
        shipScript = shipBehaviour;
    }
    public void setEngine(FCEngineBehaviour engineScript)
    {
        this.engineScript = engineScript;
    }

    public Planet getCurrentPlanet()
    {
        return currentPlanet;
    }
}

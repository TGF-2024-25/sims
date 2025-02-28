using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCNavigation : FCBehaviour
{
    [SerializeField]
    private GameObject recruitmentPanel;

    public const string NAME = "Navigation";
    private ShipBehaviour shipScript;
    private FCEngineBehaviour engineScript;
    private Planet currentPlanet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void OnClick()
    {
        Debug.Log("Click on Navigation");

        recruitmentPanel.GetComponent<RecruitmentUI>().ShowRecruitmentUI();

        if (engineScript.reduceFuel(25))
        {
            currentPlanet = new Planet(shipScript.getLevel());
        }
        else
        {
            Debug.Log("NOT FUEL TO GOOOO");
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

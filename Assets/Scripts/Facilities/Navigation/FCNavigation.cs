using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCNavigation : FCBehaviour
{
    public const string NAME = "Navigation";
    private ShipBehaviour shipScript;
    private Planet currentPlanet;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Debug.Log("click");
        currentPlanet = new Planet(shipScript.getLevel());
        Debug.Log(currentPlanet);
    }

    public void setShip(ShipBehaviour shipBehaviour)
    {
        shipScript = shipBehaviour;
    }
}

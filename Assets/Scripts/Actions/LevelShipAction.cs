using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelShipAction : GameAction
{
    public const string NAME = "levelUp";
    public const string FACILITY = FCNavigation.NAME;


    public LevelShipAction(Dictionary<string, string> parameters, GameObject crewMember, bool ordered) : base(NAME, crewMember, ordered)
    {

    }
    public override bool correctFacility(string facility)
    {
        return NAME == facility;
    }

    public override void doAction()
    {
        GameObject navigationObject = GameObject.Find(FCNavigation.NAME);
        FCNavigation navigationScript = navigationObject.GetComponent<FCNavigation>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(navigationObject.transform.position.x, navigationObject.transform.position.y));
    }

    public static string getContext()
    {
        string context = "This action upgrades the ship to unlock new researches and new resources";




        return context;
    }
}

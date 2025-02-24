using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExploreAction : GameAction
{

    public const string NAME = "explore";
    public const string FACILITY = FCExit.NAME;


    public ExploreAction(Dictionary<string, string> parameters, GameObject crewMember, bool ordered) : base(NAME, crewMember, ordered)
    {
        
    }
    public override bool correctFacility(string facility)
    {
        return NAME == facility;
    }

    public override void doAction()
    {
        GameObject exitObject = GameObject.Find(FCExit.NAME);
        FCExit exitScript = exitObject.GetComponent<FCExit>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(exitObject.transform.position.x, exitObject.transform.position.y));
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchAction : GameAction
{

    public const string NAME = "research";
    public static Dictionary<string, List<string>> parametersOptions;


    public ResearchAction(Dictionary<string, string> parameters, GameObject crewMember) : base(NAME, crewMember)
    {

    }

    public override void doAction()
    {
        GameObject labObject = GameObject.Find(FCLabBehaviour.NAME);
        FCLabBehaviour labScript = labObject.GetComponent<FCLabBehaviour>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(labObject.transform.position.x, labObject.transform.position.y));
    }

    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        parametersOptions = parameters;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftAction : GameAction
{

    public const string NAME = "craft";
    public static Dictionary<string, List<string>> parametersOptions;


    public CraftAction(Dictionary<string, string> parameters, GameObject crewMember) : base(NAME, crewMember)
    {
        GameObject workshopObject = GameObject.Find(FCWorkshopBehaviour.NAME);
        FCWorkshopBehaviour workshopScript = workshopObject.GetComponent<FCWorkshopBehaviour>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(workshopObject.transform.position.x, workshopObject.transform.position.y));
    }
    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        parametersOptions = parameters;
    }

    public override void doAction()
    {
        throw new System.NotImplementedException();
    }
}

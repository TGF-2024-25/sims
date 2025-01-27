using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAction : GameAction
{
    int percentage;
    public const string NAME = "refill";
    public static Dictionary<string, List<string>> parametersOptions;

    public RefillAction(Dictionary<string, string> parameters, GameObject crewMember) : base(NAME, crewMember)
    {
        this.percentage = int.Parse(parameters["percentage"]);
    }

    public override void doAction()
    {
        GameObject motorObject = GameObject.Find(FCMotorBehaviour.NAME);
        FCMotorBehaviour motorScript = motorObject.GetComponent<FCMotorBehaviour>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(motorObject.transform.position.x, motorObject.transform.position.y));
    }

    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        parametersOptions = parameters;
    }
}

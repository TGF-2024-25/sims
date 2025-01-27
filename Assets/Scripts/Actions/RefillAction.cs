using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAction : GameAction
{
    private int percentage;
    private string justEnough;
    public const string NAME = "refill";
    public const string FACILITY = FCMotorBehaviour.NAME;
    public static Dictionary<string, List<string>> parametersOptions;

    public RefillAction(Dictionary<string, string> parameters, GameObject crewMember) : base(NAME, crewMember)
    {
        this.percentage = int.Parse(parameters["percentage"]);
        this.justEnough = parameters["justEnough"];
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

    public string getJustEnough()
    {
        return this.justEnough;
    }

    public int getPercentage()
    {
        return this.percentage;
    }
   public override bool correctFacility(string facility)
    {
        return facility == FACILITY;
    }
}

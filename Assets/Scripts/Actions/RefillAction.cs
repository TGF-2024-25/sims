using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAction : GameAction
{
    private int percentage;
    private string justEnough;
    public const string NAME = "refill";
    public const string FACILITY = FCEngineBehaviour.NAME;
    public static Dictionary<string, List<string>> parametersOptions;

    public RefillAction(Dictionary<string, string> parameters, GameObject crewMember,bool ordered) : base(NAME, crewMember,ordered)
    {
        this.percentage = int.Parse(parameters["percentage"]);
        this.justEnough = parameters["justEnough"];
    }

    public override void doAction()
    {
        GameObject engineObject = GameObject.Find(FCEngineBehaviour.NAME);
        FCEngineBehaviour engineScript = engineObject.GetComponent<FCEngineBehaviour>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(engineObject.transform.position.x, engineObject.transform.position.y));
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
    public override string ToString()
    {
        return NAME;
    }
}

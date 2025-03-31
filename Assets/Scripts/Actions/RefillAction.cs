using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAction : GameAction
{
    private int percentage;
    public const string NAME = "refill";
    public const string FACILITY = FCEngineBehaviour.NAME;
    public static Dictionary<string, List<string>> parametersOptions;

    public RefillAction(Dictionary<string, string> parameters, GameObject crewMember,bool ordered) : base(NAME, crewMember,ordered)
    {
        this.percentage = int.Parse(parameters["percentage"]);
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

    public static string getContext()
    {
        string context = "This actions is used to increase the fuel of the ship so you can reach new planets to gain more resources to craft new materials. The percentage is the fullnes of the tank you want to get to ";




        return context;
    }
}

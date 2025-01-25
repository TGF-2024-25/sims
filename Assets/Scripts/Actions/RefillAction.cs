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
        Debug.Log("Refileando " + percentage + " %");
    }

    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {

    }
}

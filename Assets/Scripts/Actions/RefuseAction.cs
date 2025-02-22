using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuseAction : GameAction
{
    public const string NAME = "refuse";
    public static Dictionary<string, List<string>> parametersOptions;
    public const string FACILITY = "";


    public RefuseAction(GameObject crewMember,bool ordered) : base(NAME, crewMember,ordered)
    {

    }
    public override void doAction()
    {

        Debug.Log("im refusing");
        return;
    }
    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        parametersOptions = parameters;
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

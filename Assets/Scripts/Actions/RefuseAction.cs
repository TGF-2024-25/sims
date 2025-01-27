using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuseAction : GameAction
{
    public const string NAME = "refuse";
    public static Dictionary<string, List<string>> parametersOptions;
    public const string FACILITY = "";


    public RefuseAction(GameObject crewMember) : base(NAME, crewMember)
    {

    }
    public override void doAction()
    {
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
}

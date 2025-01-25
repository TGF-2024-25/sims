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
        throw new System.NotImplementedException();
    }

    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {

    }
}

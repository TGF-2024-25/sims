using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftAction : GameAction
{

    public const string NAME = "craft";
    public static Dictionary<string, List<string>> parametersOptions;


    public CraftAction(Dictionary<string, string> parameters, GameObject crewMember) : base(NAME, crewMember)
    {

    }
    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        
    }

    public override void doAction()
    {
        throw new System.NotImplementedException();
    }
}

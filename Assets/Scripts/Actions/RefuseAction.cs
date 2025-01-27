using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefuseAction : GameAction
{
    public const string NAME = "refuse";
    public static Dictionary<string, List<string>> parametersOptions;

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
}

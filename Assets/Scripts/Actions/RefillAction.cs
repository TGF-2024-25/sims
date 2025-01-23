using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefillAction : Action
{
    int percentage;
    public const string NAME = "refill";

    public RefillAction(Dictionary<string, string> parameters) : base(NAME)
    {
        this.percentage = int.Parse(parameters["percentage"]);
    }

    public override void doAction()
    {
        Debug.Log("Refileando " + percentage + " %");
    }

    public static void LoadParameterOptions()
    {
        parametersOptions = new Dictionary<string, List<string>>();
        List<string> parameterData = new List<string>();
        parameterData.Add("25");
        parameterData.Add("50");
        parameterData.Add("75");
        parameterData.Add("100");
        parametersOptions.Add("percentage", parameterData);
    }
}

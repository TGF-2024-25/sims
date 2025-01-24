using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : Action
{
    private int quantity;
    public const string NAME = "eat";

    public EatAction(Dictionary<string, string> parameters) : base(NAME)
    {
        this.quantity = int.Parse(parameters["quantity"]);
    }
    public override void doAction()
    {
        Debug.Log("Comiendo " + quantity + " raciones");
    }

    public static void loadParameterOptions()
    {
        parametersOptions = new Dictionary<string, List<string>>();
        List<string> parameterData = new List<string>();
        parameterData.Add("1");
        parameterData.Add("2");
        parameterData.Add("3");
        parametersOptions.Add("quantity", parameterData);
    }
}

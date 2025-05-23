using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : GameAction
{
    private int quantity;
    public const string NAME = "eat";
    public const string FACILITY = FCKitchenBehaviour.NAME;
    public static Dictionary<string, List<string>> parametersOptions;
    

    public EatAction(Dictionary<string, string> parameters, GameObject crewMember, bool ordered) : base(NAME,crewMember,ordered)
    {
        this.quantity = int.Parse(parameters["quantity"]);
    }
    public override void doAction()
    {
        GameObject kitchenObject = GameObject.Find(FCKitchenBehaviour.NAME);
        FCKitchenBehaviour kitchenScript = kitchenObject.GetComponent<FCKitchenBehaviour>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(kitchenObject.transform.position.x, kitchenObject.transform.position.y));
    }

    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        parametersOptions = parameters;
    }

    public int getQuantity()
    {
        return this.quantity;
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
        string context = "This action increseas the fullness of the crew member if the hunger reaches 0 that crew member will die";

        return context;
    }
}

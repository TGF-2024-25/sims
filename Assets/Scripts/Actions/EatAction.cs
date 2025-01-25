using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EatAction : GameAction
{
    private int quantity;
    private string quality;
    public const string NAME = "eat";
    public static Dictionary<string, List<string>> parametersOptions;

    public EatAction(Dictionary<string, string> parameters, GameObject crewMember) : base(NAME,crewMember)
    {
        this.quantity = int.Parse(parameters["quantity"]);
        this.quality = parameters["quality"];

        Debug.Log("Accion creada: " + this.quantity + " " + this.quality);
    }
    public override void doAction()
    {
        Debug.Log("Comiendo " + quantity + " raciones");
        GameObject kitchenObject = GameObject.Find(FCKitchenBehaviour.NAME);
        FCKitchenBehaviour kitchenScript = kitchenObject.GetComponent<FCKitchenBehaviour>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(kitchenObject.transform.position.x, kitchenObject.transform.position.y));
    }

    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        
    }
}

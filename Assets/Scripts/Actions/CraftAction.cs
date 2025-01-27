using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftAction : GameAction
{

    public const string NAME = "craft";
    public static Dictionary<string, List<string>> parametersOptions;
    public const string FACILITY = FCWorkshopBehaviour.NAME;
    private int quantity;
    private string allPosisible;
    private string material;

    public CraftAction(Dictionary<string, string> parameters, GameObject crewMember) : base(NAME, crewMember)
    {
        this.quantity =int.Parse(parameters["quantity"]);
        this.allPosisible = parameters["allPosisible"];
        this.material = parameters["material"];


    }
    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        parametersOptions = parameters;
    }

    public int getQuantity()
    {
        return this.quantity;
    }
    public string getAllPosisible()
    {
        return this.allPosisible;
    }
    public string getMaterial()
    {
        return this.material;
    }

    public override void doAction()
    {
        GameObject workshopObject = GameObject.Find(FCWorkshopBehaviour.NAME);
        FCWorkshopBehaviour workshopScript = workshopObject.GetComponent<FCWorkshopBehaviour>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(workshopObject.transform.position.x, workshopObject.transform.position.y));
    }

    public override bool correctFacility(string facility)
    {
        return facility == FACILITY;
    }
}

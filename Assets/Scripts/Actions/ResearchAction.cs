using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResearchAction : GameAction
{

    public const string NAME = "research";
    public static Dictionary<string, List<string>> parametersOptions;
    public const string FACILITY = FCLabBehaviour.NAME;
    private int hours;

    public ResearchAction(Dictionary<string, string> parameters, GameObject crewMember, bool ordered) : base(NAME, crewMember,ordered   )
    {
        this.hours = int.Parse(parameters["hours"]);
    }

    public override void doAction()
    {
        GameObject labObject = GameObject.Find(FCLabBehaviour.NAME);
        FCLabBehaviour labScript = labObject.GetComponent<FCLabBehaviour>();
        CMMovement cmMovementScript = this.crewMember.GetComponent<CMMovement>();
        cmMovementScript.setTargetPosition(new Vector2(labObject.transform.position.x, labObject.transform.position.y));
    }

    public static void loadParameterOptions(Dictionary<string, List<string>> parameters)
    {
        parametersOptions = parameters;
    }

    public int getHours()
    {
        return this.hours;
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
        string context = "This actions is used to unlock new recipies in the workshop so that you can craft new materials to upgrade the ship to the next level.";




        return context;
    }
}

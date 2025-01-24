using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public GameObject PromptGeneratorObject;
    private PromptGenerator PG;

    private List<string> actionList = new List<string> { EatAction.NAME, RefillAction.NAME };

    void Awake()
    {
        PG = PromptGeneratorObject.GetComponent<PromptGenerator>();
        loadParameterOptions();
    }

    public Action createActionByName(string name, Dictionary<string, string> parameters)
    {
        switch (name)
        {
            case EatAction.NAME:
                return new EatAction(parameters);
            case RefillAction.NAME:
                return new RefillAction(parameters);
            default:
                return null;
        }
    }

    public void loadParameterOptions()
    {
        EatAction.loadParameterOptions();
        RefillAction.loadParameterOptions();
    }

    public void generateAction(string content, List<string> possibleActions, CMBehaviour crewMember)
    {
        PG.askOrderAction(content, possibleActions, response =>
        {
            Debug.Log("Action generated: " + response);

        });
    }


}

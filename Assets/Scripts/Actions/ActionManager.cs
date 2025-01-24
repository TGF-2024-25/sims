using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManager : MonoBehaviour
{
    public GameObject LLMManagerObject;
    private LLMManager LLMM;

    private List<string> actionList = new List<string> { EatAction.NAME, RefillAction.NAME };

    void Start()
    {
        LLMM = LLMManagerObject.GetComponent<LLMManager>();
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

    public Action generateAction(string content, List<string> possibleActions, List<string> previousActions, string context, CMBehaviour tripulante)
    {
        string petition = "I am a crew member on a space ship, my attributes are: " + context + ". The previous actions I have performed are: (";

        foreach (string prevAction in previousActions)
        {
            petition = petition + " " + prevAction + ",";
        }

        petition = petition + "). According to this order given to me by my captain: " + content + ". Which one of these actions should I perform to fulfill the order: ";


        foreach (string posAction in possibleActions)
        {
            petition = petition + " " + posAction + "," ;
        }

        petition = petition + ". Answer only with a JSON with this format: {action: <actionName>}";

        Debug.Log(petition);

        SendPromptToLLM(petition, tripulante);

        return null;

    }

    public void SendPromptToLLM(string prompt, CMBehaviour tripulante)
    {

        StartCoroutine(LLMM.SendRequestToGemini(prompt, tripulante));
    }
}

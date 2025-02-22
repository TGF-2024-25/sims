using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEditor.Timeline.TimelinePlaybackControls;

public class PromptGenerator : MonoBehaviour
{
    public GameObject LLMManagerObject;
    private LLMManager LLMM;
    void Awake()
    {
        LLMM = LLMManagerObject.GetComponent<LLMManager>();
    }

    public void askAction(bool isOrder,string content, string context, List<string> possibleActions, Action<string> callback)
    {
        string petition = "I am a crew member on a space ship and this is my context and the context of my ship: " + context;
        if (isOrder)
        {
            petition += " According to this order given to me by my captain: " + content +
           ". Which one of these actions should I perform to fulfill the order: ";
        }
        else
        {
            petition += ". Which action should I do now considering that between 10:00 and 18:00 I should be 40% likely to do my job." + 
                " This is the list of possible actions: ";
        }


        foreach (string posAction in possibleActions)
        {
            petition += " " + posAction + ",";
        }

        if (isOrder)
        {
            //petition += ".If the other ones make no sense, choose refuse.";
        }

        petition += ".Answer only with a JSON with this format: {action: <actionName>}. " +
            "Action and action name should always be between quotation marks as a string";



        //Debug.Log(petition);    

        
        LLMM.SendRequestToGemini(petition, response =>
        {
            callback(response);
        });
    }

    public void askParameters(bool isOrder,string content, string context, string actionName, Dictionary<string, List<string>> parameterOptions, Action<string> callback)
    {

        string petition = "I am a crew member on a space ship and this is my context and the context of my ship: " + context;
        if (isOrder)
        {
            petition += ". According to this order given to me by my captain: " + content +
            ". You told me to perform the action " + actionName;
            petition += ". Now I have the following parameters each with their different options, regarding this action. " +
            "I need you to choose one option for each parameter, the one you think suits best the order given by my captain. Here are the parameters and their options:";
        }
        else
        {
            petition += ". You told me to perform the action " + actionName;
            petition += ". Now I have the following parameters each with their different options, regarding this action. " +
            "I need you to choose one option for each parameter, the one you think suits best the context given. Here are the parameters and their options:";
        } 

        
        foreach (var parameter in parameterOptions)
        {
            petition += " " + parameter.Key + " (";

            foreach (string op in parameter.Value)
            {
                petition += " " + op + ",";
            }
            petition += ") ;";
        }

        petition += ". Answer only with a JSON with this format, one key-value pair for each parameter: {parameterName: <optionChosen>}. " +
            "ParameterName and optionChosen should always be between quotation marks as a string";

        //Debug.Log(petition);

        LLMM.SendRequestToGemini(petition, response =>
        {
            callback(response); // Llama al callback con la respuesta recibida
        });
    }

}

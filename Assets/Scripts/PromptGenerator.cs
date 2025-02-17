using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class PromptGenerator : MonoBehaviour
{
    public GameObject LLMManagerObject;
    private LLMManager LLMM;
    void Awake()
    {
        LLMM = LLMManagerObject.GetComponent<LLMManager>();
    }

    public void askAction(bool isOrder,string content, List<string> possibleActions, Action<string> callback)
    {
        string petition = "";
        if (isOrder)
        {
            petition = "I am a crew member on a space ship. According to this order given to me by my captain: " + content +
           ". Which one of these actions should I perform to fulfill the order: ";
        }
        else
        {
            petition = "I am a crew member on a space ship and this is my context and the context of my ship: " + content +
                ". Which action should I do prioritazing eating if my hunger level is below 25 and if not, doing my assigned job." + 
                " This is the list of possible actions: ";
        }
       

        foreach (string posAction in possibleActions)
        {
            petition += " " + posAction + ",";
        }

        petition += ".If the other ones make no sense, choose refuse. Answer only with a JSON with this format: {action: <actionName>}. " +
            "Action and action name should always be between quotation marks as a string";

        //Debug.Log(petition);    

        // Llamada a la función de petición con corrutina
        LLMM.SendRequestToGemini(petition, response =>
        {
            callback(response); // Llama al callback con la respuesta recibida
        });
    }

    public void askParameters(bool isOrder,string content, string actionName, Dictionary<string, List<string>> parameterOptions, Action<string> callback)
    {

        string petition = "";
        if (isOrder)
        {
            petition = "I am a crew member on a space ship. According to this order given to me by my captain: " + content +
            ". You told me to perform the action " + actionName;
            petition += ". Now I have the following parameters each with their different options, regarding this action. " +
            "I need you to choose one option for each parameter, the one you think suits best the order given by my captain. Here are the parameters and their options:";
        }
        else
        {
            petition = "I am a crew member on a space ship. According to this context: " + content +
            ". You told me to perform the action " + actionName;
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

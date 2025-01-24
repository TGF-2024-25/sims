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

    public void askOrderAction(string content, List<string> possibleActions, Action<string> callback)
    {
        string petition = "I am a crew member on a space ship. According to this order given to me by my captain: " + content + 
            ". Which one of these actions should I perform to fulfill the order: ";

        foreach (string posAction in possibleActions)
        {
            petition += " " + posAction + ",";
        }

        petition += ".If the other ones make no sense, choose idle. Answer only with a JSON with this format: {action: <actionName>}.";

        Debug.Log("Generated prompt: " + petition);

        // Llamada a la función de petición con corrutina
        LLMM.SendRequestToGemini(petition, response =>
        {
            Debug.Log("Received response: " + response);
            callback(response); // Llama al callback con la respuesta recibida
        });
    }

    public void askOrderParameters(string content, string actionName, Dictionary<string, List<string>> parameterOptions, Action<string> callback)
    {
        string petition = "I am a crew member on a space ship. According to this order given to me by my captain: " + content +
            ". You told me to perform the action " + actionName;

        petition += ". Now I have the following parameters each with their different options, regarding this action. " +
            "I need you to choose one option for each parameter, the one you think suits best the order given by my captain. Here are the parameters and their options:";

        foreach (var parameter in parameterOptions)
        {
            petition += " " + parameter.Key + " (";

            foreach (string op in parameter.Value)
            {
                petition += " " + op + ",";
            }
            petition += ") ;";
        }

        petition += ". Answer only with a JSON with this format, one key-value pair for each parameter: {parameterName: <optionChosen>}.";

        Debug.Log("Generated prompt: " + petition);

        LLMM.SendRequestToGemini(petition, response =>
        {
            Debug.Log("Received response: " + response);
            callback(response); // Llama al callback con la respuesta recibida
        });
    }

}

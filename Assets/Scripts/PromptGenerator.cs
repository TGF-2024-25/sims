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
        /*string petition = "I am a crew member on a space ship and this is my context and the context of my ship: " + context;
        if (isOrder)
        {
            petition += " According to this order given to me by my captain: " + content +
           ". Which one of these actions should I perform to fulfill the order: ";
        }
        else
        {
            petition += ". Which action should I do now considering that i should be more likely to do my job." + 
                " This is the list of possible actions: ";
        }


        foreach (string posAction in possibleActions)
        {
            petition += " " + posAction + ",";
        }

        petition += ". This is what each action does: ";
        petition += "eat: " + EatAction.getContext();
        petition += "refill: " + RefillAction.getContext();
        petition += "research: " + ResearchAction.getContext();
        petition += "craft: " + CraftAction.getContext();
        petition += "levelShip: " + LevelShipAction.getContext();

        if (isOrder)
        {
            //petition += ".If the other ones make no sense, choose refuse.";
        }

        petition += ".Answer only with a JSON with this format: {action: <actionName>}. " +
            "Action and action name should always be between quotation marks as a string";
        */
        string petition = "I am a crew member on a space ship. Here is my status and the ship's context:\n" + context + "\n\n";

        if (isOrder)
        {
            petition += "I have received an order from my captain: '" + content + "'.\n";
            petition += "I must prioritize fulfilling this order unless I lack the required materials.\n\n";
        }
        else
        {
            petition += "I should act based on my job" +
                        ", while also considering the ship�s needs and my own well-being.\n\n";
        }

        petition += "Actions and their effects(not all are posible):\n";
        petition += "- **eat**: " + EatAction.getContext() + "\n";
        petition += "- **refill**: "+ RefillAction.getContext() + "\n";
        petition += "- **research**: " + ResearchAction.getContext() + "\n";
        petition += "- **craft**: " + CraftAction.getContext() + "\n";
        if (isOrder)
        {
            petition += "- **levelShip**: " + LevelShipAction.getContext() + "\n";
        }
  
        petition += "List of posible actions: \n";
        foreach (string posAction in possibleActions)
        {
            petition += " " + posAction + ",\n";
        }

        petition += "Decision-making rules:\n";
        petition += "1. If my hunger is dangerously low (below 25), prioritize **eating** and if its above 50 dont eat.\n";
        petition += "2. If fuel is critical (below 50), prioritize **refill**.\n";
        petition += "3. If the order requires upgrading the ship, check if I have the necessary materials:\n";
        petition += "   - If I do, choose **levelShip**.\n";
        petition += "   - If I don�t, determine what�s missing and choose the best action (**craft** or **research**) to progress.\n";

        petition += "Answer only with a JSON in this format:  {action: <actionName>}.\n";
        petition += "Action and action name must always be between quotation marks as a string.";



        Debug.Log(petition);    

        
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

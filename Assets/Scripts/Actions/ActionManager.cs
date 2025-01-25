using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;
using System.Xml;
using Newtonsoft.Json;

public class ActionManager : MonoBehaviour
{
    public GameObject PromptGeneratorObject;
    private PromptGenerator PG;

    private List<string> actionList = new List<string> { EatAction.NAME, RefillAction.NAME, IddleAction.NAME };

    private const int MAX_RETRY = 3;

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
            case IddleAction.NAME:
                return new IddleAction();
            default:
                return null;
        }
    }

    public Dictionary<string, List<string>> getActionParameterOptions(string name)
    {
        switch (name)
        {
            case EatAction.NAME:
                return EatAction.parametersOptions;
            case RefillAction.NAME:
                return RefillAction.parametersOptions;
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
        Debug.Log("Creating action");
        int i = 0;
        while(generateActionCorutine(content, possibleActions, crewMember, i) != null)
        {
            Debug.Log("Trying again");
            i++;
        }
    }

    public Action generateActionCorutine(string content, List<string> possibleActions, CMBehaviour crewMember, int retryCount)
    {
        if(retryCount == MAX_RETRY)
        {
            return createActionByName(IddleAction.NAME,null);
        }
        Action newAction = null;
        
        PG.askOrderAction(content, possibleActions, response =>
        {
            string cleanResponse = ExtractJson(response);
            JObject jsonResponse = JObject.Parse(cleanResponse);
            if (jsonResponse.ContainsKey("action"))
            {
                string action = jsonResponse["action"].ToString();

                Debug.Log("Action: " + action);

                Dictionary<string, List<string>> parameterOptions = getActionParameterOptions(action);

                PG.askOrderParameters(content, action, parameterOptions, response2 =>
                {
                    string cleanResponse2 = ExtractJson(response2);
                    JObject jsonResponse2 = JObject.Parse(cleanResponse);

                    if (checkParametersJson(jsonResponse2, parameterOptions))
                    {
                        Dictionary<string, string> parametersChosen = JsonConvert.DeserializeObject<Dictionary<string, string>>(cleanResponse2);

                        foreach (var entry in parametersChosen)
                        {
                            Debug.Log($"Key: {entry.Key}, Value: {entry.Value}");
                        }

                        newAction = createActionByName(action, parametersChosen);
                    }
                    else
                    {
                        Debug.Log("Not valid json format in parameters response");
                    }
                    
                });
            }
            else
            {
                Debug.Log("Not valid json format in action response");
            }

        });
        return newAction;
    }





    private bool checkParametersJson(JObject jsonResponse, Dictionary<string, List<string>> parameterOptions)
    {
        bool isValid = true;
        foreach(var param in parameterOptions)
        {
            if (jsonResponse.ContainsKey(param.Key))
            {
                string chosenOption = jsonResponse[param.Key].ToString();
                if (!param.Value.Contains(chosenOption))
                {
                    isValid = false;
                }
            }
            else
            {
                isValid = false;
            }
        }
        return isValid;
    }


    private string ExtractJson(string rawResponse)
    {
        var match = Regex.Match(rawResponse, @"\{(?:[^{}]|(?<Open>\{)|(?<-Open>\}))*\}");

        if (match.Success)
        {
            return match.Value;
        }

        Debug.LogError("No valid JSON found in the response!");
        return string.Empty;
    }


}

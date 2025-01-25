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

    public GameAction createActionByName(string name, Dictionary<string, string> parameters, GameObject crewMember)
    {
        switch (name)
        {
            case EatAction.NAME:
                return new EatAction(parameters,crewMember);
            case RefillAction.NAME:
                return new RefillAction(parameters,crewMember);
            case IddleAction.NAME:
                return new IddleAction(crewMember);
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

    public void generateAction(string content, List<string> possibleActions, GameObject crewMember)
    {
        generateAction(content, possibleActions, crewMember, 0);
    }

    public void generateAction(string content, List<string> possibleActions, GameObject crewMember, int retry)
    {
        generateActionCorutine(content, possibleActions, crewMember, response =>
        {
            if (response != null)
            {
                CMBehaviour cmBehaviourScript = crewMember.GetComponent<CMBehaviour>();
                cmBehaviourScript.updateActionList(response);
            }
            if (response == null && retry < MAX_RETRY)
            {
                generateAction(content, possibleActions, crewMember,retry+1);
            }
        });
        
    }

    public void generateActionCorutine(string content, List<string> possibleActions, GameObject crewMember, Action<GameAction> callback)
    {
        GameAction newAction = null;
   
        PG.askOrderAction(content, possibleActions, response =>
        {
            string cleanResponse = ExtractJson(response);
            JObject jsonResponse = JObject.Parse(cleanResponse);

            if (jsonResponse.ContainsKey("action"))
            {
                string action = jsonResponse["action"].ToString();

                Dictionary<string, List<string>> parameterOptions = getActionParameterOptions(action);

                PG.askOrderParameters(content, action, parameterOptions, response2 =>
                {
                    string cleanResponse2 = ExtractJson(response2);
                    JObject jsonResponse2 = JObject.Parse(cleanResponse2);
                    if (checkParametersJson(jsonResponse2, parameterOptions))
                    {
                        Dictionary<string, string> parametersChosen = JsonConvert.DeserializeObject<Dictionary<string, string>>(cleanResponse2);

                        newAction = createActionByName(action, parametersChosen,crewMember);
                        
                        callback?.Invoke(newAction);
                    }
                    else
                    {
                        Debug.Log("Not valid json format in parameters response");
                        callback?.Invoke(newAction);
                    }
                    
                });
            }
            else
            {
                Debug.Log("Not valid json format in action response");
                callback?.Invoke(newAction);
            }
        });
    
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

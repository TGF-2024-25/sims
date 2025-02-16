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

    private List<string> actionList = new List<string> {
        EatAction.NAME,
        RefillAction.NAME,
        RefuseAction.NAME,
        ResearchAction.NAME, 
        CraftAction.NAME 
    };

    private const int MAX_RETRY = 3;

    void Awake()
    {
        PG = PromptGeneratorObject.GetComponent<PromptGenerator>();
        loadParameterOptions();
    }

    public GameAction createActionByName(string name, Dictionary<string, string> parameters, GameObject crewMember, bool ordered)
    {
        switch (name)
        {
            case EatAction.NAME:
                return new EatAction(parameters,crewMember,ordered);
            case RefillAction.NAME:
                return new RefillAction(parameters,crewMember,ordered);
            case RefuseAction.NAME:
                return new RefuseAction(crewMember,ordered);
            case ResearchAction.NAME:
                return new ResearchAction(parameters, crewMember,ordered);
            case CraftAction.NAME:
                return new CraftAction(parameters, crewMember,ordered);
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
            case ResearchAction.NAME:
                return ResearchAction.parametersOptions;
            case CraftAction.NAME:
                return CraftAction.parametersOptions;
            default:
                return null;
        }
    }

    public void loadParameterOptions()
    {
        string jsonFile = Resources.Load<TextAsset>("ActionData").ToString();
        Dictionary<string, Dictionary<string, List<string>>> data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<string>>>>(jsonFile);
        EatAction.loadParameterOptions(data[EatAction.NAME]);
        RefillAction.loadParameterOptions(data[RefillAction.NAME]);
        ResearchAction.loadParameterOptions(data[ResearchAction.NAME]);
        CraftAction.loadParameterOptions(data[CraftAction.NAME]);
    }

    public void generateOrder(string content,GameObject crewMember)
    {
        generateOrder(content, actionList, crewMember, 0);
    }

    public void generateOrder(string content, List<string> possibleActions, GameObject crewMember, int retry)
    {
        generateActionCorutine(true,content, possibleActions, crewMember, response =>
        {
            if (response != null)
            {
                CMBehaviour cmBehaviourScript = crewMember.GetComponent<CMBehaviour>();
                cmBehaviourScript.updateActionList(response);
            }
            if (response == null && retry < MAX_RETRY)
            {
                generateOrder(content, possibleActions, crewMember,retry+1);
            }
        });
        
    }

    public void generateAction(string content, GameObject crewMember, List<string> possibleActions, Action<GameAction> callback)
    {
        generateAction(content, possibleActions, crewMember, 0, response =>
        {
            callback?.Invoke(response);
        });
    }

    public void generateAction(string content, List<string> possibleActions, GameObject crewMember, int retry, Action<GameAction> callback)
    {
        generateActionCorutine(false,content, possibleActions, crewMember, response =>
        {
            if (response != null)
            {
                callback?.Invoke(response);
            }
            if (response == null && retry < MAX_RETRY)
            {
                generateOrder(content, possibleActions, crewMember, retry + 1);
            }
        });

    }

    public void generateActionCorutine(bool isOrder,string content, List<string> possibleActions, GameObject crewMember, Action<GameAction> callback)
    {
        GameAction newAction = null;
   
        PG.askAction(isOrder,content, possibleActions, response =>
        {
            string cleanResponse = ExtractJson(response);
            JObject jsonResponse = JObject.Parse(cleanResponse);
            Debug.Log(cleanResponse);

            if (jsonResponse.ContainsKey("action"))
            {
                string action = jsonResponse["action"].ToString();
                Dictionary<string, List<string>> parameterOptions = getActionParameterOptions(action);

                PG.askParameters(isOrder,content, action, parameterOptions, response2 =>
                {
                    string cleanResponse2 = ExtractJson(response2);
                    JObject jsonResponse2 = JObject.Parse(cleanResponse2);
                    Debug.Log(cleanResponse);

                    if (checkParametersJson(jsonResponse2, parameterOptions))
                    {
                        Dictionary<string, string> parametersChosen = JsonConvert.DeserializeObject<Dictionary<string, string>>(cleanResponse2);

                        newAction = createActionByName(action, parametersChosen,crewMember,true);
                        
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

    public void chooseNextAction(List<GameAction> gameActions, GameObject crewMember, List<string> possibleActions)
    {

        CMBehaviour cmBehaviourScript = crewMember.GetComponent<CMBehaviour>();

        if (gameActions.Count != 0)
        {
            GameAction nextAction = gameActions[0];
            cmBehaviourScript.setCurrentAction(nextAction);
            nextAction.doAction();
            cmBehaviourScript.addPreviousAction(nextAction);
        }
        else
        {
            generateAction("I am Juan and my job is to research. My hunger level is at 24. This ship fuel is at 78 of 100 percent. The current research is at 35 of 100 percent", crewMember, possibleActions, response =>
            {
                cmBehaviourScript.setCurrentAction(response);
                response.doAction();
                cmBehaviourScript.addPreviousAction(response);

            });
        }
        
    }

    
}

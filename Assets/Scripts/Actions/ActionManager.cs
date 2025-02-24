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
            case ExploreAction.NAME:
                return new ExploreAction(parameters, crewMember, ordered);
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

    public void generateOrder(string content,GameObject crewMember, ShipBehaviour shipScript)
    {
        CMBehaviour cmBehaviourScript = crewMember.GetComponent<CMBehaviour>();
        string crewContext = cmBehaviourScript.getContext();
        string shipContext = shipScript.getContext();

        generateOrder(content, crewContext + shipContext, actionList, crewMember, 0);
    }

    public void generateOrder(string content, string context, List<string> possibleActions, GameObject crewMember, int retry)
    {
        generateActionCorutine(true,content, context, possibleActions, crewMember, response =>
        {
            if (response != null)
            {
                CMBehaviour cmBehaviourScript = crewMember.GetComponent<CMBehaviour>();
                Debug.Log(response.ToString());
                if (response.ToString() != RefuseAction.NAME)
                {
                    cmBehaviourScript.updateActionList(response);
                }
                else
                {
                    Debug.Log("Refuse to do that");
                }

            }
            if (response == null && retry < MAX_RETRY)
            {
                generateOrder(content, context, possibleActions, crewMember,retry+1);
            }
        });
        
    }

    public void generateAction(string context, GameObject crewMember, List<string> possibleActions)
    {
        generateAction("", context, possibleActions, crewMember, 0);
    }

    public void generateAction(string content, string context, List<string> possibleActions, GameObject crewMember, int retry)
    {
        generateActionCorutine(false, content, context, possibleActions, crewMember, response =>
        {
            if (response != null)
            {
                CMBehaviour cmBehaviourScript = crewMember.GetComponent<CMBehaviour>();

                cmBehaviourScript.setCurrentAction(response);
                //Debug.Log(response);
                response.doAction();
                cmBehaviourScript.addPreviousAction(response);

            }
            if (response == null && retry < MAX_RETRY)
            {
                generateAction(content, context, possibleActions, crewMember, retry + 1);
            }
        });

    }

    public void generateActionCorutine(bool isOrder, string content, string context, List<string> possibleActions, GameObject crewMember, Action<GameAction> callback)
    {
        GameAction newAction = null;
   
        PG.askAction(isOrder,content, context, possibleActions, response =>
        {
            string cleanResponse = ExtractJson(response);
            JObject jsonResponse = JObject.Parse(cleanResponse);

            if (jsonResponse.ContainsKey("action"))
            {
                string action = jsonResponse["action"].ToString();

                if (action != RefuseAction.NAME)
                {
                    Dictionary<string, List<string>> parameterOptions = getActionParameterOptions(action);

                    PG.askParameters(isOrder, content, context, action, parameterOptions, response2 =>
                    {
                        string cleanResponse2 = ExtractJson(response2);
                        JObject jsonResponse2 = JObject.Parse(cleanResponse2);

                        if (checkParametersJson(jsonResponse2, parameterOptions))
                        {
                            Dictionary<string, string> parametersChosen = JsonConvert.DeserializeObject<Dictionary<string, string>>(cleanResponse2);

                            newAction = createActionByName(action, parametersChosen, crewMember, true);

                            Debug.Log(newAction);

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
                    newAction = createActionByName(action, null, crewMember, true);
                    callback?.Invoke(newAction);
                }
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

    public void chooseNextAction(List<GameAction> gameActions, GameObject crewMember, List<string> possibleActions, ShipBehaviour shipScript)
    {

        CMBehaviour cmBehaviourScript = crewMember.GetComponent<CMBehaviour>();

        if (gameActions.Count != 0)
        {
            Debug.Log("Gonna do the order: " + gameActions[0]);
            GameAction nextAction = gameActions[0];
            cmBehaviourScript.setCurrentAction(nextAction);
            nextAction.doAction();
            cmBehaviourScript.addPreviousAction(nextAction);
        }
        else
        {
            string crewContext = cmBehaviourScript.getContext();
            string shipContext = shipScript.getContext();
            generateAction(crewContext + shipContext, crewMember, possibleActions);
        }
        
    }

    
}

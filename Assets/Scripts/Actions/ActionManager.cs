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
        PG.askOrderAction(content, possibleActions, response =>
        {
            string cleanResponse = ExtractJson(response);
            JObject jsonResponse = JObject.Parse(cleanResponse);
            string action = jsonResponse["action"].ToString();

            Debug.Log("Action: " + action);

            Dictionary<string, List<string>> parameterOptions = getActionParameterOptions(action);

            PG.askOrderParameters(content, action, parameterOptions, response2 =>
            {
                string cleanResponse2 = ExtractJson(response2);
                //JObject jsonResponse = JObject.Parse(cleanResponse);

                Dictionary<string, string> parametersChosen = JsonConvert.DeserializeObject<Dictionary<string, string>>(cleanResponse2);

                foreach (var entry in parametersChosen)
                {
                    Debug.Log($"Key: {entry.Key}, Value: {entry.Value}");
                }

                Action newAction = createActionByName(action, parametersChosen);
            });


        });
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

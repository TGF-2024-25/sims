using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class CMInfoUI : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI dataText;
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject orderPanel;
    [SerializeField]
    private TextMeshProUGUI hungerText;
    [SerializeField]
    private Image hungerBar;
    [SerializeField]
    private TextMeshProUGUI actionText;

    private GameObject crewMember;
    private CMBehaviour cmScript;

    

    private string cmName;
    private string job;
    private string personality;
    private int hunger;
    private string currentAction;

    void Start()
    {
        panel.SetActive(false);
    }


    void Update()
    {
        
    }

    public void ShowCrewInfo(GameObject crewMate)
    {
        crewMember = crewMate;
        cmScript = crewMate.GetComponent<CMBehaviour>();
        cmName = cmScript.getName();
        job = cmScript.getJob();
        personality = cmScript.getPersonality();
        hunger = cmScript.getHunger();
        //Image sprite = cmScript.GetComponent<SpriteRenderer>().sprite;
        //current action

        dataText.text = "<b>Name:</b> " + cmName + "\n\n<b>Job:</b> " + job + "\n\n<b>Personality:</b> " + personality;
        //crewSprite.sprite = sprite;

        panel.SetActive(true);
        GetComponentInParent<Canvas>().sortingOrder = UIManager.GetHighestSortingOrder();

        InvokeRepeating("UpdateInfo", 0f, 1f);
    }

    public void CloseCrewInfo()
    {
        panel.SetActive(false);
        CancelInvoke("UpdateInfo");
    }

    public void OpenOrderUI()
    {
        OrderUI orderScript = orderPanel.GetComponent<OrderUI>();
        orderScript.ShowOrderUI(crewMember);
        CloseCrewInfo();
    }

    private void UpdateInfo()
    {
        if (cmScript != null)
        {
            hunger = cmScript.getHunger();
            GameAction action = cmScript.getCurrentAction();

            //set hunger and action
            hungerText.text = "<b>Hunger:</b> " + hunger + "%";
            hungerBar.fillAmount = (float)hunger / 100;

            if(action != null)
            {
                currentAction = Regex.Replace(action.ToString(), "(?<!^)([A-Z])", " $1");
                currentAction = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(currentAction.ToLower());
                actionText.text = "<b>Current Action:</b> " + currentAction;
            }
            else
                actionText.text = "<b>Current Action:</b> None";
        }
    }

}

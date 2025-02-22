using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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

    private GameObject crewMember;
    private CMBehaviour cmScript;

    

    private string cmName;
    private string job;
    private string personality;
    private int hunger;

    void Start()
    {
        panel.SetActive(false);
    }


    void Update()
    {
        if(cmScript != null)
        {
            hunger = cmScript.getHunger();
            //current action

            //set hunger and action
            hungerText.text = "<b>Hunger:</b> " + hunger + "%";
            hungerBar.fillAmount = (float)hunger / 100;
        }
        
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
    }

    public void CloseCrewInfo()
    {
        panel.SetActive(false);
    }

    public void OpenOrderUI()
    {
        OrderUI orderScript = orderPanel.GetComponent<OrderUI>();
        orderScript.ShowOrderUI(crewMember);
        CloseCrewInfo();
    }
}

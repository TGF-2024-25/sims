using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;

public class NavigationUI : MonoBehaviour
{
    [SerializeField]
    private GameObject actionManager;
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject recruitmentPanel;
    [SerializeField]
    private GameObject planetImage;
    [SerializeField]
    private GameObject navigationFacility;
    [SerializeField]
    private GameObject exitFacility;
    [SerializeField]
    private GameObject engineFacility;
    [SerializeField]
    private TextMeshProUGUI fuelRequiredText;
    [SerializeField]
    private TextMeshProUGUI recruitsRequiredText;
    [SerializeField]
    private TextMeshProUGUI planetName;
    [SerializeField]
    private GameObject iconPrefab;
    [SerializeField]
    private Transform resourceGridContent;
    [SerializeField]
    private TextMeshProUGUI fuelText;
    [SerializeField]
    private Image fuelBar;
    [SerializeField]
    private GameObject backgroundPlanet;
    [SerializeField]
    private Button travelButton;
    [SerializeField]
    private Button recruitButton;
    [SerializeField]
    private Button startButton;
    [SerializeField]
    private TextMeshProUGUI startButtonText;
    [SerializeField]
    private GameObject recruitmentStateImage;

    private ActionManager actionManagerScript;
    private RecruitmentUI recruitmentUIScript;
    private FCNavigation navigationScript;
    private FCExit exitScript;
    private FCEngineBehaviour engineScript;
    private Animator animatorPlanetUI;
    private Animator animatorPlanetBG;
    private Planet currentPlanet;
    private int fuelLevel;
    private int timeExpedition;
    private List<GameObject> recruits;

    private bool exploring = false;


    // Start is called before the first frame update
    void Start()
    {
        actionManagerScript = actionManager.GetComponent<ActionManager>();
        recruitmentUIScript = recruitmentPanel.GetComponent<RecruitmentUI>();
        navigationScript = navigationFacility.GetComponent<FCNavigation>();
        exitScript = exitFacility.GetComponent<FCExit>();
        engineScript = engineFacility.GetComponent<FCEngineBehaviour>();
        animatorPlanetUI = planetImage.GetComponent<Animator>();
        animatorPlanetBG = backgroundPlanet.GetComponent<Animator>();

        recruitButton.GetComponent<Button>().interactable = false;
        startButton.GetComponent<Button>().interactable = false;

        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void ShowNavigationUI()
    {
        panel.SetActive(true);
        GetComponentInParent<Canvas>().sortingOrder = UIManager.GetHighestSortingOrder();

        fuelRequiredText.text = "50% Fuel required to travel";

        updateFuel();

        if(currentPlanet != null)
        {
            int model = currentPlanet.getModel();
            animatorPlanetUI.SetFloat("planetModel", model);
            animatorPlanetUI.SetBool("OnPlanet", true);
        }
        

        InvokeRepeating("updateFuel", 0f, 1f);
    }

    public void travel()
    {
        currentPlanet = navigationScript.generatePlanet();

        if(currentPlanet != null)
        {  
            //cambiar al final de cuando termina de viajar
            int model = currentPlanet.getModel();
            animatorPlanetUI.SetFloat("planetModel", model);
            animatorPlanetBG.SetFloat("PlanetModel", model);
            animatorPlanetUI.SetBool("OnPlanet", true);
            animatorPlanetBG.SetBool("OnPlanet", true);

            string name = currentPlanet.GetName();
            planetName.text = name;

            Dictionary<Resource, int> obtainableResources = currentPlanet.GetObtainableResources();
            foreach (Transform child in resourceGridContent)
            {
                Destroy(child.gameObject);
            }
            foreach(Resource resource in obtainableResources.Keys)
            {
                GameObject newItem = Instantiate(iconPrefab, resourceGridContent);
                Image icon = newItem.GetComponent<Image>();
                Sprite resourceSprite = Resources.Load<Sprite>("Sprites/InventoryIcons/" + resource.ToString());
                icon.sprite = resourceSprite;
            }

            updateFuel();

            //reset expeditions
            recruitButton.GetComponent<Button>().interactable = true;
            startButton.GetComponent<Button>().interactable = false;
            changeRecruitmentState("empty");
            changeExpeditionState("not");
            recruitsRequiredText.text = "Must recruit crew mates before starting an expedition";

        }
        else
        {
            fuelRequiredText.text = "NOT ENOUGH FUEL TO TRAVEL";
        }
    }

    public void CloseNavigationUI()
    {
        panel.SetActive(false);
        CancelInvoke("updateFuel");
    }

    private void updateFuel()
    {
        fuelLevel = engineScript.getFuel();
        fuelText.text = "<b>Fuel: </b> " + fuelLevel + "%";
        fuelBar.fillAmount = (float)fuelLevel / 100;
    }

    private void updateExpeditionState()
    {
        timeExpedition = exitScript.getTime();
        recruitsRequiredText.text = "Exploring: " + timeExpedition + "s";
    }
    private void changeRecruitmentState(string option)
    {
        Image stateIcon = recruitmentStateImage.GetComponent<Image>();
        Sprite resourceSprite = Resources.Load<Sprite>("Sprites/UI/" + option);
        stateIcon.sprite = resourceSprite;
    }

    public void changeExpeditionState(string state)
    {
        Image startButtonIcon = startButton.GetComponent<Image>();
        if (state == "not")
        {
            Sprite startButtonSprite = Resources.Load<Sprite>("Sprites/UI/button6");
            startButtonIcon.sprite = startButtonSprite;
            startButtonText.text = "Start Expedition";
        }
        else if(state == "before")
        {
            recruitsRequiredText.text = "Waiting for crew to start expedition";
            travelButton.GetComponent<Button>().interactable = false;
            startButton.GetComponent<Button>().interactable = false;
            Sprite startButtonSprite = Resources.Load<Sprite>("Sprites/UI/button7");
            startButtonIcon.sprite = startButtonSprite;
            startButtonText.text = "Waiting for crew";
        }
        else if (state == "exploring")
        {
            InvokeRepeating("updateExpeditionState", 0f, 1f);
            startButton.GetComponent<Button>().interactable = true;
            startButtonText.text = "Finish Expedition";
        }
        else
        {
            CancelInvoke("updateExpeditionState");
            recruitsRequiredText.text = "Planet already explored";
            travelButton.GetComponent<Button>().interactable = true;
            startButton.GetComponent<Button>().interactable = false;
            startButtonText.text = "Explored";
        }
    }

    public void addRecruitedCM(List<GameObject> newRecruits)
    {
        recruits = newRecruits;
        recruitButton.GetComponent<Button>().interactable = false;
        if (recruits.Count > 0)
        {
            changeRecruitmentState("yes");
            recruitsRequiredText.text = recruits.Count + "/" + recruitmentUIScript.getMaxRecruits() + " recruited";
            startButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            changeRecruitmentState("no");
            recruitsRequiredText.text = "No crew member accepted the expedition. Can`t explore";
        }
    }

    public void clickExpedition()
    {
        if (!exploring)
        {
            startExpedition();
        }
        else
        {
            finishExpedition();
        }
    }


    private void startExpedition()
    {
        changeExpeditionState("before");

        foreach (GameObject crewMember in recruits)
        {
            GameAction newAction = actionManagerScript.createActionByName(ExploreAction.NAME, null, crewMember, true);
            crewMember.GetComponent<CMBehaviour>().updateActionList(newAction);
        }
        exitScript.startExpedition();

        exploring = true;
    }

    private void finishExpedition()
    {
        changeExpeditionState("explored");

        exitScript.claimRewards();

        exploring = false;

    }

    public void openRecruitmentPanel()
    {
        recruitmentUIScript.ShowRecruitmentUI();
    }

}

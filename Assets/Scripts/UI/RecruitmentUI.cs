using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecruitmentUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject navigationPanel;
    [SerializeField]
    private GameObject cmPrefab;
    [SerializeField]
    private Transform gridContent;
    [SerializeField]
    private GameObject ship;

    private NavigationUI navigationUIScript;
    private ShipBehaviour shipScript;
    private List<GameObject> crewMembers;
    private Dictionary<GameObject, GameObject> cmObjects;
    private List<GameObject> cmItemsSelected;

    private int maxRecruits = 3;

    private Color normalColor;
    private Color selectedColor;

    void Start()
    {

        shipScript = ship.GetComponent<ShipBehaviour>();
        navigationUIScript = navigationPanel.GetComponent<NavigationUI>();

        cmObjects = new Dictionary<GameObject, GameObject>();
        cmItemsSelected = new List<GameObject>();

        ColorUtility.TryParseHtmlString("#73B2A9", out normalColor);
        ColorUtility.TryParseHtmlString("#476D67", out selectedColor);

        panel.SetActive(false);
    }

    void Update()
    {

    }

    public void ShowRecruitmentUI()
    {
        panel.SetActive(true);
        cmObjects = new Dictionary<GameObject, GameObject>();
        cmItemsSelected = new List<GameObject>();

        load();
        GetComponentInParent<Canvas>().sortingOrder = UIManager.GetHighestSortingOrder();

    }

    public void CloseRecruitmentUI()
    {
        panel.SetActive(false);
    }

    private void load()
    {
        foreach (Transform child in gridContent)
        {
            Destroy(child.gameObject);
        }

        crewMembers = shipScript.getCrewMembers();

        foreach (var cm in crewMembers)
        {
            CMBehaviour cmScript = cm.GetComponent<CMBehaviour>();
            GameObject newItem = Instantiate(cmPrefab, gridContent);

            string name = cmScript.getName();

            newItem.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = name;

            Button buttonComponent = newItem.GetComponent<Button>();
            buttonComponent.onClick.AddListener(() => addCrew(newItem) );
            //Set sprite

            cmObjects[newItem] = cm;
        }
    }

    public void recruitCrew()
    {
        List<GameObject> cmObjectsSelected = new List<GameObject>();

        foreach(GameObject cmItem in cmItemsSelected)
        {
            GameObject cmObject = cmObjects[cmItem];
            cmObjectsSelected.Add(cmObject);
            cmObject.GetComponent<CMBehaviour>().explore();

        }

        navigationUIScript.addRecruitedCM(cmObjectsSelected);
        panel.SetActive(false);
    }

    public void addCrew(GameObject item)
    {
        if (!cmItemsSelected.Contains(item))
        {
            if(cmItemsSelected.Count < maxRecruits)
            {
                item.GetComponent<Image>().color = selectedColor;
                cmItemsSelected.Add(item);
            }
        }
        else
        {
            item.GetComponent<Image>().color = normalColor;
            cmItemsSelected.Remove(item);
        }
    }

    public int getMaxRecruits()
    {
        return maxRecruits;
    }
}

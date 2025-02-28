using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class RecruitmentUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject cmPrefab;
    [SerializeField]
    private Transform gridContent;
    [SerializeField]
    private GameObject ship;

    private ShipBehaviour shipScript;
    private List<GameObject> crewMembers;
    private Dictionary<GameObject, GameObject> cmObjects;
    private List<GameObject> cmItemsSelected;

    private Color normalColor;
    private Color selectedColor;

    void Start()
    {
        shipScript = ship.GetComponent<ShipBehaviour>();

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
        load();
        panel.SetActive(true);
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
        //lista para pasarla mas adelante a quien sea que maneje los tripulantes y quien muere y todo eso
        List<GameObject> cmObjectsSelected = new List<GameObject>();

        foreach(GameObject cmItem in cmItemsSelected)
        {
            GameObject cmObject = cmObjects[cmItem];
            cmObjectsSelected.Add(cmObject);
            cmObject.GetComponent<CMBehaviour>().explore();

        }

        //enviar tripulantes seleccionados a navegación
        panel.SetActive(false);
    }

    public void addCrew(GameObject item)
    {
        

        if (!cmItemsSelected.Contains(item))
        {
            item.GetComponent<Image>().color = selectedColor;
            cmItemsSelected.Add(item);
        }
        else
        {
            item.GetComponent<Image>().color = normalColor;
            cmItemsSelected.Remove(item);
        }
        Debug.Log(cmItemsSelected.Count);
    }

    
}

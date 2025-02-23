using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Globalization;
using UnityEngine.Windows;

public class InventoryUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject itemPrefab;
    [SerializeField]
    private Transform gridContent;
    [SerializeField]
    private GameObject ship;

    private ShipBehaviour shipScript;
    private Dictionary<Resource, int> inventoryResources;
    private Dictionary<Material, int> inventoryMaterials;

    // Start is called before the first frame update
    void Start()
    {
        shipScript = ship.GetComponent<ShipBehaviour>();

        
        UpdateInventoryUI();
        panel.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowInventoryUI(GameObject crewMate)
    {
        panel.SetActive(true);
    }
    public void CloseInventoryUI()
    {
        panel.SetActive(false);
    }

    public void UpdateInventoryUI()
    {
        inventoryResources = shipScript.GetInventoryResources();
        inventoryMaterials = shipScript.GetInventoryMaterials();

        foreach (Transform child in gridContent)
        {
            Destroy(child.gameObject);
        }

        foreach (var item in inventoryResources)
        {
            if (item.Value > 0)
            {
                GameObject newItem = Instantiate(itemPrefab, gridContent);

                string name = Regex.Replace(item.Key.ToString(), "(?<!^)([A-Z])", " $1");
                name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

                newItem.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = name;
                newItem.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = "x" + item.Value.ToString();

                // Cargar el sprite
                Image icon = newItem.transform.Find("Icon").GetComponent<Image>();
                Sprite resourceSprite = Resources.Load<Sprite>("Sprites/InventoryIcons/" + item.Key.getName());

                if (resourceSprite != null)
                {
                    icon.sprite = resourceSprite;
                }
                else
                {
                    Debug.Log("No se encontró el sprite para " + item.Key);
                }
            }
        }

        foreach (var item in inventoryMaterials)
        {
            if (item.Value > 0)
            {
                GameObject newItem = Instantiate(itemPrefab, gridContent);

                string name = Regex.Replace(item.Key.ToString(), "(?<!^)([A-Z])", " $1");
                name = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(name.ToLower());

                newItem.transform.Find("Name").GetComponent<TextMeshProUGUI>().text = name;
                newItem.transform.Find("Quantity").GetComponent<TextMeshProUGUI>().text = "x" + item.Value.ToString();

                // Cargar el sprite
                Image icon = newItem.transform.Find("Icon").GetComponent<Image>();
                Sprite resourceSprite = Resources.Load<Sprite>("Sprites/InventoryIcons/" + item.Key.getName());

                if (resourceSprite != null)
                {
                    icon.sprite = resourceSprite;
                }
                else
                {
                    Debug.Log("No se encontró el sprite para " + item.Key);
                }
            }
        }

    }

}

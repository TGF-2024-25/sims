using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainUI : MonoBehaviour
{

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject inventoryPanel;

    void Start()
    {
        panel.SetActive(true);

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenInventoryUI()
    {
        InventoryUI inventoryScript = inventoryPanel.GetComponent<InventoryUI>();
        inventoryScript.ShowInventoryUI();
    }
}

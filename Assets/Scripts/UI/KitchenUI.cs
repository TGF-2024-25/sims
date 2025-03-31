using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class KitchenUI : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private GameObject kitchenFacility;
    [SerializeField]
    private TextMeshProUGUI kitchenText;

    private FCKitchenBehaviour kitchenScript;
    private int food;

    // Start is called before the first frame update
    void Start()
    {
        kitchenScript = kitchenFacility.GetComponent<FCKitchenBehaviour>();

        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowKitchenUI()
    {
        panel.SetActive(true);
        GetComponentInParent<Canvas>().sortingOrder = UIManager.GetHighestSortingOrder();


        updateFood();

        InvokeRepeating("updateFood", 0f, 1f);
    }

    private void updateFood()
    {
        food = kitchenScript.getAvailableFood();
        kitchenText.text = "<b>Food Available: </b> " + food + " rations";
    }

    public void CloseKitchenUI()
    {
        panel.SetActive(false);
        CancelInvoke("updateFood");
    }
}

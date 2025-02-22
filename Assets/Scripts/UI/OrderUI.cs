using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using TMPro;
using Unity.Jobs;
using UnityEngine;

public class OrderUI : MonoBehaviour
{

    [SerializeField]
    private GameObject panel;
    [SerializeField]
    private TextMeshProUGUI titleText;
    [SerializeField]
    private TMP_InputField inputField;

    private CMBehaviour cmScript;

    // Start is called before the first frame update
    void Start()
    {
        panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowOrderUI(GameObject crewMate)
    {
        cmScript = crewMate.GetComponent<CMBehaviour>();
        string cmName = cmScript.getName();
        titleText.text = "Talking to " + cmName;
        panel.SetActive(true);
    }

    public void CloseOrderUI()
    {
        panel.SetActive(false);
    }

    public void SendOrder()
    {
        string userInput = inputField.text;
        cmScript.simulateOrder(userInput);
        panel.SetActive(false);
    }
}

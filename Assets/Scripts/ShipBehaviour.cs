using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameObject LabObject;
    private FCLabBehaviour labScript;
    [SerializeField]
    private GameObject WorkshopObject;
    private FCWorkshopBehaviour workshopScript;
    [SerializeField]
    private GameObject KitchenObject;
    private FCKitchenBehaviour kitchenScript;
    [SerializeField]
    private GameObject EngineObject;
    private FCEngineBehaviour engineScript;
    // Start is called before the first frame update

    void Awake()
    {
        labScript = LabObject.GetComponent<FCLabBehaviour>();
        workshopScript = WorkshopObject.GetComponent<FCWorkshopBehaviour>();
        kitchenScript = KitchenObject.GetComponent<FCKitchenBehaviour>();
        engineScript = EngineObject.GetComponent<FCEngineBehaviour>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

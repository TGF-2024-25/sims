using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FCKitchenBehaviour : FCBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Transform myTransform = transform;
        this.position = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

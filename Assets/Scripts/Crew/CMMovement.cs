using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CMMovement : MonoBehaviour
{
    public float speed;

    public Animator animator;

    private Vector2 targetPosition;
    // Start is called before the first frame update

    void Awake()
    {
        targetPosition = transform.position;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2f);
        }

    }


    public void setTargetPosition(Vector2 position)
    {
        this.targetPosition = position;
    }
}

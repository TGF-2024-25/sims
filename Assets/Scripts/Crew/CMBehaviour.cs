using System.Collections;
using UnityEngine;

public class CMBehaviour : MonoBehaviour
{
    private int x;
    private int y;

    private Vector2 targetPosition;
    private LLMManager llmManager;

    void Start()
    {
        llmManager = FindObjectOfType<LLMManager>();
        targetPosition = transform.position;
    }

    void Update()
    {
        if ((Vector2)transform.position != targetPosition)
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, Time.deltaTime * 2f);
        }
    }

    public void SendPromptToLLM(string prompt)
    {
        if (llmManager != null)
        {
            StartCoroutine(llmManager.SendRequestToGemini(prompt, this));
        }
    }

    public void UpdatePosition(int newX, int newY)
    {
        x = newX;
        y = newY;
        targetPosition = new Vector2(x, y);
    }
}
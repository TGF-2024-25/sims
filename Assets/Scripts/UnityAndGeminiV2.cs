using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class LLMManager : MonoBehaviour
{
    public TextAsset jsonApi;
    private string apiKey = "";
    private string apiEndpoint = "https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash-latest:generateContent";

    void Start()
    {
        UnityAndGeminiKey jsonApiKey = JsonUtility.FromJson<UnityAndGeminiKey>(jsonApi.text);
        apiKey = jsonApiKey.key;
    }

    public IEnumerator SendRequestToGemini(string promptText, CMBehaviour tripulante)
    {
        string url = $"{apiEndpoint}?key={apiKey}";
        string jsonData = "{\"contents\": [{\"parts\": [{\"text\": \"" + promptText + "\"}]}]}";

        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(jsonData);

        using (UnityWebRequest www = new UnityWebRequest(url, "POST"))
        {
            www.uploadHandler = new UploadHandlerRaw(jsonToSend);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Request complete!");
                Response response = JsonUtility.FromJson<Response>(www.downloadHandler.text);
                if (response.candidates.Length > 0 && response.candidates[0].content.parts.Length > 0)
                {
                    string text = response.candidates[0].content.parts[0].text;
                    Debug.Log(text);
                    ParseOutput(text, tripulante);
                }
                else
                {
                    Debug.Log("No text found.");
                }
            }
        }
    }

    public void ParseOutput(string output, CMBehaviour tripulante)
    {
        int newX = 0;
        int newY = 0;

        if (output.Contains("investigar_recetas"))
        {
            newX = 6;
            newY = 4;
        }
        else if (output.Contains("cocinar_desayuno"))
        {
            newX = 3;
            newY = 5;
        }
        else if (output.Contains("cocinar_cena"))
        {
            newX = 2;
            newY = 7;
        }
        else if (output.Contains("charlar_tripulacion"))
        {
            newX = 1;
            newY = 1;
        }
        else if (output.Contains("descansar"))
        {
            newX = 0;
            newY = 0;
        }

        tripulante.UpdatePosition(newX, newY);
    }
}

[System.Serializable]
public class UnityAndGeminiKey
{
    public string key;
}

[System.Serializable]
public class Response
{
    public Candidate[] candidates;
}

[System.Serializable]
public class Candidate
{
    public Content content;
}

[System.Serializable]
public class Content
{
    public Part[] parts;
}

[System.Serializable]
public class Part
{
    public string text;
}
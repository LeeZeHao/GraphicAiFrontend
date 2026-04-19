using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class PollCurrentTextGenScript : MonoBehaviour
{
    [SerializeField] SettingsScript settingsScript;
    [SerializeField] DialogBoxScript dialogBoxScript;
    [SerializeField] int requestTimeout = 3;

    private bool shouldPoll = false;
    private Coroutine pollingCoroutine;
    private string url = "http://localhost:5001";
    private string endpointUrl = "http://localhost:5001/api/extra/generate/check";
    public float intervalSeconds = 0.1f;
    private void GetSettings()
    {
        this.url = settingsScript.url;
        this.endpointUrl = settingsScript.url + "/api/extra/generate/check";
    }

    public void StartPolling()
    {
        GetSettings();
        shouldPoll = true;
        if (pollingCoroutine == null)
            pollingCoroutine = StartCoroutine(GetRequestLoop());
    }

    public void StopPolling()
    {
        shouldPoll = false;
        if (pollingCoroutine != null)
        {
            StopCoroutine(pollingCoroutine);
            pollingCoroutine = null;
        }
    }

    IEnumerator GetRequestLoop()
    {
        while (true)
        {
            yield return StartCoroutine(GetRequest());
            yield return new WaitForSeconds(intervalSeconds);
        }
    }

    IEnumerator GetRequest()
    {
        using (UnityWebRequest request = UnityWebRequest.Get(endpointUrl))
        {
            request.timeout = requestTimeout;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error: " + request.error);
            }
            else
            {
                string json = request.downloadHandler.text;

                ResponseWrapper data = JsonUtility.FromJson<ResponseWrapper>(json);

                if (data.results != null && data.results.Length > 0)
                {
                    string text = data.results[0].text;
                    Debug.Log("Extracted text: " + text);
                    if (shouldPoll) // Check so that we do not override the final result
                    {
                        dialogBoxScript.UpdatePollCurrentTextGen(text);
                    }
                }
            }
        }
    }

    [System.Serializable]
    public class Result
    {
        public string text;
    }

    [System.Serializable]
    public class ResponseWrapper
    {
        public Result[] results;
    }
}

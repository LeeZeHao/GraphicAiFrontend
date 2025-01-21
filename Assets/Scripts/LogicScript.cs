using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class LogicScript : MonoBehaviour
{
    private List<string> emotions = new List<string> { "Neutral" };

    [SerializeField] ObserverInterface[] observers;
    [SerializeField] SettingsScript settingsScript;
    [SerializeField] DialogTextHandlerScript dialogTextHandlerScript;
    [SerializeField] StatusScript statusScript;

    [SerializeField] TMP_InputField sendInputField;
    [SerializeField] Button sendButton;
    [SerializeField] Button[] actionButtons;

    private string url = "http://localhost:5001";
    private float temperature = 0.75f;
    private float repPen = 1.07f;

    private void GetSettings() { 
        url = settingsScript.url;
        temperature = settingsScript.temperature;
        repPen = settingsScript.repPen;
    }

    public void onClickSendButton() {
        Send(sendInputField.text);
    }

    public void Send(string userMessage) {

        sendInputField.text = userMessage;

        GetSettings();

        // clean input
        userMessage = userMessage.Trim();
        if (userMessage.Length <= 0) {
            return;
        }

        string message = userMessage;
        string prompt = dialogTextHandlerScript.GeneratePrompt(message);

        // Generate JSON to send
        GenerateRequestObject generateRequestObject = new GenerateRequestObject();
        generateRequestObject.prompt = prompt;
        generateRequestObject.temperature = temperature; 
        generateRequestObject.rep_pen = repPen;
        string generateRequestString = JsonUtility.ToJson(generateRequestObject);

        // disable send button
        sendInputField.interactable = false;
        sendButton.interactable = false;
        foreach (Button button in actionButtons) {
            button.interactable = false;
        }

        StartCoroutine(PostGenerateResponse(generateRequestString));
    }

    IEnumerator PostGenerateResponse(string data) {

        Debug.Log("Sending to " + url + "/api/v1/generate");
        Debug.Log(data);

        foreach (ObserverInterface observer in observers) {
            if (observer == null) {
                continue;
            } else {
                observer.Waiting();
            }
        }

        UnityWebRequest www = UnityWebRequest.Post(url + "/api/v1/generate", data, "application/json");

        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success) {

            foreach (ObserverInterface observer in observers) {
                if (observer == null) {
                    continue;
                } else {
                    observer.ServerError(www.error);
                }
            }

        } else {

            // Clean result string
            string result = www.downloadHandler.text;

            result = result.Split("\"text\": \"")[1];
            result = result.Split("\", \"finish_reason\"")[0];

            StartCoroutine(PostCheckSentiment(result));

        }
        www.Dispose();
    }

    // Use the LLM to check the emotion!
    IEnumerator PostCheckSentiment(string response) {
        this.emotions = settingsScript.emotions;

        // make the emotion checking prompt
        string emotionPrompt = "</s>[INST]One word response. Is this ";
        foreach (string emotion in emotions) {
            emotionPrompt += ", " + emotion;
        }
        emotionPrompt += "? \"" + response + "\"[/INST]";

        Debug.Log(emotionPrompt);

        GenerateRequestObject generateRequestObject = new GenerateRequestObject();
        generateRequestObject.prompt = emotionPrompt;
        generateRequestObject.temperature = 0.75f;
        generateRequestObject.rep_pen = 1.07f;
        string generateRequestString = JsonUtility.ToJson(generateRequestObject);

        UnityWebRequest www = UnityWebRequest.Post(settingsScript.url + "/api/v1/generate", generateRequestString, "application/json");

        yield return new WaitForSeconds(0.1f);

        yield return www.SendWebRequest();

        int responseMood = -1;

        if (www.result != UnityWebRequest.Result.Success) {
            Debug.Log("responseMood fail: " + responseMood);

            foreach (ObserverInterface observer in observers) {
                if (observer == null) {
                    continue;
                } else {
                    observer.UpdateObserver(response, -1);
                    dialogTextHandlerScript.StoreResponse(response);
                }
            }
        } else {

            // Clean result string
            string result = www.downloadHandler.text;
            result = result.Split("\"text\": \"")[1];
            result = result.Split("\", \"finish_reason\"")[0];
            result = result.Trim();
            result = result.ToLower();

            Debug.Log("Emotion result: " + result);

            // Check emotion
            for (int i = 0; i < emotions.Count; i++) {
                if (result.Contains(emotions[i].ToLower())) {
                    responseMood = i;
                    break;
                }
            }

            Debug.Log("responseMood success: " + responseMood);

            foreach (ObserverInterface observer in observers) {
                if (observer == null) {
                    continue;
                } else {
                    observer.UpdateObserver(response, responseMood);
                    dialogTextHandlerScript.StoreResponse(response);
                }
            }
        }

        // re-enable send button 
        sendInputField.text = "";
        sendInputField.interactable = true;
        sendButton.interactable = true;
        foreach (Button button in actionButtons) {
            button.interactable = true;
        }
        www.Dispose();
    }

    // Private class for dealing with making JSON for /api/v1/generate
    private class GenerateRequestObject {
        public string prompt;
        public float temperature;
        public float rep_pen;
    }

    // For when the user has just exited settings page for the first time (display greeting / continuation of last dialog)
    public void JustBootedDisplay() {
        this.emotions = settingsScript.emotions;
        if (settingsScript.isFolderInit) {
            StartCoroutine(DelayBootedDisplay());
        }
    }

    IEnumerator DelayBootedDisplay() {
        foreach (ObserverInterface observer in observers) {
            if (observer == null) {
                continue;
            } else {
                observer.UpdateObserver("...", -1);
            }
        }

        yield return new WaitForSeconds(1);

        string displayDialog = dialogTextHandlerScript.GetLatestResponse();
        foreach (ObserverInterface observer in observers) {
            if (observer == null) {
                continue;
            } else {
                observer.UpdateObserver(displayDialog, statusScript.GetMood());
            }
        }
    }
}

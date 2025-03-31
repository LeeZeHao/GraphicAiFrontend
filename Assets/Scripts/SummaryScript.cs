using System;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class SummaryScript : MonoBehaviour
{
    [SerializeField] private Canvas summaryCanvas;

    [SerializeField] private SettingsScript settingsScript;
    [SerializeField] private DialogTextHandlerScript dialogTextHandlerScript;
    [SerializeField] private ContextScript contextScript;

    [SerializeField] private TMP_Text saveStatusText;
    [SerializeField] private TMP_Text summarySaveLocationText;
    [SerializeField] private Button summaryCloseButton;
    [SerializeField] private Button summarySaveButton;
    [SerializeField] private TMP_Text useSummaryButtonText;

    private string url = "http://localhost:5001";
    private float temperature = 0.75f;
    private float repPen = 1.07f;

    private string folder = Application.dataPath + "/character";

    private string botName = "";

    private string summary = "";
    [HideInInspector] public bool isUsingSummary = true;

    private void GetSettings()
    {
        url = settingsScript.url;
        temperature = settingsScript.temperature;
        repPen = settingsScript.repPen;
    }

    public void OnClickSummaryButton()
    {
        GetSettings();
        botName = contextScript.botName;
        folder = settingsScript.folder;
        summarySaveLocationText.text = folder + "/SummarySave.txt";
        summaryCanvas.gameObject.SetActive(true);
    }
    public void OnClickSummaryCloseButton()
    {
        summaryCanvas.gameObject.SetActive(false);
    }

    public void OnClickUseSummaryButton()
    {
        if (isUsingSummary)
        {
            useSummaryButtonText.text = "Not using summary";
            isUsingSummary = false;
        }
        else
        {
            useSummaryButtonText.text = "Using prev. summary";
            isUsingSummary = true;
        }
    }

    public void GenerateSummary()
    {
        botName = contextScript.botName;
        if (!settingsScript.isFolderInit)
        {
            saveStatusText.text = "Character folder is not set in Settings!";
            return;
        }

        saveStatusText.text = "Generating dialog summary...";
        Send(dialogTextHandlerScript.GetOnlyDialogAsString());
    }

    private void Send(string userMessage)
    {
        GetSettings();

        // clean input
        userMessage = userMessage.Trim();
        if (userMessage.Length <= 0)
        {
            return;
        }

        string prompt = userMessage;

        // Generate JSON to send
        GenerateRequestObject generateRequestObject = new GenerateRequestObject();
        generateRequestObject.prompt = "</s>[INST]In 100 words, summarize the following dialog between " + botName + " and the user in paragraph form: \n" + prompt + "[/INST]";
        generateRequestObject.temperature = temperature;
        generateRequestObject.rep_pen = repPen;
        string generateRequestString = JsonUtility.ToJson(generateRequestObject);

        // disable send button
        summaryCloseButton.interactable = false;
        summarySaveButton.interactable = false;

        StartCoroutine(PostGetSummary(generateRequestString));
    }

    // Use the LLM to generate a summary!
    IEnumerator PostGetSummary(string data)
    {
        Debug.Log("Sending to " + url + "/api/v1/generate");
        Debug.Log(data);

        UnityWebRequest www = UnityWebRequest.Post(settingsScript.url + "/api/v1/generate", data, "application/json");

        yield return new WaitForSeconds(0.1f);

        yield return www.SendWebRequest();

        string summaryResult = "";

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.Log("summarization failed: " + www.result);

            saveStatusText.text = "LLM dialog summarization failed!";
        }
        else
        {
            // Clean result string
            summaryResult = www.downloadHandler.text;

            summaryResult = summaryResult.Split("\"text\": \"")[1];
            summaryResult = summaryResult.Split("\", \"finish_reason\"")[0];

            summaryResult = summaryResult.Trim();

            Debug.Log("Dialog summary: " + summaryResult);
        }

        // save the summary
        this.summary = summaryResult;
        SaveSummary();

        // make buttons interactible, reset save status text to blank and make summary canvas inactive
        summaryCloseButton.interactable = true;
        summarySaveButton.interactable = true;
        saveStatusText.text = "";
        summaryCanvas.gameObject.SetActive(false);

        www.Dispose();
    }

    // Private class for dealing with making JSON for /api/v1/generate
    private class GenerateRequestObject
    {
        public string prompt;
        public float temperature;
        public float rep_pen;
    }

    private void SaveSummary()
    {
        if (!settingsScript.isFolderInit)
        {
            return;
        }

        SummarySaveObject summarySaveObject = new SummarySaveObject();
        summarySaveObject.summary = summary;
        string json = JsonUtility.ToJson(summarySaveObject);

        File.WriteAllText(folder + "/SummarySave.txt", json);
    }

    public void LoadSummary()
    {

        if (!settingsScript.isFolderInit)
        {
            return;
        }

        folder = settingsScript.folder;

        if (!File.Exists(folder + "/SummarySave.txt"))
        {
            saveStatusText.text = "No previous summary save, creating new save at " + folder + "/SummarySave.txt";
            SaveSummary();
            return;
        }

        string summarySaveString = File.ReadAllText(folder + "/SummarySave.txt");
        try
        {
            SummarySaveObject summarySaveObject = JsonUtility.FromJson<SummarySaveObject>(summarySaveString);
            this.summary = summarySaveObject.summary;

            saveStatusText.text = "Previous summary exists. (" + folder + "/SummarySave.txt)";
        }
        catch
        {
            saveStatusText.text = "Previous summary unreadable due to bad JSON format, creating new save at " + folder + "/SummarySave.txt";
            SaveSummary();
        }
    }

    // Lets any class get a copy of the summary text.
    public string GetSummaryString()
    {
        LoadSummary();
        return summary;
    }

    private class SummarySaveObject
    {
        public string summary;
    }


}

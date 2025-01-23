using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

// Example
/// <s>[INST]Senko from The Helpful Fox Senko-San, an 800 year old kitsune fox girl, 
/// she is cute and asks a lot of questions![/INST]Hi, I'm Senko! Did you need anything?</s>[INST]Yes, what is the capital of the US?[/INST]

public class DialogTextHandlerScript : MonoBehaviour {

    [SerializeField] Canvas logCanvas;
    [SerializeField] TMP_Text logText;
    [SerializeField] TMP_Text logSaveLocationText;

    [SerializeField] Canvas resetCanvas;
    [SerializeField] TMP_Text resetSaveLocationText;

    [SerializeField] SettingsScript settingsScript;
    [SerializeField] ContextScript contextScript;
    [SerializeField] StatusScript statusScript;

    private string folder = Application.dataPath + "/character";

    private string format1 = " ";
    private string format2 = " ";
    private string format3 = " ";
    private string format4 = " ";

    private string botName = "Bot";
    private string context = "";
    private string firstDialog = "";
    private List<string> messages = new List<string>();
    private List<string> responses = new List<string>();

    private string lastMessage = "";

    private bool shouldStoreResponse = false;

    public string GeneratePrompt(string message) {
        // Load to ensure it is the accurate version of the dialog
        LoadDialog();

        lastMessage = message;
        GetFormats();
        GetContext();

        string finalPrompt = "";

        finalPrompt += format1;
        finalPrompt += context;
        finalPrompt += format2;
        finalPrompt += firstDialog;
        finalPrompt += format3;

        for (int i = 0; i < messages.Count; i++) {
            finalPrompt += messages[i];
            finalPrompt += format4;
            finalPrompt += responses[i];
            finalPrompt += format3;
        }

        finalPrompt += message;
        finalPrompt += format4;

        shouldStoreResponse = true;
        return finalPrompt;
    }

    public void StoreResponse(string response) {
        if (!shouldStoreResponse) {
            return;
        }

        messages.Add(lastMessage);
        responses.Add(response);
        // Save to keep dialog file up to date
        SaveDialog();

        shouldStoreResponse = false;
    }

    public string GetLatestResponse() {
        GetContext();
        if (responses.Count == 0) {
            return firstDialog;
        } else {
            return responses[responses.Count - 1];
        }
    }

    private void GetFormats() {
        this.format1 = settingsScript.format1;
        this.format2 = settingsScript.format2;
        this.format3 = settingsScript.format3;
        this.format4 = settingsScript.format4;
    }

    private void GetContext() {
        this.botName = contextScript.botName;
        this.context = contextScript.context;
        this.firstDialog = contextScript.firstDialog;
    }

    public void LoadDialog() {
        folder = settingsScript.folder;

        if (!File.Exists(folder + "/DialogSave.txt")) {
            statusScript.ResetStatus();
            SaveDialog();
            return;
        }

        string dialogSaveString = File.ReadAllText(folder + "/DialogSave.txt");
        try {
            DialogSaveObject dialogSaveObject = JsonUtility.FromJson<DialogSaveObject>(dialogSaveString);
            this.messages = dialogSaveObject.messages;
            this.responses = dialogSaveObject.responses;
        } catch {
            SaveDialog();
        }
    }

    private void SaveDialog() {
        folder = settingsScript.folder;

        DialogSaveObject dialogSaveObject = new DialogSaveObject();
        dialogSaveObject.messages = messages;
        dialogSaveObject.responses = responses;
        string json = JsonUtility.ToJson(dialogSaveObject);

        File.WriteAllText(folder + "/DialogSave.txt", json);
    }

    // JSON template object that saves two Lists of strings.
    private class DialogSaveObject {
        public List<string> messages;
        public List<string> responses;
    }

    public void OnClickLogButton() {
        GetContext();
        LoadDialog();

        string logString = "";
        logString += botName + " : " + firstDialog + "\n\n";
        for (int i = 0; i < messages.Count; i++) {
            logString += messages[i];
            logString += "\n\n";
            logString += botName + ": " + responses[i];
            logString += "\n\n";
        }

        logText.text = logString;
        logSaveLocationText.text = "Note: Dialog file saved at " + folder + "/DialogSave.txt";

        logCanvas.gameObject.SetActive(true);
    }

    public void OnClickLogCloseButton() {
        logCanvas.gameObject.SetActive(false);
    }

    public void OnClickResetButton() {
        GetContext();
        LoadDialog();

        resetSaveLocationText.text = folder + "/DialogSave.txt";

        resetCanvas.gameObject.SetActive(true);
    }

    public void OnClickConfirmReset() {
        this.messages = new List<string>();
        this.responses = new List<string>();

        SaveDialog();
        resetCanvas.gameObject.SetActive(false);
    }

    public void OnClickCancelReset() {
        resetCanvas.gameObject.SetActive(false);
    }
}

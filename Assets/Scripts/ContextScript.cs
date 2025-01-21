using System;
using System.IO;
using TMPro;
using UnityEngine;

public class ContextScript : MonoBehaviour
{
    [SerializeField] private Canvas contextCanvas;

    [SerializeField] private SettingsScript settingsScript;

    [SerializeField] private TMP_InputField nameInputField;
    [SerializeField] private TMP_InputField contextInputField;
    [SerializeField] private TMP_InputField firstDialogInputField;
    [SerializeField] private TMP_Text nameText;

    [SerializeField] private TMP_Text saveStatusText;

    [HideInInspector] public string botName = "Bot";
    [HideInInspector] public string context = "Bot is helpful to humans.";
    [HideInInspector] public string firstDialog = "I'm Bot, What would you like to talk about?";

    private string folder = Application.dataPath + "/character";
    public void OnClickContextButton() {
        LoadContext();
        contextCanvas.gameObject.SetActive(true);
    }

    public void OnClickContextCloseButton() {
        SaveContext();
        contextCanvas.gameObject.SetActive(false);
    }

    public void LoadContext() {

        if (!settingsScript.isFolderInit) {
            return;
        }

        folder = settingsScript.folder;

        if (!File.Exists(folder + "/ContextSave.txt")) {
            saveStatusText.text = "No previous context save, creating new save at " + folder + "/ContextSave.txt";
            SaveContext();
            return;
        }

        string contextSaveString = File.ReadAllText(folder + "/ContextSave.txt");
        try {
            ContextSaveObject contextSaveObject = JsonUtility.FromJson<ContextSaveObject>(contextSaveString);
            this.botName = contextSaveObject.botName;
            this.context = contextSaveObject.context;
            this.firstDialog = contextSaveObject.firstDialog;

            nameInputField.text = botName;
            contextInputField.text = context;
            firstDialogInputField.text = firstDialog;
            nameText.text = botName;

            saveStatusText.text = "Previous context loaded. (" + folder + "/ContextSave.txt)";
        } catch {
            saveStatusText.text = "Load failed due to bad JSON format, creating new save at " + folder + "/ContextSave.txt";
            SaveContext();
        }
    }

    private void SaveContext() {
        if (!settingsScript.isFolderInit) {
            return;
        }

        botName = nameInputField.text;
        context = contextInputField.text;
        firstDialog = firstDialogInputField.text;
        nameText.text = botName;

        ContextSaveObject contextSaveObject = new ContextSaveObject();
        contextSaveObject.botName = botName;
        contextSaveObject.context = context;
        contextSaveObject.firstDialog = firstDialog;
        string json = JsonUtility.ToJson(contextSaveObject);

        File.WriteAllText(folder + "/ContextSave.txt", json);
    }

    private class ContextSaveObject {
        public string botName;
        public string context;
        public string firstDialog;
    }
}

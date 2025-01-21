using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class SettingsScript : MonoBehaviour
{

    [SerializeField] private Canvas settingsCanvas;

    [SerializeField] private Button urlButton;
    [SerializeField] private TMP_InputField urlInputField;
    [SerializeField] private TMP_Text urlStatusText;

    [SerializeField] private ImagesScript imagesScript;
    [SerializeField] private TMP_InputField folderInputField;
    [SerializeField] private TMP_Text folderStatusText;

    [SerializeField] private TMP_InputField temperatureInputField;
    [SerializeField] private TMP_InputField repPenInputField;

    [SerializeField] private TMP_InputField formatInputField1;
    [SerializeField] private TMP_InputField formatInputField2;
    [SerializeField] private TMP_InputField formatInputField3;
    [SerializeField] private TMP_InputField formatInputField4;

    [SerializeField] private TMP_InputField emotionInputField0;
    [SerializeField] private TMP_InputField emotionInputField1;
    [SerializeField] private TMP_InputField emotionInputField2;
    [SerializeField] private TMP_InputField emotionInputField3;
    [SerializeField] private TMP_InputField emotionInputField4;
    [SerializeField] private TMP_InputField emotionInputField5;

    [SerializeField] private TMP_Text saveStatusText;

    [SerializeField] private TMP_Text closeStatusText;

    [HideInInspector] public string url = "http://localhost:5001";
    [HideInInspector] public string folder = Application.dataPath + "/character";
    [HideInInspector] public float temperature = 0.75f;
    [HideInInspector] public float repPen = 1.07f;

    [HideInInspector] public List<string> emotions = new List<string> { "neutral" };

    [HideInInspector] public string format1 = "<s>[INST]";
    [HideInInspector] public string format2 = "[/INST]";
    [HideInInspector] public string format3 = "</s>[INST]";
    [HideInInspector] public string format4 = "[/INST]";

    [HideInInspector] public bool isUrlConnected = false;
    [HideInInspector] public bool isFolderInit = false;

    private void Awake() {
        urlInputField.text = url;
        folderInputField.text = folder;
        temperatureInputField.text = temperature.ToString();
        repPenInputField.text = repPen.ToString();

        formatInputField1.text = format1;
        formatInputField2.text = format2;
        formatInputField3.text = format3;
        formatInputField4.text = format4;

        LoadSettings();
    }

    public void OnClickSettingsButton() {
        // Set back to false so user has to confirm again next time they open settings
        isUrlConnected = false;
        isFolderInit = false;
        urlStatusText.text = "No backend connected...";
        folderStatusText.text = "Folder not initialized...";

        LoadSettings();
        settingsCanvas.gameObject.SetActive(true);
    }

    public void OnClickUrlButton() {
        urlButton.interactable = false;

        urlInputField.text = urlInputField.text.Replace(" ", "");
        urlInputField.text = urlInputField.text.Trim('/');
        urlInputField.text = urlInputField.text.Trim('\\');
        urlInputField.text = urlInputField.text.Trim('#');

        if (urlInputField.text.Length == 0 ) {
            urlStatusText.text = "Nothing entered in the URL text field!";
            urlButton.interactable = true;
            return;
        }

        url = urlInputField.text;
        Debug.Log("Testing url: " +  url);

        StartCoroutine(TestUrl());
    } 

    public void OnClickFolderButton() {
        folderInputField.text = folderInputField.text.Replace(" ", "");
        folderInputField.text = folderInputField.text.Trim('/');
        folderInputField.text = folderInputField.text.Trim('\\');
        folderInputField.text = folderInputField.text.Trim('#');

        if (folderInputField.text.Length == 0) {
            folderStatusText.text = "Nothing entered in the folder text field!";
            return;
        }

        folder = folderInputField.text;
        RefreshEmotions();

        folderStatusText.text = "Initializing images and save files...";

        if (imagesScript.InitImages(folder, emotions) == false) {
            folderStatusText.text = "Unable to load images, check if folder path correct + all images exist and are named properly!";
        } else {
            folderStatusText.text = "Folder initialized, images, context, actions loaded.";
            isFolderInit = true;
        }
    }

    // If the user changes a value at the emotions, then we need to tell them to re-int folder
    public void OnEmotionValueChanged(string newValue) {
        if (isFolderInit) {
            isFolderInit = false;
            folderStatusText.text = "Emotions value changed, need to re-initialize folder!";
        }
    }

    private void RefreshEmotions() {
        emotions.Clear();
        if (emotionInputField0.text.Trim().Length > 0) {
            emotions.Add(emotionInputField0.text.Trim());
        }
        if (emotionInputField1.text.Trim().Length > 0) {
            emotions.Add(emotionInputField1.text.Trim());
        }
        if (emotionInputField2.text.Trim().Length > 0) {
            emotions.Add(emotionInputField2.text.Trim());
        }
        if (emotionInputField3.text.Trim().Length > 0) {
            emotions.Add(emotionInputField3.text.Trim());
        }
        if (emotionInputField4.text.Trim().Length > 0) {
            emotions.Add(emotionInputField4.text.Trim());
        }
        if (emotionInputField5.text.Trim().Length > 0) {
            emotions.Add(emotionInputField5.text.Trim());
        }

        // Cannot have 0 emotions, if none exist then there must be neutral
        if (emotions.Count <= 0) {
            emotions = new List<string> { "neutral" };
        }
        DisplayEmotions();
    }

    private void DisplayEmotions() {
        int i = 0;

        if (i < emotions.Count) {
            emotionInputField0.text = emotions[i];
            i++;
        } else {
            emotionInputField0.text = "";
        }
        if (i < emotions.Count) {
            emotionInputField1.text = emotions[i];
            i++;
        } else {
            emotionInputField1.text = "";
        }
        if (i < emotions.Count) {
            emotionInputField2.text = emotions[i];
            i++;
        } else {
            emotionInputField2.text = "";
        }
        if (i < emotions.Count) {
            emotionInputField3.text = emotions[i];
            i++;
        } else {
            emotionInputField3.text = "";
        }
        if (i < emotions.Count) {
            emotionInputField4.text = emotions[i];
            i++;
        } else {
            emotionInputField4.text = "";
        }
        if (i < emotions.Count) {
            emotionInputField5.text = emotions[i];
            i++;
        } else {
            emotionInputField5.text = "";
        }
    }

    public void OnClickSettingsCloseButton() {
        if (!isUrlConnected) {
            closeStatusText.text = "Please connect to a valid URL first! (Press the 'Connect' button too)";
        } else if (!isFolderInit) {
            closeStatusText.text = "Please set a valid character folder first! (Press the 'Init' button too)";
        } else {
            // Save all settings
            SaveSettings();

            closeStatusText.text = "";
            settingsCanvas.gameObject.SetActive(false); 
            
        }
    }

    private IEnumerator TestUrl() {
        using (UnityWebRequest request = UnityWebRequest.Get(url + "/api/v1/model")) {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success) {

                string json = request.downloadHandler.text;
                ModelNameObject modelNameObject = JsonUtility.FromJson<ModelNameObject>(json);

                urlStatusText.text = "Connected, current model is: " + modelNameObject.result;
                isUrlConnected = true;

            } else {
                urlStatusText.text = "Model connection failed! Please check entered URL.";
                isUrlConnected = false;
            }
            urlButton.interactable = true;
        }
    }

    private void LoadSettings() {
        if (!File.Exists(Application.dataPath + "/SettingsSave.txt")) {
            saveStatusText.text = "No previous settings, creating new save at " + Application.dataPath + "/SettingsSave.txt";
            SaveSettings();
            return;
        }

        string settingsSaveString = File.ReadAllText(Application.dataPath + "/SettingsSave.txt");
        try {
            SettingsSaveObject settingsSaveObject = JsonUtility.FromJson<SettingsSaveObject>(settingsSaveString);
            this.url = settingsSaveObject.url;
            this.folder = settingsSaveObject.folder;
            this.temperature = settingsSaveObject.temperature;
            this.repPen = settingsSaveObject.repPen;

            this.format1 = settingsSaveObject.format1;
            this.format2 = settingsSaveObject.format2;
            this.format3 = settingsSaveObject.format3;
            this.format4 = settingsSaveObject.format4;

            this.emotions = settingsSaveObject.emotions;
            DisplayEmotions();

            urlInputField.text = url;
            folderInputField.text = folder;
            temperatureInputField.text = temperature.ToString();
            repPenInputField.text = repPen.ToString();

            saveStatusText.text = "Previous settings loaded. (" + Application.dataPath + "/SettingsSave.txt)";
        } catch {
            saveStatusText.text = "Load failed due to bad JSON format, creating new save at " + Application.dataPath + "/SettingsSave.txt";
            SaveSettings();
        }
    }

    private void SaveSettings() {
        // need to grab values of the float input fields and formats too!
        float.TryParse(temperatureInputField.text, out this.temperature);
        float.TryParse(repPenInputField.text, out this.repPen);
        format1 = formatInputField1.text;
        format2 = formatInputField2.text;
        format3 = formatInputField3.text;
        format4 = formatInputField4.text;
        RefreshEmotions();

        SettingsSaveObject settingsSaveObject = new SettingsSaveObject();
        settingsSaveObject.url = url;
        settingsSaveObject.folder = folder;
        settingsSaveObject.temperature = temperature;
        settingsSaveObject.repPen = repPen;
        settingsSaveObject.format1 = format1;
        settingsSaveObject.format2 = format2;
        settingsSaveObject.format3 = format3;
        settingsSaveObject.format4 = format4;

        settingsSaveObject.emotions = emotions;

        string json = JsonUtility.ToJson(settingsSaveObject);

        File.WriteAllText(Application.dataPath + "/SettingsSave.txt", json);
    }


    // For parsing and making JSON.
    private class SettingsSaveObject {
        public string url;
        public string folder;

        public float temperature;
        public float repPen;

        public string format1;
        public string format2;
        public string format3;
        public string format4;

        public List<string> emotions;
    }

    private class ModelNameObject {
        public string result;
    }
}

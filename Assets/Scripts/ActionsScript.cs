using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class ActionsScript : MonoBehaviour
{
    [SerializeField] LogicScript logicScript;
    [SerializeField] SettingsScript settingsScript;
    [SerializeField] StatusScript statusScript;

    [SerializeField] TMP_Text responseText;
    [SerializeField] Canvas actionsCanvas;

    [SerializeField] TMP_Text saveStatusText;

    // actions for switching between body0-3
    [SerializeField] TMP_InputField actionInputField0;
    [SerializeField] TMP_InputField actionInputField1;
    [SerializeField] TMP_InputField actionInputField2;
    [SerializeField] TMP_InputField actionInputField3;

    // for Websites
    [SerializeField] TMP_Dropdown websiteDropdown;
    [SerializeField] Button deleteWebsiteButton;
    [SerializeField] TMP_InputField websiteNameInput;
    [SerializeField] TMP_InputField websiteLinkInput;
    [SerializeField] TMP_InputField websitePromptInput;
    [SerializeField] TMP_Dropdown openWebsiteDropdown;

    private int currentWebsiteValue = 0;
    private List<string> websiteNames = new List<string> { "Example Site" };
    private List<string> websiteLinks = new List<string> { "https://example.com/" };
    private List<string> websitePrompts = new List<string> { "I'm opening the site. *I open example.com.*" };

    private string folder = Application.dataPath + "/character";

    public void OnClickActionsButton() {
        LoadActions();
        actionsCanvas.gameObject.SetActive(true);
    }

    public void OnClickActionsCloseButton() {
        SaveActions();
        openWebsiteDropdown.ClearOptions();
        openWebsiteDropdown.AddOptions(websiteNames);
        openWebsiteDropdown.RefreshShownValue();

        actionsCanvas.gameObject.SetActive(false);
    }

    public void LoadActions() {
        folder = settingsScript.folder;

        if (!File.Exists(folder + "/ActionsSave.txt")) {
            saveStatusText.text = "No previous actions save, creating new save at " + folder + "/ActionsSave.txt";
            SaveActions();
            return;
        }

        string actionsSaveString = File.ReadAllText(folder + "/ActionsSave.txt");
        try {
            ActionsSaveObject actionsSaveObject = JsonUtility.FromJson<ActionsSaveObject>(actionsSaveString);

            actionInputField0.text = actionsSaveObject.body0;
            actionInputField1.text = actionsSaveObject.body1;
            actionInputField2.text = actionsSaveObject.body2;
            actionInputField3.text = actionsSaveObject.body3;

            websiteNames = actionsSaveObject.websiteNames;
            websiteLinks = actionsSaveObject.websiteLinks;
            websitePrompts = actionsSaveObject.websitePrompts;

            currentWebsiteValue = 0;

            websiteDropdown.ClearOptions();
            websiteDropdown.AddOptions(websiteNames);

            openWebsiteDropdown.ClearOptions();
            openWebsiteDropdown.AddOptions(websiteNames);

            websiteDropdown.value = 0;
            websiteNameInput.text = websiteNames[0];
            websiteLinkInput.text = websiteLinks[0];
            websitePromptInput.text = websitePrompts[0];

            if (websiteNames.Count > 1) {
                deleteWebsiteButton.interactable = true;
            }

            saveStatusText.text = "Previous actions loaded. (" + folder + "/ActionsSave.txt)";
        } catch {
            websiteNames = new List<string> { "Example Site" };
            websiteLinks = new List<string> { "https://example.com/" };
            websitePrompts = new List<string> { "I'm opening the site. *I open example.com.*" };

            saveStatusText.text = "Load failed due to bad JSON format, creating new save at " + folder + "/ActionsSave.txt";
            SaveActions();
        }
    }
    private void SaveActions() {
        folder = settingsScript.folder;

        websiteNames[currentWebsiteValue] = websiteNameInput.text;
        websiteLinks[currentWebsiteValue] = websiteLinkInput.text;
        websitePrompts[currentWebsiteValue] = websitePromptInput.text;

        ActionsSaveObject actionsSaveObject = new ActionsSaveObject();
        actionsSaveObject.body0 = actionInputField0.text;
        actionsSaveObject.body1 = actionInputField1.text;
        actionsSaveObject.body2 = actionInputField2.text;
        actionsSaveObject.body3 = actionInputField3.text;

        actionsSaveObject.websiteNames = websiteNames;
        actionsSaveObject.websiteLinks = websiteLinks;
        actionsSaveObject.websitePrompts = websitePrompts;

        string json = JsonUtility.ToJson(actionsSaveObject);

        File.WriteAllText(folder + "/ActionsSave.txt", json);
    }

    // Object for saving and parsing JSON file
    private class ActionsSaveObject {
        public string body0;
        public string body1;
        public string body2;
        public string body3;

        public List<string> websiteNames;
        public List<string> websiteLinks;
        public List<string> websitePrompts;
    }

    public void OnWebsiteDropdownChanged(int newValue) {

        websiteDropdown.options[currentWebsiteValue].text = websiteNameInput.text;

        websiteNames[currentWebsiteValue] = websiteNameInput.text;
        websiteLinks[currentWebsiteValue] = websiteLinkInput.text;
        websitePrompts[currentWebsiteValue] = websitePromptInput.text;

        websiteNameInput.text = websiteNames[newValue];
        websiteLinkInput.text = websiteLinks[newValue];
        websitePromptInput.text = websitePrompts[newValue];

        currentWebsiteValue = newValue;
    }

    public void OnClickAddWebsiteButton() {

        websiteNames[currentWebsiteValue] = websiteNameInput.text;
        websiteLinks[currentWebsiteValue] = websiteLinkInput.text;
        websitePrompts[currentWebsiteValue] = websitePromptInput.text;

        websiteNames.Add("New Website");
        websiteLinks.Add("");
        websitePrompts.Add("");

        websiteDropdown.ClearOptions();
        websiteDropdown.AddOptions(websiteNames);
        websiteDropdown.RefreshShownValue();
        websiteDropdown.value = websiteNames.Count - 1;

        websiteNameInput.text = websiteNames[websiteNames.Count - 1];
        websiteLinkInput.text = websiteLinks[websiteNames.Count - 1];
        websitePromptInput.text = websitePrompts[websiteNames.Count - 1];

        currentWebsiteValue = websiteNames.Count - 1;

        deleteWebsiteButton.interactable = true;
    }

    public void OnClickDeleteWebsiteButton() {

        websiteNames.RemoveAt(currentWebsiteValue);
        websiteLinks.RemoveAt(currentWebsiteValue);
        websitePrompts.RemoveAt(currentWebsiteValue);

        websiteDropdown.ClearOptions();
        websiteDropdown.AddOptions(websiteNames);
        websiteDropdown.RefreshShownValue();
        websiteDropdown.value = 0;

        websiteNameInput.text = websiteNames[0];
        websiteLinkInput.text = websiteLinks[0];
        websitePromptInput.text = websitePrompts[0];

        currentWebsiteValue = 0;

        if (websiteNames.Count <= 1) {
            deleteWebsiteButton.interactable = false;
        }
    }

    public void OnClickBody0() {
        LoadActions();
        logicScript.Send(actionInputField0.text);
        statusScript.SetBody(0);
    }

    public void OnClickBody1() {
        LoadActions();
        logicScript.Send(actionInputField1.text);
        statusScript.SetBody(1);
    }

    public void OnClickBody2() {
        LoadActions();
        logicScript.Send(actionInputField2.text);
        statusScript.SetBody(2);
    }

    public void OnClickBody3() {
        LoadActions();
        logicScript.Send(actionInputField3.text);
        statusScript.SetBody(3);
    }

    public void OnClickOpenWebsite() {
        Debug.Log("Open website at index: " + openWebsiteDropdown.value);

        Debug.Log("Open website at link: " + websiteLinks[openWebsiteDropdown.value]);
        string websiteLink = websiteLinks[openWebsiteDropdown.value];
        Application.OpenURL(websiteLink);

        Debug.Log("Open website with prompt: " + websitePrompts[openWebsiteDropdown.value]);
        logicScript.Send(websitePrompts[openWebsiteDropdown.value]);
    }
}

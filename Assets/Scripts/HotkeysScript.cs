using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class HotkeysScript : MonoBehaviour
{
    [SerializeField] LogicScript logicScript;
    [SerializeField] SettingsScript settingsScript;

    [SerializeField] Canvas hotkeysCanvas;

    [SerializeField] TMP_Text saveStatusText;

    // input fields for prompts 0-7
    [SerializeField] TMP_InputField hotkeyInputField0;
    [SerializeField] TMP_InputField hotkeyInputField1;
    [SerializeField] TMP_InputField hotkeyInputField2;
    [SerializeField] TMP_InputField hotkeyInputField3;
    [SerializeField] TMP_InputField hotkeyInputField4;
    [SerializeField] TMP_InputField hotkeyInputField5;
    [SerializeField] TMP_InputField hotkeyInputField6;
    [SerializeField] TMP_InputField hotkeyInputField7;

    private string folder = Application.dataPath + "/character";

    public void OnClickHotkeysButton()
    {
        LoadHotkeys();
        hotkeysCanvas.gameObject.SetActive(true);
    }

    public void OnClickHotkeysCloseButton()
    {
        SaveHotkeys();
        hotkeysCanvas.gameObject.SetActive(false);
    }

    public void LoadHotkeys()
    {
        folder = settingsScript.folder;

        if (!File.Exists(folder + "/HotkeysSave.txt"))
        {
            saveStatusText.text = "No previous hotkeys save, creating new save at " + folder + "/HotkeysSave.txt";
            SaveHotkeys();
            return;
        }

        string hotkeysSaveString = File.ReadAllText(folder + "/HotkeysSave.txt");
        try
        {
            HotkeysSaveObject hotkeysSaveObject = JsonUtility.FromJson<HotkeysSaveObject>(hotkeysSaveString);

            hotkeyInputField0.text = hotkeysSaveObject.hotkey0;
            hotkeyInputField1.text = hotkeysSaveObject.hotkey1;
            hotkeyInputField2.text = hotkeysSaveObject.hotkey2;
            hotkeyInputField3.text = hotkeysSaveObject.hotkey3;
            hotkeyInputField4.text = hotkeysSaveObject.hotkey4;
            hotkeyInputField5.text = hotkeysSaveObject.hotkey5;
            hotkeyInputField6.text = hotkeysSaveObject.hotkey6;
            hotkeyInputField7.text = hotkeysSaveObject.hotkey7;

            saveStatusText.text = "Previous actions loaded. (" + folder + "/HotkeysSave.txt)";
        }
        catch
        {
            saveStatusText.text = "Load failed due to bad JSON format, creating new save at " + folder + "/HotkeysSave.txt";
            SaveHotkeys();
        }
    }

    private void SaveHotkeys()
    {
        folder = settingsScript.folder;

        HotkeysSaveObject hotkeysSaveObject = new HotkeysSaveObject();
        hotkeysSaveObject.hotkey0 = hotkeyInputField0.text;
        hotkeysSaveObject.hotkey1 = hotkeyInputField1.text;
        hotkeysSaveObject.hotkey2 = hotkeyInputField2.text;
        hotkeysSaveObject.hotkey3 = hotkeyInputField3.text;
        hotkeysSaveObject.hotkey4 = hotkeyInputField4.text;
        hotkeysSaveObject.hotkey5 = hotkeyInputField5.text;
        hotkeysSaveObject.hotkey6 = hotkeyInputField6.text;
        hotkeysSaveObject.hotkey7 = hotkeyInputField7.text;

        string json = JsonUtility.ToJson(hotkeysSaveObject);

        File.WriteAllText(folder + "/HotkeysSave.txt", json);
    }

    // Object for saving and parsing JSON file
    private class HotkeysSaveObject
    {
        public string hotkey0;
        public string hotkey1;
        public string hotkey2;
        public string hotkey3;
        public string hotkey4;
        public string hotkey5;
        public string hotkey6;
        public string hotkey7;
    }

    public void OnClickHotkey0()
    {
        LoadHotkeys();
        if (hotkeyInputField0.text.Trim() == "")
        {
            return;
        }
        logicScript.Send(hotkeyInputField0.text);
    }

    public void OnClickHotkey1()
    {
        LoadHotkeys();
        if (hotkeyInputField1.text.Trim() == "")
        {
            return;
        }
        logicScript.Send(hotkeyInputField1.text);
    }

    public void OnClickHotkey2()
    {
        LoadHotkeys();
        if (hotkeyInputField2.text.Trim() == "")
        {
            return;
        }
        logicScript.Send(hotkeyInputField2.text);
    }

    public void OnClickHotkey3()
    {
        LoadHotkeys();
        if (hotkeyInputField3.text.Trim() == "")
        {
            return;
        }
        logicScript.Send(hotkeyInputField3.text);
    }

    public void OnClickHotkey4()
    {
        LoadHotkeys();
        if (hotkeyInputField4.text.Trim() == "")
        {
            return;
        }
        logicScript.Send(hotkeyInputField4.text);
    }

    public void OnClickHotkey5()
    {
        LoadHotkeys();
        if (hotkeyInputField5.text.Trim() == "")
        {
            return;
        }
        logicScript.Send(hotkeyInputField5.text);
    }

    public void OnClickHotkey6()
    {
        LoadHotkeys();
        if (hotkeyInputField6.text.Trim() == "")
        {
            return;
        }
        logicScript.Send(hotkeyInputField6.text);
    }

    public void OnClickHotkey7()
    {
        LoadHotkeys();
        if (hotkeyInputField7.text.Trim() == "")
        {
            return;
        }
        logicScript.Send(hotkeyInputField7.text);
    }
}

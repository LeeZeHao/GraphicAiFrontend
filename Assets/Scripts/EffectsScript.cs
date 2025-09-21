using System.Collections.Generic;
using System.IO;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class EffectsScript : MonoBehaviour
{
    [SerializeField] LogicScript logicScript;
    [SerializeField] SettingsScript settingsScript;
    [SerializeField] StatusScript statusScript;

    [SerializeField] Canvas effectsCanvas;

    [SerializeField] TMP_Text saveStatusText;

    // actions for turning on effect0-3
    [SerializeField] TMP_InputField effectInputField0;
    [SerializeField] TMP_InputField effectInputField1;
    [SerializeField] TMP_InputField effectInputField2;
    [SerializeField] TMP_InputField effectInputField3;
    // actions for turning on effect0-3a
    [SerializeField] TMP_InputField effectInputField0a;
    [SerializeField] TMP_InputField effectInputField1a;
    [SerializeField] TMP_InputField effectInputField2a;
    [SerializeField] TMP_InputField effectInputField3a;
    // actions for turning on effect0-3b
    [SerializeField] TMP_InputField effectInputField0b;
    [SerializeField] TMP_InputField effectInputField1b;
    [SerializeField] TMP_InputField effectInputField2b;
    [SerializeField] TMP_InputField effectInputField3b;

    private string folder = Application.dataPath + "/character";

    public void OnClickEffectsButton()
    {
        LoadEffects();
        effectsCanvas.gameObject.SetActive(true);
    }

    public void OnClickEffectsCloseButton()
    {
        SaveEffects();
        effectsCanvas.gameObject.SetActive(false);
    }

    public void LoadEffects()
    {
        folder = settingsScript.folder;

        if (!File.Exists(folder + "/EffectsSave.txt"))
        {
            saveStatusText.text = "No previous effects save, creating new save at " + folder + "/EffectsSave.txt";
            SaveEffects();
            return;
        }

        string effectsSaveString = File.ReadAllText(folder + "/EffectsSave.txt");
        try
        {
            EffectsSaveObject effectsSaveObject = JsonUtility.FromJson<EffectsSaveObject>(effectsSaveString);

            effectInputField0.text = effectsSaveObject.effect0;
            effectInputField1.text = effectsSaveObject.effect1;
            effectInputField2.text = effectsSaveObject.effect2;
            effectInputField3.text = effectsSaveObject.effect3;
            effectInputField0a.text = effectsSaveObject.effect0a;
            effectInputField1a.text = effectsSaveObject.effect1a;
            effectInputField2a.text = effectsSaveObject.effect2a;
            effectInputField3a.text = effectsSaveObject.effect3a;
            effectInputField0b.text = effectsSaveObject.effect0b;
            effectInputField1b.text = effectsSaveObject.effect1b;
            effectInputField2b.text = effectsSaveObject.effect2b;
            effectInputField3b.text = effectsSaveObject.effect3b;

            saveStatusText.text = "Previous actions loaded. (" + folder + "/EffectsSave.txt)";
        }
        catch
        {
            saveStatusText.text = "Load failed due to bad JSON format, creating new save at " + folder + "/EffectsSave.txt";
            SaveEffects();
        }
    }

    private void SaveEffects()
    {
        folder = settingsScript.folder;

        EffectsSaveObject effectsSaveObject = new EffectsSaveObject();
        effectsSaveObject.effect0 = effectInputField0.text;
        effectsSaveObject.effect1 = effectInputField1.text;
        effectsSaveObject.effect2 = effectInputField2.text;
        effectsSaveObject.effect3 = effectInputField3.text;
        effectsSaveObject.effect0a = effectInputField0a.text;
        effectsSaveObject.effect1a = effectInputField1a.text;
        effectsSaveObject.effect2a = effectInputField2a.text;
        effectsSaveObject.effect3a = effectInputField3a.text;
        effectsSaveObject.effect0b = effectInputField0b.text;
        effectsSaveObject.effect1b = effectInputField1b.text;
        effectsSaveObject.effect2b = effectInputField2b.text;
        effectsSaveObject.effect3b = effectInputField3b.text;

        string json = JsonUtility.ToJson(effectsSaveObject);

        File.WriteAllText(folder + "/EffectsSave.txt", json);
    }

    // Object for saving and parsing JSON file
    private class EffectsSaveObject
    {
        public string effect0;
        public string effect1;
        public string effect2;
        public string effect3;
        public string effect0a;
        public string effect1a;
        public string effect2a;
        public string effect3a;
        public string effect0b;
        public string effect1b;
        public string effect2b;
        public string effect3b;
    }

    public void OnClickEffect0(int newActive)
    {
        LoadEffects();
        switch (newActive)
        {
            case 0:
                logicScript.Send(effectInputField0.text);
                break;
            case 1:
                logicScript.Send(effectInputField0a.text);
                break;
            case 2:
                logicScript.Send(effectInputField0b.text);
                break;
            default:
                break;
        }
        statusScript.SetEffect0Active(newActive);
    }

    public void OnClickEffect1(int newActive)
    {
        LoadEffects();
        switch (newActive)
        {
            case 0:
                logicScript.Send(effectInputField1.text);
                break;
            case 1:
                logicScript.Send(effectInputField1a.text);
                break;
            case 2:
                logicScript.Send(effectInputField1b.text);
                break;
            default:
                break;
        }
        statusScript.SetEffect1Active(newActive);
    }

    public void OnClickEffect2(int newActive)
    {
        LoadEffects();
        switch (newActive)
        {
            case 0:
                logicScript.Send(effectInputField2.text);
                break;
            case 1:
                logicScript.Send(effectInputField2a.text);
                break;
            case 2:
                logicScript.Send(effectInputField2b.text);
                break;
            default:
                break;
        }
        statusScript.SetEffect2Active(newActive);
    }

    public void OnClickEffect3(int newActive)
    {
        LoadEffects();
        switch (newActive)
        {
            case 0:
                logicScript.Send(effectInputField3.text);
                break;
            case 1:
                logicScript.Send(effectInputField3a.text);
                break;
            case 2:
                logicScript.Send(effectInputField3b.text);
                break;
            default:
                break;
        }
        statusScript.SetEffect3Active(newActive);
    }
}

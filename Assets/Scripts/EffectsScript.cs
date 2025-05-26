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
    // actions for turning off effect0-3
    [SerializeField] TMP_InputField effectInputFieldOff0;
    [SerializeField] TMP_InputField effectInputFieldOff1;
    [SerializeField] TMP_InputField effectInputFieldOff2;
    [SerializeField] TMP_InputField effectInputFieldOff3;

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
            effectInputFieldOff0.text = effectsSaveObject.effectOff0;
            effectInputFieldOff1.text = effectsSaveObject.effectOff1;
            effectInputFieldOff2.text = effectsSaveObject.effectOff2;
            effectInputFieldOff3.text = effectsSaveObject.effectOff3;

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
        effectsSaveObject.effectOff0 = effectInputFieldOff0.text;
        effectsSaveObject.effectOff1 = effectInputFieldOff1.text;
        effectsSaveObject.effectOff2 = effectInputFieldOff2.text;
        effectsSaveObject.effectOff3 = effectInputFieldOff3.text;

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
        public string effectOff0;
        public string effectOff1;
        public string effectOff2;
        public string effectOff3;
    }

    public void OnClickEffect0()
    {
        LoadEffects();
        logicScript.Send(effectInputField0.text);
        statusScript.SetEffect0Active(true);
    }

    public void OnClickEffect1()
    {
        LoadEffects();
        logicScript.Send(effectInputField1.text);
        statusScript.SetEffect1Active(true);
    }

    public void OnClickEffect2()
    {
        LoadEffects();
        logicScript.Send(effectInputField2.text);
        statusScript.SetEffect2Active(true);
    }

    public void OnClickEffect3()
    {
        LoadEffects();
        logicScript.Send(effectInputField3.text);
        statusScript.SetEffect3Active(true);
    }

    public void OnClickEffectOff0()
    {
        LoadEffects();
        logicScript.Send(effectInputFieldOff0.text);
        statusScript.SetEffect0Active(false);
    }

    public void OnClickEffectOff1()
    {
        LoadEffects();
        logicScript.Send(effectInputFieldOff1.text);
        statusScript.SetEffect1Active(false);
    }

    public void OnClickEffectOff2()
    {
        LoadEffects();
        logicScript.Send(effectInputFieldOff2.text);
        statusScript.SetEffect2Active(false);
    }

    public void OnClickEffectOff3()
    {
        LoadEffects();
        logicScript.Send(effectInputFieldOff3.text);
        statusScript.SetEffect3Active(false);
    }
}

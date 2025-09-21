using System.IO;
using UnityEngine;

public class StatusScript : ObserverInterface
{

    [SerializeField] SettingsScript settingsScript;
    private string folder = Application.dataPath + "/character";

    private int mood = 0;
    private int body = 0;

    // For effects
    private int effect0Active = 0;
    private int effect1Active = 0;
    private int effect2Active = 0;
    private int effect3Active = 0;

    public int GetMood() {
        return mood;
    }

    public void SetMood(int mood) {
        this.mood = mood;
    }

    public int GetBody() {
        return body; 
    }

    public void SetBody(int body) {
        this.body = body;
        Debug.Log("body = " + body);
    }

    // For effects
    public int GetEffect0Active()
    {
        return effect0Active;
    }

    public void SetEffect0Active(int isActive)
    {
        this.effect0Active = isActive;
    }

    public int GetEffect1Active()
    {
        return effect1Active;
    }

    public void SetEffect1Active(int isActive)
    {
        this.effect1Active = isActive;
    }

    public int GetEffect2Active()
    {
        return effect2Active;
    }

    public void SetEffect2Active(int isActive)
    {
        this.effect2Active = isActive;
    }

    public int GetEffect3Active()
    {
        return effect3Active;
    }

    public void SetEffect3Active(int isActive)
    {
        this.effect3Active = isActive;
    }

    // Saves and loads the StatusSave.txt file when the mood is updated
    public override void UpdateObserver(string response, int mood = -1) {
        if (mood >= 0) {
            this.mood = mood;
        }
        SaveStatus();
    }

    public void LoadStatus() {

        if (!settingsScript.isFolderInit) {
            return;
        }

        folder = settingsScript.folder;

        if (!File.Exists(folder + "/StatusSave.txt")) {
            SaveStatus();
            return;
        }

        string statusSaveString = File.ReadAllText(folder + "/StatusSave.txt");
        try {
            StatusSaveObject statusSaveObject = JsonUtility.FromJson<StatusSaveObject>(statusSaveString);
            this.mood = statusSaveObject.mood;
            this.body = statusSaveObject.body;
            this.effect0Active = statusSaveObject.effect0Active;
            this.effect1Active = statusSaveObject.effect1Active;
            this.effect2Active = statusSaveObject.effect2Active;
            this.effect3Active = statusSaveObject.effect3Active;
        } catch {
            SaveStatus();
        }
    }

    private void SaveStatus() {
        // Debug.Log("Status saving...");
        folder = settingsScript.folder;

        if (!settingsScript.isFolderInit) {
            return;
        }

        StatusSaveObject statusSaveObject = new StatusSaveObject();
        statusSaveObject.mood = mood;
        statusSaveObject.body = body;
        statusSaveObject.effect0Active = effect0Active;
        statusSaveObject.effect1Active = effect1Active;
        statusSaveObject.effect2Active = effect2Active;
        statusSaveObject.effect3Active = effect3Active;
        string json = JsonUtility.ToJson(statusSaveObject);

        // Debug.Log(json);

        File.WriteAllText(folder + "/StatusSave.txt", json);
    }

    private class StatusSaveObject {
        public int mood;
        public int body;
        public int effect0Active;
        public int effect1Active;
        public int effect2Active;
        public int effect3Active;
    }

    public void ResetStatus() {
        mood = 0;
        body = 0;
        effect0Active = 0;
        effect1Active = 0;
        effect2Active = 0;
        effect3Active = 0;
        SaveStatus();
    }

    public override void ServerError(string error) {
        return;
    }

    public override void Waiting() {
        return;
    }
}

using System.IO;
using UnityEngine;

public class StatusScript : ObserverInterface
{

    [SerializeField] SettingsScript settingsScript;
    private string folder = Application.dataPath + "/character";

    private int mood = 0;
    private int body = 0;

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
        string json = JsonUtility.ToJson(statusSaveObject);

        // Debug.Log(json);

        File.WriteAllText(folder + "/StatusSave.txt", json);
    }

    private class StatusSaveObject {
        public int mood;
        public int body;
    }

    public void ResetStatus() {
        mood = 0;
        body = 0;
        SaveStatus();
    }

    public override void ServerError(string error) {
        return;
    }

    public override void Waiting() {
        return;
    }
}

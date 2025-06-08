using TMPro;
using UnityEngine;

public class DialogBoxScript : ObserverInterface
{

    [SerializeField] TMP_Text responseText;

    public override void UpdateObserver(string response, int mood) {
        string finalText = response;
        //finalText.Replace("\\", "");
        //finalText.Replace("\\\"", "\"");
        //finalText.Replace("\\n", "\n");
        //finalText.Replace("\\\n", "\n");
        //finalText.Replace("\\\\n", "\n");
        //finalText.Replace("\\*", "*");
        //finalText.Replace("*\\", "*");
        //finalText.Replace("\\\\*", "\\*");

        responseText.text = finalText;
        responseText.enabled = true;
    }

    public override void Waiting() {
        responseText.text = "Sending...";
        responseText.enabled = true;
    }

    public override void ServerError(string error) {
        responseText.text = "An error has occured, please check connection to server!\n" + error;
        responseText.enabled = true;
    }
}

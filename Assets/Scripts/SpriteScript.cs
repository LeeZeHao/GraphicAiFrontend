using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SpriteScript : ObserverInterface
{

    [SerializeField] private Image bodyImage;
    [SerializeField] private Image faceImage;
    // For effects
    [SerializeField] private Image effect0Image;
    [SerializeField] private Image effect1Image;
    [SerializeField] private Image effect2Image;
    [SerializeField] private Image effect3Image;
    [SerializeField] private ImagesScript imagesScript;

    [SerializeField] private AudioSource notificationAudioSource;
    [SerializeField] private AudioClip notificationAudio;
    [SerializeField] private TMP_Text toggleAudioButtonText;

    [SerializeField] private TMP_Text spriteDebugText;
    [SerializeField] private StatusScript statusScript;

    private int currentMood = 0;
    private int body = 0;
    // For effects
    private bool effect0Active = false;
    private bool effect1Active = false;
    private bool effect2Active = false;
    private bool effect3Active = false;

    private bool justBooted = true;

    [SerializeField] private float secondsBetweenBlink = 7.0f;
    [SerializeField] private float secondsBetweenBlinkRandomness = 0.5f;
    [SerializeField] private float secondsBlinkClosed = 0.2f;
    [SerializeField] private float secondsBetweenDoubleBlink = 0.2f;
    private bool eyesClosed = false;

    [SerializeField] private float speechSecondsPerChar = 0.005f;
    [SerializeField] private float speechSecondsBetweenSentence = 0.5f;
    private string response = "";
    private bool mouthOpen = false;

    private bool isAudioEnabled = true;

    // Start eye blinking after booting
    private void JustBootedSprite() {
        // Debug.Log("Sprite script JustBootedSprite");

        bodyImage.color = Color.white;
        faceImage.color = Color.white;
        // For effects
        effect0Image.color = Color.clear; // Default is clear so that we can see the character even if there is no effect image
        effect1Image.color = Color.clear;
        effect2Image.color = Color.clear;
        effect3Image.color = Color.clear;

        StartCoroutine(BlinkTimer());
    }

    public override void UpdateObserver(string response, int mood = -1) {

        if (justBooted && mood >= 0) {
            JustBootedSprite();
            justBooted = false;
        }

        // Update stored response string, status of mood, body
        this.response = response;
        if (mood >= 0) {
            this.currentMood = mood;
        }
        this.body = statusScript.GetBody();
        Debug.Log(this.body);

        UpdateSprite();

        if (isAudioEnabled) {
            notificationAudioSource.PlayOneShot(notificationAudio, 3f);
        }

        StopCoroutine(SpeechTimer());
        StartCoroutine(SpeechTimer());

        // For effects
        effect0Image.color = Color.clear;
        effect1Image.color = Color.clear;
        effect2Image.color = Color.clear;
        effect3Image.color = Color.clear;

        if (imagesScript.effect0)
        {
            effect0Active = statusScript.GetEffect0Active();
            effect0Image.sprite = imagesScript.effect0;
            effect0Image.color = Color.white;
            effect0Image.gameObject.SetActive(effect0Active);
        }
        if (imagesScript.effect1)
        {
            effect1Active = statusScript.GetEffect1Active();
            effect1Image.sprite = imagesScript.effect1;
            effect1Image.color = Color.white;
            effect1Image.gameObject.SetActive(effect1Active);
        }
        if (imagesScript.effect2)
        {
            effect2Active = statusScript.GetEffect2Active();
            effect2Image.sprite = imagesScript.effect2;
            effect2Image.color = Color.white;
            effect2Image.gameObject.SetActive(effect2Active);
        }
        if (imagesScript.effect3)
        {
            effect3Active = statusScript.GetEffect3Active();
            effect3Image.sprite = imagesScript.effect3;
            effect3Image.color = Color.white;
            effect3Image.gameObject.SetActive(effect3Active);
        }
    }

    private void UpdateSprite() {
        // Debug.Log("Sprite script UpdateSprite");

        string debugText = "";

        if (justBooted) {
            debugText += "justBooted";
        }

        debugText += "\ncurrentMood: " + this.currentMood;
        debugText += "\nbody: " + this.body;

        debugText += "\n\neyesClosed: " + this.eyesClosed;
        debugText += "\nmouthOpen:" + this.mouthOpen;

        // For effects
        debugText += "\n\neffect0: " + this.effect0Active;
        debugText += "\neffect1: " + this.effect1Active;
        debugText += "\neffect2: " + this.effect2Active;
        debugText += "\neffect3: " + this.effect3Active;

        spriteDebugText.text = debugText;

        // ** debug text complete **

        // face status: 0 = normal, 1 = mouthopen, 2 = eyesclosed
        int faceStatus = 0;
        if (this.mouthOpen) {
            faceStatus = 1;
        } else if (this.eyesClosed) {
            faceStatus = 2;
        } else {
            faceStatus = 0;
        }

        Sprite faceSprite;
        faceSprite = imagesScript.faces[this.currentMood][faceStatus];

        Sprite bodySprite;
        switch (this.body) {
            case 0:
                bodySprite = imagesScript.body0;
                break;
            case 1:
                bodySprite = imagesScript.body1;
                break;
            case 2:
                bodySprite = imagesScript.body2;
                break;
            case 3:
                bodySprite = imagesScript.body3;
                break;
            default:
                bodySprite = imagesScript.body0;
                break;
        }

        faceImage.sprite = faceSprite;
        bodyImage.sprite = bodySprite;

        /*
        // For effects
        effect0Image.color = Color.clear;
        effect1Image.color = Color.clear;
        effect2Image.color = Color.clear;
        effect3Image.color = Color.clear;

        // If just booted, don't display the effects
        if (justBooted)
        {
            return;
        }

        if (imagesScript.effect0)
        {
            effect0Active = statusScript.GetEffect0Active();
            effect0Image.sprite = imagesScript.effect0;
            effect0Image.color = Color.white;
            effect0Image.gameObject.SetActive(effect0Active);
        } 

        if (imagesScript.effect1)
        {
            effect1Active = statusScript.GetEffect1Active();
            effect1Image.sprite = imagesScript.effect1;
            effect1Image.color = Color.white;
            effect1Image.gameObject.SetActive(effect1Active);
        }

        if (imagesScript.effect2)
        {
            effect2Active = statusScript.GetEffect2Active();
            effect2Image.sprite = imagesScript.effect2;
            effect2Image.color = Color.white;
            effect2Image.gameObject.SetActive(effect2Active);
        }

        if (imagesScript.effect3)
        {
            effect3Active = statusScript.GetEffect3Active();
            effect3Image.sprite = imagesScript.effect3;
            effect3Image.color = Color.white;
            effect3Image.gameObject.SetActive(effect3Active);
        }
        */
    }

    private IEnumerator BlinkTimer() {
        eyesClosed = false;
        UpdateSprite();

        while (true) {
            // wait for seconds in range
            yield return new WaitForSeconds(Random.Range(
                    secondsBetweenBlink - secondsBetweenBlinkRandomness, 
                    secondsBetweenBlink + secondsBetweenBlinkRandomness
                    )
                );

            if ( justBooted == true ) {
                continue;
            }

            // perform single or double blink
            if (Random.Range(0.0f, 1.0f) >= 0.5) {
                // single blink
                eyesClosed = true;
                UpdateSprite();
                yield return new WaitForSeconds(secondsBlinkClosed);
                eyesClosed = false;
                UpdateSprite();
            } else {
                // double blink
                eyesClosed = true;
                UpdateSprite();
                yield return new WaitForSeconds(secondsBlinkClosed);
                eyesClosed = false;
                UpdateSprite();
                yield return new WaitForSeconds(secondsBetweenDoubleBlink);
                eyesClosed = true;
                UpdateSprite();
                yield return new WaitForSeconds(secondsBlinkClosed);
                eyesClosed = false;
                UpdateSprite();
            }
        }
    }

    private IEnumerator SpeechTimer() {
        mouthOpen = false;
        UpdateSprite();

        string stringToSplit = response;
        System.Text.RegularExpressions.Regex.Replace(stringToSplit, "(?<= *).* (?= *)", "");
        Debug.Log(stringToSplit);

        string[] sentences = stringToSplit.Split('.', '!', '?');

        foreach (string currentSentence in sentences) {
            string sentence = currentSentence;

            // filter out parts that are actions (has '*')
            if (sentence.Contains('*')) {
                int temp = sentence.Split('*').Length;
                sentence = sentence.Split('*')[temp - 1];
            }

            // Ignore empty sentences
            if (sentence.Length <= 0) {
                continue;
            }

            yield return new WaitForSeconds(speechSecondsBetweenSentence);
            mouthOpen = true;
            UpdateSprite();
            // Debug
            // Debug.Log("SpriteScript speaking Sentence: " + sentence);
            yield return new WaitForSeconds(sentence.Length * speechSecondsPerChar);
            mouthOpen = false;
            UpdateSprite();
        }

        mouthOpen = false;
        UpdateSprite();
    }

    public void OnClickToggleAudio() {
        isAudioEnabled = !isAudioEnabled;
        if (isAudioEnabled) {
            toggleAudioButtonText.text = "Audio: On";
        } else {
            toggleAudioButtonText.text = "Audio: Off";
        }
    }
    public override void ServerError(string error) {
        return;
    }

    public override void Waiting() {
        return;
    }
}

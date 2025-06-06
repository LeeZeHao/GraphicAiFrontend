using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ImagesScript : MonoBehaviour
{
    [SerializeField] private Image backgroundImage;

    private Texture2D backgroundTex;

    private Texture2D body0Tex;
    private Texture2D body1Tex;
    private Texture2D body2Tex;
    private Texture2D body3Tex;
    
    [HideInInspector] public List<Texture2D[]> faceTextures = new List<Texture2D[]>();

    // For effects
    private Texture2D effect0Tex;
    private Texture2D effect1Tex;
    private Texture2D effect2Tex;
    private Texture2D effect3Tex;

    [HideInInspector] public Sprite body0;
    [HideInInspector] public Sprite body1;
    [HideInInspector] public Sprite body2;
    [HideInInspector] public Sprite body3;

    [HideInInspector] public List<Sprite[]> faces = new List<Sprite[]>();

    // For effects
    [HideInInspector] public Sprite effect0;
    [HideInInspector] public Sprite effect1;
    [HideInInspector] public Sprite effect2;
    [HideInInspector] public Sprite effect3;

    private void Awake() {

    }

    public bool InitImages(string folder, List<string> emotions) {

        try {
            backgroundTex = GetTexFromFile(folder + "/background.png");

            body0Tex = GetTexFromFile(folder + "/body0.png");
            body1Tex = GetTexFromFile(folder + "/body1.png");
            body2Tex = GetTexFromFile(folder + "/body2.png");
            body3Tex = GetTexFromFile(folder + "/body3.png");

            faceTextures.Clear();
            for (int i = 0; i < emotions.Count; i++) {
                string emotion = emotions[i];
                Texture2D[] newTextures = new Texture2D[3];
                newTextures[0] = GetTexFromFile(folder + "/" + emotion + ".png");
                newTextures[1] = GetTexFromFile(folder + "/" + emotion + "_mouthopen.png");
                newTextures[2] = GetTexFromFile(folder + "/" + emotion + "_eyesclosed.png");
                faceTextures.Add(newTextures);
            }

            // convert to sprite

            body0 = GetSpriteFromTex(body0Tex);
            body1 = GetSpriteFromTex(body1Tex);
            body2 = GetSpriteFromTex(body2Tex);
            body3 = GetSpriteFromTex(body3Tex);

            faces.Clear();
            for (int i = 0; i < faceTextures.Count; i++) {
                Sprite[] newSprites = new Sprite[3];
                newSprites[0] = GetSpriteFromTex(faceTextures[i][0]);
                newSprites[1] = GetSpriteFromTex(faceTextures[i][1]);
                newSprites[2] = GetSpriteFromTex(faceTextures[i][2]);
                faces.Add(newSprites);
            }

            backgroundImage.sprite = GetSpriteFromTex(backgroundTex);
            backgroundImage.color = Color.white;
        } catch {
            return false;
        }

        // Effect images are optional, use a bunch of try catches
        try
        {
            effect0Tex = GetTexFromFile(folder + "/effect0.png");
            // convert to sprite
            effect0 = GetSpriteFromTex(effect0Tex);
        }
        catch
        {
        }

        try
        {
            effect1Tex = GetTexFromFile(folder + "/effect1.png");
            // convert to sprite
            effect1 = GetSpriteFromTex(effect1Tex);
        }
        catch
        {
        }

        try
        {
            effect2Tex = GetTexFromFile(folder + "/effect2.png");
            // convert to sprite
            effect2 = GetSpriteFromTex(effect2Tex);
        }
        catch
        {
        }

        try
        {
            effect3Tex = GetTexFromFile(folder + "/effect3.png");
            // convert to sprite
            effect3 = GetSpriteFromTex(effect3Tex);
        }
        catch
        {
        }

        return true;
    }

    private Texture2D GetTexFromFile(string path) {
        byte[] bytes = File.ReadAllBytes(path);
        Texture2D loadTexture = new Texture2D(1, 1); //mock size 1x1
        loadTexture.LoadImage(bytes);
        return loadTexture;
    }

    public Sprite GetSpriteFromTex(Texture2D tex) {
        return Sprite.Create(tex, new Rect(0.0f, 0.0f, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100.0f);
    }
}

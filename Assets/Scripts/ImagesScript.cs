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
    private Texture2D effect0aTex;
    private Texture2D effect1aTex;
    private Texture2D effect2aTex;
    private Texture2D effect3aTex;
    private Texture2D effect0bTex;
    private Texture2D effect1bTex;
    private Texture2D effect2bTex;
    private Texture2D effect3bTex;

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
    [HideInInspector] public Sprite effect0a;
    [HideInInspector] public Sprite effect1a;
    [HideInInspector] public Sprite effect2a;
    [HideInInspector] public Sprite effect3a;
    [HideInInspector] public Sprite effect0b;
    [HideInInspector] public Sprite effect1b;
    [HideInInspector] public Sprite effect2b;
    [HideInInspector] public Sprite effect3b;

    void Awake()
    {
        // Make a transparent sprite as a stand in for the effects by default
        Texture2D tex = new Texture2D(1, 1);
        tex.SetPixel(0, 0, Color.clear);
        tex.Apply();

        Sprite transparentSprite = Sprite.Create(tex, new Rect(0, 0, 1, 1), Vector2.one * 0.5f);

        effect0 = effect1 = effect2 = effect3 =
        effect0a = effect1a = effect2a = effect3a =
        effect0b = effect1b = effect2b = effect3b =
        transparentSprite;
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

        // Effects "a" and "b" variations, optional as well

        try
        {
            effect0aTex = GetTexFromFile(folder + "/effect0a.png");
            // convert to sprite
            effect0a = GetSpriteFromTex(effect0aTex);
        }
        catch
        {
        }

        try
        {
            effect1aTex = GetTexFromFile(folder + "/effect1a.png");
            // convert to sprite
            effect1a = GetSpriteFromTex(effect1aTex);
        }
        catch
        {
        }

        try
        {
            effect2aTex = GetTexFromFile(folder + "/effect2a.png");
            // convert to sprite
            effect2a = GetSpriteFromTex(effect2aTex);
        }
        catch
        {
        }

        try
        {
            effect3aTex = GetTexFromFile(folder + "/effect3a.png");
            // convert to sprite
            effect3a = GetSpriteFromTex(effect3aTex);
        }
        catch
        {
        }

        try
        {
            effect0bTex = GetTexFromFile(folder + "/effect0b.png");
            // convert to sprite
            effect0b = GetSpriteFromTex(effect0bTex);
        }
        catch
        {
        }

        try
        {
            effect1bTex = GetTexFromFile(folder + "/effect1b.png");
            // convert to sprite
            effect1b = GetSpriteFromTex(effect1bTex);
        }
        catch
        {
        }

        try
        {
            effect2bTex = GetTexFromFile(folder + "/effect2b.png");
            // convert to sprite
            effect2b = GetSpriteFromTex(effect2bTex);
        }
        catch
        {
        }

        try
        {
            effect3bTex = GetTexFromFile(folder + "/effect3b.png");
            // convert to sprite
            effect3b = GetSpriteFromTex(effect3bTex);
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

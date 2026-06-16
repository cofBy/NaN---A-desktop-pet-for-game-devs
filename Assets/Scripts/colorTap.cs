using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class colorTap : MonoBehaviour
{
    [Header("refrences")]
    public int size;
    public Slider Hue, Sat, Val;

    float oldH, oldS, oldV;
    string oldHex;

    RawImage hueImage, satImage, valImage;
    Texture2D hueTex, satTex, valTex;

    [Header("saving system")]
    public TMP_InputField hexValue;
    public Image endResult;
    public string whatColor;
    bool usingSliders;

    private void Start()
    {
        displayHue();
        displaySat();
        displayVal();

        hexValue.text = PlayerPrefs.GetString(whatColor + "Color");
    }
    private void Update()
    {
        if (oldH != Hue.value || oldS != Sat.value || oldV != Val.value)
        {
            displaySat();
            displayVal();
            usingSliders = true;
        }

        if (oldHex != hexValue.text)
        {
            usingSliders = false;
        }

        bool usingHex = (UnityEngine.ColorUtility.TryParseHtmlString("#" + hexValue.text, out Color colorFromHex));
        if (usingSliders) 
        {
            endResult.color = Color.HSVToRGB(Hue.value, Sat.value, Val.value);
        }
        else
        {
            if (usingHex)
            {
                endResult.color = colorFromHex;
            }
            else
            {
                usingSliders = true;
            }
        }

        oldH = Hue.value;
        oldS = Sat.value;
        oldV = Val.value;
        oldHex = hexValue.text;
    }
    
    public void saveHex()
    {
        PlayerPrefs.SetString(whatColor + "Color", endResult.color.ToHexString());
    }

    void displayHue()
    {
        hueImage = Hue.transform.GetChild(0).GetComponent<RawImage>();
        hueTex = new Texture2D(size, 1);
        hueTex.wrapMode = TextureWrapMode.Clamp;

        for (int i = 0; i < hueTex.width; i++)
        {
            hueTex.SetPixel(i, 0, Color.HSVToRGB((float)i / hueTex.width, 1, 1f));
        }
        hueTex.Apply();

        hueImage.texture = hueTex;
    }
    
    void displaySat()
    {
        satImage = Sat.transform.GetChild(0).GetComponent<RawImage>();
        satTex = new Texture2D(size, 1);
        satTex.wrapMode = TextureWrapMode.Clamp;

        for (int i = 0; i < satTex.width; i++)
        {
            satTex.SetPixel(i, 0, Color.HSVToRGB(Hue.value, (float)i / satTex.width, 1));
        }
        satTex.Apply();

        satImage.texture = satTex;
    }
    
    void displayVal()
    {
        valImage = Val.transform.GetChild(0).GetComponent<RawImage>();
        valTex = new Texture2D(size, 1);
        valTex.wrapMode = TextureWrapMode.Clamp;

        for (int i = 0; i < valTex.width; i++)
        {
            valTex.SetPixel(i, 0, Color.HSVToRGB(Hue.value, Sat.value, (float)i / valTex.width));
        }
        valTex.Apply();

        valImage.texture = valTex;
    }

}

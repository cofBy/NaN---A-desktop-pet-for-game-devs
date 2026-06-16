using UnityEngine;

public class NaNColorHandler : MonoBehaviour
{
    [Header("stuff to color")]
    public Material outlineMat;
    public Material NaNColors;
    public Color white;
    private void Start()
    {
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("outlineColor"), out Color outlineColor);
        ColorUtility.TryParseHtmlString("#" + PlayerPrefs.GetString("NaNColor"), out Color NaNColor);

        if (outlineColor == Color.white)
        {
            outlineMat.SetColor("_outline_Color", white);
        }
        else
        {
            outlineMat.SetColor("_outline_Color", outlineColor);
        }
        NaNColors.SetColor("_wantedColor", NaNColor);
    }
}

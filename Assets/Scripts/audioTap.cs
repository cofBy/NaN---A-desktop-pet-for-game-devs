using UnityEngine;
using UnityEngine.UI;

public class audioTap : MonoBehaviour
{
    [Header("getting the volume")]
    public Slider volSlider;

    private void Start()
    {
        if (PlayerPrefs.HasKey("volume"))
        {
            volSlider.value = PlayerPrefs.GetFloat("volume");
        }
    }
    public void Save()
    {
        PlayerPrefs.SetFloat("volume", volSlider.value);
        PlayerPrefs.Save();
    }
}

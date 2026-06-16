using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class behaviorTap : MonoBehaviour
{
    [Header("getting the behaviour")]
    public TMP_Dropdown dropDown;
    public Slider talkingFreq;
    public Slider emotingFreq;

    private void Start()
    {
        dropDown.value = PlayerPrefs.GetInt("behaviour");
        talkingFreq.value = PlayerPrefs.GetFloat("talking freq");
        emotingFreq.value = PlayerPrefs.GetFloat("emoting freq");
    }
    public void Save()
    {
        PlayerPrefs.SetInt("behaviour", dropDown.value);
        PlayerPrefs.SetFloat("talking freq", talkingFreq.value);
        PlayerPrefs.SetFloat("emoting freq", emotingFreq.value);
    }
}

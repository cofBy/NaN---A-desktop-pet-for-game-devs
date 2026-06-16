using TMPro;
using UnityEngine;

public class timerApp : MonoBehaviour
{
    [Header("Timer")]
    public float time;
    bool typing;
    bool started;

    [Header("displaying time")]
    public GameObject TimeHUD;
    public TMP_InputField minutes;
    public TMP_InputField seconds;
    public TextMeshProUGUI startButton;


    app me;

    private void Awake()
    {
        me = GetComponent<app>();
    }

    public void tick()
    {
        if (!started)
        {
            typing = false;

            float m = 0;
            float s = 0;

            float.TryParse(minutes.text, out m);
            float.TryParse(seconds.text, out s);

            time = m * 60 + s;
        }

        started = !started;
    }

    private void Update()
    {
        TimeHUD.SetActive(me.isOpen);

        startButton.text = started ? "stop" : "start";

        if (started)
        {
            time -= Time.deltaTime;

            if (time <= 0)
            {
                me.sayWhenUse(false);
                FEEL.PlaySound("timerEnd");
                time = 0;
                started = false;
            }
        }

        int totalSeconds = Mathf.FloorToInt(time);
        int minutesValue = totalSeconds / 60;
        int secondsValue = totalSeconds % 60;

        if (typing == false)
        {
            minutes.text = minutesValue.ToString("00");
            seconds.text = secondsValue.ToString("00");
        }
    }
    public void Type()
    {
        typing = true;
    }
}
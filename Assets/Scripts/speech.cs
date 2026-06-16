using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class speech : MonoBehaviour
{
    [Header("speeches")]
    public string[] randomSpeeches;
    bool talking;

    public float MinTime, MaxTime;
    public float framesBetweenChars;

    float rnd;
    float timer;

    [Header("placing the bubble")]
    public Transform Head;
    public Vector3 offset;

    [Header("sizing the bubble")]
    public Image speechBubble;
    public TextMeshProUGUI speechUI;
    public BoxCollider2D col;

    public float charWidth;
    public float extraWidth;
    public float height;

    [Header("closing after mouse touch")]
    public float toliranceSize;
    public float framesHovering;
    int hover;

    [Header("talking frequency")]
    float freq;

    private void Start()
    {
        freq = PlayerPrefs.GetFloat("talking freq");

        rnd = Random.Range(MinTime, MaxTime);

        speechBubble.gameObject.SetActive(false);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= rnd)
        {
            rnd = Random.Range(MinTime, MaxTime);
            timer = 0;

            float rndC = Random.Range(0f, 1f);
            if (talking == false && rndC < freq)
            {
                say(randomSpeeches[Random.Range(0, randomSpeeches.Length)]);
            }
        }

        float size = Head.transform.position.x > 0 ? 1 : -1;
        speechBubble.transform.localScale = new Vector3(size, 1, 1);
        speechUI.transform.localScale = new Vector3(size, 1, 1);

        speechBubble.rectTransform.pivot = offset;
        speechBubble.rectTransform.position = Camera.main.WorldToScreenPoint(Head.transform.position) + speechBubble.transform.right;

        col.size = speechBubble.rectTransform.sizeDelta - (Vector2.one * toliranceSize);
        col.offset = new Vector2(offset.x * -0.5f * speechBubble.rectTransform.sizeDelta.x, 60);

        bool hovering = false;
        foreach (Collider2D overlap in Physics2D.OverlapPointAll(Input.mousePosition))
        {
            if (overlap == col)
            {
                hovering = true;
                break;
            }
        }

        hover = hovering == true ? hover + 1 : 0;
        if (hover >= framesHovering)
        {
            StopTalking();
        }
    }
    public void say(string saying, bool important = false)
    {
        StopAllCoroutines();
        StartCoroutine(talk(saying, important));
    }
    IEnumerator talk(string sp, bool important)
    {
        talking = true;
        speechBubble.gameObject.SetActive(true);

        for (int i = 0; i <= sp.Length; i++)
        {
            speechUI.text = sp.Substring(0, i);

            float width = Mathf.Clamp(speechUI.preferredWidth + extraWidth, height, float.MaxValue);
            speechBubble.rectTransform.sizeDelta = new Vector2(width, height);
            for (int j = 0; j < framesBetweenChars; j++)
            {
                yield return null;
            }
        }
        talking = important;
    }
    public void StopTalking()
    {
        talking = false;
        speechBubble.gameObject.SetActive(false);
    }
}
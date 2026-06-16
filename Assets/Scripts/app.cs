using System;
using UnityEngine;

public class app : MonoBehaviour
{
    [Header("sprites")]
    public Sprite unused;
    public Sprite used;

    public SpriteRenderer sr;

    [Header("OpenState")]
    public bool isOpen;
    float downTime;
    public float window;
    public bool held;
    public bool keepOnScreen;

    [Header("talking")]
    public speech mouth;
    public string[] endingSayings;
    public string[] startingSayings;
    public bool sayAuto;

    [Header("ignoring bounds")]
    public Collider2D col;
    public LayerMask boundsMask;

    private void Start()
    {
        held = true;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            downTime = Time.time;
        }

        if (Input.GetMouseButtonUp(0))
        {
            if (Time.time - downTime < window)
            {
                RaycastHit2D hit = Physics2D.Raycast(mousePos(), Vector2.zero);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    isOpen = !isOpen;
                    FEEL.PlaySound("open");

                    if (sayAuto == true)
                    {
                        sayWhenUse(isOpen);
                    }
                }
            }
        }
        sr.sprite = isOpen ? used : unused;
        col.excludeLayers = held ? boundsMask : 0;
    }
    public void sayWhenUse(bool atStart, string addedValue = "")
    {
        string saying = atStart ? startingSayings[UnityEngine.Random.Range(0, startingSayings.Length)] : endingSayings[UnityEngine.Random.Range(0, endingSayings.Length)];
        mouth.say(saying + addedValue, true);
    }
    Vector3 mousePos()
    {
        Vector3 p = Input.mousePosition;
        p.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(p);
    }
}

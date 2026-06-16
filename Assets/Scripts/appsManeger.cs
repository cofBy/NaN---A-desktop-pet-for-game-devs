using UnityEngine;

public class appsManeger : MonoBehaviour
{
    [Header("apps")]
    public app[] apps;

    bool[] oldOpen;

    [Header("placing apps")]
    public float minDistance;
    public Transform[] restingPos;
    bool[] held;

    Rigidbody2D[] rbs;


    [Header("Keeping focus")]
    public TransparentWindow TW;

    private void Awake()
    {
        oldOpen = new bool[apps.Length];
        held = new bool[restingPos.Length];

        rbs = new Rigidbody2D[apps.Length];

        for (int i = 0; i < apps.Length; i++)
        {
            rbs[i] = apps[i].GetComponent<Rigidbody2D>();
        }
    }

    private void Update()
    {
        for (int j = 0; j < held.Length; j++)
        {
            held[j] = false;
        }

        int keepAmount = 0;

        for (int i = 0; i < apps.Length; i++)
        {
            if (apps[i].isOpen && !oldOpen[i])
            {
                for (int j = 0; j < apps.Length; j++)
                {
                    if (j != i)
                    {
                        apps[j].isOpen = false;
                    }
                }
            }

            if (apps[i].keepOnScreen)
            {
                keepAmount += apps[i].isOpen ? 1 : 0;
            }
            TW.KeepInScreen = keepAmount > 0;

            oldOpen[i] = apps[i].isOpen;
        }

        for (int i = 0; i < apps.Length; i++)
        {
            bool snapped = false;

            for (int j = 0; j < restingPos.Length; j++)
            {
                if (held[j]) continue;

                if (Vector2.Distance(apps[i].transform.position, restingPos[j].position) < minDistance)
                {
                    rbs[i].position = restingPos[j].position;
                    rbs[i].gravityScale = 0;
                    rbs[i].linearVelocity = Vector2.zero;

                    held[j] = true;
                    snapped = true;

                    if (!apps[i].held)
                    {
                        FEEL.PlaySound("snap");
                        apps[i].held = true;
                    }

                    break;
                }
            }

            if (!snapped)
            {
                rbs[i].gravityScale = 1;
                apps[i].held = false;
            }
        }
    }
}
using System.Collections;
using UnityEngine;

public class NanMovment : MonoBehaviour
{
    [Header("refrences")]
    public Rigidbody2D rb;

    [Header("movement")]
    public float speed;
    public float timeMoving;
    public float timeBetweenMoves;
    public float timeRange;

    float rndM;
    bool moved;

    public float angleRange;

    float TimeToMove;
    float timer;

    [Header("drawing the body")]
    public float headHight;
    public LineRenderer Body;
    public Transform Hips;
    public Transform Neck;
    public TargetJoint2D Head;
    public float neckRadius;

    [Header("is grounded")]
    public float RayLength;
    public LayerMask groundMask;

    [Header("emotes")]
    public Animator anim;

    public float minEtime;
    public float maxEtime;

    float eTimer;
    float timeToEmote;

    public float[] weights;
    public float[] lengths;

    float freq;
    private void Start()
    {
        freq = PlayerPrefs.GetFloat("emoting freq");

        Application.targetFrameRate = 60;

        TimeToMove = Random.Range(timeBetweenMoves - timeRange, timeBetweenMoves + timeRange);
        timeToEmote = Random.Range(minEtime, maxEtime);
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= TimeToMove && grounded())
        {
            timer = 0;
            TimeToMove = Random.Range(1.5f, 2.5f);

            if (moved == false)
            {
                StartCoroutine(Move());
            }
        }

        eTimer += Time.deltaTime;
        if (eTimer >= timeToEmote && grounded())
        {
            eTimer = 0;

            timeToEmote = Random.Range(minEtime, maxEtime);

            float rndE = Random.Range(0f, 1f);
            float sum = 0;
            for (int i = 0; i < weights.Length; i++)
            {
                sum += weights[i];
                if (rndE < sum * freq)
                {
                    StartCoroutine(emote(i));
                    break;
                }
            }
        }

        Head.target = Hips.transform.position + new Vector3(0, headHight, 0);
        Neck.transform.position = Head.enabled ? Head.transform.position + new Vector3(0, -0.3f, 0) : Hips.transform.position + new Vector3(0, headHight, 0);

        if (Vector2.Distance(Head.transform.position, Neck.transform.position) < neckRadius && Head.enabled == false)
        {
            Head.enabled = true;
        }

        Body.SetPosition(0, Neck.transform.position);
        Body.SetPosition(1, Hips.position);

        if (Mathf.Abs(rb.linearVelocityX) > angleRange)
        {
            transform.eulerAngles = new Vector3(0, (Mathf.Sign(rb.linearVelocityX) - 1) * 90f, 0);
        }
    }
    bool grounded()
    {
        return Physics2D.Raycast(transform.position, Vector2.down, RayLength, groundMask);
    }
    IEnumerator emote(int index)
    {

        rb.linearVelocityX = 0;
        moved = true;

        anim.SetBool("playing", true);
        anim.SetInteger("index", index);

        yield return new WaitForSeconds(lengths[index]);

        anim.SetBool("playing", false);

        moved = false;
    }
    IEnumerator Move()
    {
        moved = true;

        float moveTime = Random.Range(timeMoving - timeRange, timeMoving + timeRange);
        float dir = Random.Range(0, 2);

        rb.linearVelocity = new Vector2(Mathf.Cos(dir * Mathf.PI) * speed, rb.linearVelocityY);

        yield return new WaitForSeconds(moveTime);

        moved = false;

        rb.linearVelocityX = 0;
    }
}

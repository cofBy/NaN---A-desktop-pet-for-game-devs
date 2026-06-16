using System.Net;
using UnityEngine;

public class dragAndDrop : MonoBehaviour
{
    [Header("checking for touching")]
    bool hover;
    bool draging;

    [Header("draging")]
    public bool usePhysics = true;
    public Rigidbody2D rb;
    public float strength;
    Vector2 offset;

    [Header("get away from mouse")]
    public float mouseRadius;
    public float mouseStrength;
    Collider2D col;

    public bool isNaN = false;
    private void Start()
    {
        col = GetComponent<Collider2D>();

        if (isNaN == true)
        {
            if (PlayerPrefs.GetInt("behaviour") == 0)
            {
                mouseRadius = 0;
            }
            else
            {
                strength = 0;
            }
        }
    }
    private void Update()
    {
        if (hover && Input.GetKeyDown(KeyCode.Mouse0))
        {
            offset = transform.position - mousePos();
            draging = true;
        }

        if (usePhysics)
        {
            if (strength != 0)
            {
                handlePhysics();
            }

            if (mouseRadius != 0)
            {
                foreach (Collider2D collider in Physics2D.OverlapCircleAll(mousePos(), mouseRadius))
                {
                    if (collider == col)
                    {
                        Vector3 dir = (col.bounds.center - mousePos()).normalized;
                        rb.linearVelocity = dir * mouseStrength;
                    }
                }
            }
        }
        else
        {
            handleTransforms();

            if (Vector2.Distance(transform.position, mousePos()) < mouseRadius)
            {
                Vector3 dir = transform.position - mousePos();
                transform.position += dir;
            }
        }
    }
    void handlePhysics()
    {
        if (draging == true && Input.GetKey(KeyCode.Mouse0))
        {
            Vector2 dir = (mousePos() + (Vector3)offset) - transform.position;
            rb.linearVelocity = dir * strength;

            rb.gravityScale = 0f;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            rb.gravityScale = 1f;
            draging = false;
        }
    }

    void handleTransforms()
    {

        if (draging == true && Input.GetKey(KeyCode.Mouse0))
        {
            transform.position = mousePos() + (Vector3)offset;
        }
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            draging = false;
        }

    }

    Vector3 mousePos()
    {
        Vector3 p = Input.mousePosition;
        p.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(p);
    }
    private void OnMouseEnter()
    {
        hover = true;
    }
    private void OnMouseExit()
    {
        hover = false;
        if (usePhysics)
        {
            rb.gravityScale = 1f;
        }
    }
}

using TMPro;
using UnityEngine;

public class rulerApp : MonoBehaviour
{
    [Header("displaying the ruler")]
    public LineRenderer line;
    bool isUsing;

    [Header("displaying length")]
    public TextMeshPro meter;
    public float offset;
    float angle;

    [Header("calculating distance")]
    public float distanceInPixels;
    Vector3 startingPoint;

    app me;

    private void Awake()
    {
        me = GetComponent<app>();
        isUsing = false;
    }
    private void Update()
    {
        meter.gameObject.SetActive(me.isOpen);

        if (me.isOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                startingPoint = mousePos();
                line.SetPosition(0, startingPoint);
                isUsing = true;
            }
            if (Input.GetMouseButton(0))
            {
                line.SetPosition(1, mousePos());
                distanceInPixels = Vector2.Distance(Camera.main.WorldToScreenPoint(startingPoint), Camera.main.WorldToScreenPoint(mousePos()));

                angle = Mathf.Atan2(mousePos().y - startingPoint.y, mousePos().x - startingPoint.x) * Mathf.Rad2Deg;
                meter.transform.position = Vector3.Lerp(startingPoint, mousePos(), 0.5f) + new Vector3(-Mathf.Sin(angle * Mathf.Deg2Rad) * offset, Mathf.Cos(angle * Mathf.Deg2Rad) * offset, 0);
                meter.text = Mathf.Round(distanceInPixels).ToString();
            }
            if (Input.GetMouseButtonUp(0) && isUsing == true)
            {
                isUsing = false;
                me.sayWhenUse(false);
            }
        }
        else
        {
            line.SetPosition(0, Vector2.zero);
            line.SetPosition(1, Vector2.zero);
        }
    }
    Vector3 mousePos()
    {
        Vector3 p = Input.mousePosition;
        p.z = -Camera.main.transform.position.z;
        p = new Vector3(Mathf.Round(p.x), Mathf.Round(p.y), Mathf.Round(p.z));
        return Camera.main.ScreenToWorldPoint(p);
    }
}

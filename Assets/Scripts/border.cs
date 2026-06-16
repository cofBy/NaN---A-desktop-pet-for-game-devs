using UnityEngine;

public class Border : MonoBehaviour
{
    [Header("positioning")]
    public float rot;

    [Header("scaling")]
    public float thickness;
    public float length;

    [Header("sizing")]
    public float resizesPerSec;
    float timer;

    private void Start()
    {
        resize();
    }
    private void Update()
    {
        timer += Time.deltaTime;
        if (timer > resizesPerSec)
        {
            timer = 0;
            resize();
        }
    }
    void resize()
    {
        float rad = rot * Mathf.Deg2Rad;

        Vector2 dir = new Vector2(Mathf.Sin(rad), Mathf.Cos(rad));

        float width = Camera.main.orthographicSize * Camera.main.aspect;
        float height = Camera.main.orthographicSize;

        DisplayInfo display = Screen.mainWindowDisplayInfo;
        RectInt wa = display.workArea;

        int taskbarVertical = display.height - wa.height;
        int taskbarHorizontal = display.width - wa.width;

        float meterPerPixel = 2 * height / display.height;

        transform.position = new Vector2((width * dir.x) + (dir.x * thickness * 0.5f) + (taskbarHorizontal * meterPerPixel), (height * dir.y) + (dir.y * thickness * 0.5f) + (taskbarVertical * meterPerPixel));
        transform.up = dir;
        transform.localScale = new Vector3(length, thickness, 1);
    }
}

using System;
using System.Collections;
using System.Runtime.InteropServices;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class colorDropperApp : MonoBehaviour
{
    [DllImport("user32.dll")]
    static extern IntPtr GetDC(IntPtr hWnd);

    [DllImport("gdi32.dll")]
    static extern uint GetPixel(IntPtr hdc, int x, int y);

    [DllImport("user32.dll")]
    static extern int ReleaseDC(IntPtr hWnd, IntPtr hDC);

    [Header("taking a color")]
    public GameObject infoParent;
    public Image colorChoosed;
    public TextMeshProUGUI HexValue;
    public Color output;

    app me;
    private void Awake()
    {
        me = GetComponent<app>();
    }
    private void Update()
    {
        infoParent.SetActive(me.isOpen);
        if (me.isOpen == true)
        {
            IntPtr dc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(dc, (int)Input.mousePosition.x, Screen.currentResolution.height - (int)Input.mousePosition.y);
            ReleaseDC(IntPtr.Zero, dc);

            float r = (pixel & 0x000000FF) / 255f;
            float g = ((pixel & 0x0000FF00) >> 8) / 255f;
            float b = ((pixel & 0x00FF0000) >> 16) / 255f;

            output = new Color(r, g, b);

            colorChoosed.color = output;
            infoParent.transform.position = Input.mousePosition;
            HexValue.text = output.ToHexString();

            if (Input.GetMouseButtonDown(0))
            {
                GUIUtility.systemCopyBuffer = output.ToHexString();
                me.isOpen = false;

                me.sayWhenUse(false, output.ToHexString());
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                me.isOpen = false;
            }
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

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DrawMesh : MonoBehaviour
{
    [Header("displaying a mesh")]
    List<Mesh> drawings = new List<Mesh>();
    Vector3 lastMousePos;

    public GameObject newFilter;
    public List<MeshFilter> activeFilters = new List<MeshFilter>();
    public List<MeshFilter> deadFilters = new List<MeshFilter>();

    bool isdrawing;

    [Header("values")]
    public float thickness;
    public float minDistance;
    app me;

    [Header("layering")]
    public float zMultiplier;
    bool madeMesh;

    [Header("customization")]
    public Slider customThickness;
    public Toggle[] Colors;
    public Material[] materials;
    bool[] oldOpen;
    public GameObject parent;
    int matIndex;

    private void Awake()
    {
        me = GetComponent<app>();
        oldOpen = new bool[Colors.Length];
    }
    private void Update()
    {
        parent.SetActive(me.isOpen);
        thickness = customThickness.value;

        for (int i = 0; i < Colors.Length; i++)
        {
            Colors[i].transform.GetChild(0).GetComponent<Outline>().enabled = Colors[i].isOn;
            if (Colors[i].isOn == true && oldOpen[i] == false)
            {
                for (int j = 0; j < Colors.Length; j++)
                {
                    if (j != i)
                    {
                        Colors[j].isOn = false;
                    }
                }
                matIndex = i;
            }
            oldOpen[i] = Colors[i].isOn;
        }

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            isdrawing = false;
        }

        if (me.isOpen)
        {
            if (Input.GetMouseButtonDown(0))
            {
                isdrawing = true;
                madeMesh = false;
                lastMousePos = mousePos();
            }

            if (Input.GetMouseButton(0) && isdrawing == true)
            {
                if (Vector3.Distance(mousePos(), lastMousePos) > minDistance)
                {
                    if (madeMesh == false)
                    {
                        makeMesh();
                        madeMesh = true;
                    }

                    Mesh drawing = drawings[drawings.Count - 1];

                    Vector3[] vert = new Vector3[drawing.vertices.Length + 2];
                    Vector2[] uv = new Vector2[drawing.uv.Length + 2];
                    int[] tria = new int[drawing.triangles.Length + 6];

                    drawing.vertices.CopyTo(vert, 0);
                    drawing.uv.CopyTo(uv, 0);
                    drawing.triangles.CopyTo(tria, 0);

                    int tI = drawing.triangles.Length;

                    int vI0 = vert.Length - 4;
                    int vI1 = vert.Length - 3;
                    int vI2 = vert.Length - 2;
                    int vI3 = vert.Length - 1;

                    Vector3 mouseDir = (mousePos() - lastMousePos).normalized;
                    Vector3 vUp = mousePos() + Vector3.Cross(mouseDir, Vector3.back) * thickness;
                    Vector3 vDown = mousePos() + Vector3.Cross(mouseDir, Vector3.forward) * thickness;

                    vert[vI2] = vUp + new Vector3(0, 0, -zMultiplier * drawings.Count);
                    vert[vI3] = vDown + new Vector3(0, 0, -zMultiplier * drawings.Count);

                    uv[vI2] = Vector2.zero;
                    uv[vI3] = Vector2.zero;

                    tria[tI + 0] = vI0;
                    tria[tI + 1] = vI2;
                    tria[tI + 2] = vI1;

                    tria[tI + 3] = vI1;
                    tria[tI + 4] = vI2;
                    tria[tI + 5] = vI3;

                    drawing.vertices = vert;
                    drawing.uv = uv;
                    drawing.triangles = tria;

                    lastMousePos = mousePos();
                }
            }
        }

        if (Input.GetKey(KeyCode.LeftControl))
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKeyDown(KeyCode.Z))
                {
                    redo();
                }
            }
            else if (Input.GetKeyDown(KeyCode.Z))
            {
                undo();
            }
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            deletAll();
        }
    }
    void makeMesh()
    {
        Mesh drawing = new Mesh();
        drawings.Add(drawing);

        Vector3[] vert = new Vector3[4];
        Vector2[] uv = new Vector2[4];
        int[] tria = new int[6];

        vert[0] = mousePos();
        vert[1] = mousePos();
        vert[2] = mousePos();
        vert[3] = mousePos();

        uv[0] = new Vector3(0, 0, -zMultiplier * drawings.Count);
        uv[1] = new Vector3(0, 0, -zMultiplier * drawings.Count);
        uv[2] = new Vector3(0, 0, -zMultiplier * drawings.Count);
        uv[3] = new Vector3(0, 0, -zMultiplier * drawings.Count);

        tria[0] = 0;
        tria[1] = 3;
        tria[2] = 1;

        tria[3] = 1;
        tria[4] = 3;
        tria[5] = 2;

        drawing.vertices = vert;
        drawing.uv = uv;
        drawing.triangles = tria;
        drawing.MarkDynamic();

        MeshFilter filter = Instantiate(newFilter, Vector3.zero, Quaternion.identity).GetComponent<MeshFilter>();
        filter.mesh = drawing;
        filter.gameObject.GetComponent<MeshRenderer>().material = materials[matIndex];
        activeFilters.Add(filter);
    }
    public void undo()
    {
        if (activeFilters.Count > 0)
        {
            delet(activeFilters[activeFilters.Count - 1]);
        }
    }
    public void redo()
    {
        if (deadFilters.Count > 0)
        {
            MeshFilter filter = deadFilters[deadFilters.Count - 1];

            filter.gameObject.SetActive(true);

            deadFilters.Remove(filter);
            activeFilters.Add(filter);
        }
    }
    public void deletAll()
    {
        for (int i = 0; i < activeFilters.Count; i++)
        {
            activeFilters[i].gameObject.SetActive(false);
            deadFilters.Add(activeFilters[i]);
        }
        activeFilters.Clear();
    }
    void delet(MeshFilter deleted)
    {
        deleted.gameObject.SetActive(false);
        deadFilters.Add(deleted);
        activeFilters.Remove(deleted);
    }
    Vector3 mousePos()
    {
        Vector3 p = Input.mousePosition;
        p.z = -Camera.main.transform.position.z;
        return Camera.main.ScreenToWorldPoint(p);
    }
}

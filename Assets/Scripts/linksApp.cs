using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Diagnostics;

public class linksApp : MonoBehaviour
{
    [Header("making new buttons")]
    public Button linkButton;
    public TMP_InputField newLink;
    public Transform bButtonParent;

    public List<string> urls = new List<string>();
    public List<Button> buttons = new List<Button>();

    [Header("coloring buttons")]
    public Color link;
    public Color fileOrFolder;

    [Header("using it")]
    public GameObject panel;
    app me;
    private void Awake()
    {
        me = GetComponent<app>();
    }
    private void Start()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("urlsAmount"); i++)
        {
            string url = PlayerPrefs.GetString(i.ToString());

            if (File.Exists(url) || Directory.Exists(url))
            {
                urls.Add(url);

                Button b = Instantiate(linkButton, bButtonParent);
                buttons.Add(b);

                TextMeshProUGUI urlText = b.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                Button removalButton = b.transform.GetChild(1).GetComponent<Button>();

                urlText.text = new DirectoryInfo(url).Name;
                urlText.color = fileOrFolder;

                b.onClick.AddListener(() => { OpenFolder(url); });
                removalButton.onClick.AddListener(() => { removeLink(buttons.IndexOf(b)); });
            }
            else if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
            {
                urls.Add(url);

                Button b = Instantiate(linkButton, bButtonParent);
                buttons.Add(b);

                TextMeshProUGUI urlText = b.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
                Button removalButton = b.transform.GetChild(1).GetComponent<Button>();

                urlText.text = uri.Host.Replace("www.", "");
                urlText.color = link;

                b.onClick.AddListener(() => { OpenURL(url); });
                removalButton.onClick.AddListener(() => { removeLink(buttons.IndexOf(b)); });
            }
        }
    }

    private void Update()
    {
        panel.SetActive(me.isOpen);
    }

    public void addLink()
    {
        string url = newLink.text;
        newLink.text = "";

        if (File.Exists(url) || Directory.Exists(url))
        {
            PlayerPrefs.SetString(urls.Count.ToString(), url);
            urls.Add(url);
            PlayerPrefs.Save();

            Button b = Instantiate(linkButton, bButtonParent);
            buttons.Add(b);

            TextMeshProUGUI urlText = b.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Button removalButton = b.transform.GetChild(1).GetComponent<Button>();

            urlText.text = new DirectoryInfo(url).Name;
            urlText.color = fileOrFolder;

            b.onClick.AddListener(() => { OpenFolder(url); });
            removalButton.onClick.AddListener(() => { removeLink(buttons.IndexOf(b)); });

            PlayerPrefs.SetInt("urlsAmount", urls.Count);
            PlayerPrefs.Save();
        }
        else if (Uri.TryCreate(url, UriKind.Absolute, out Uri uri))
        {
            PlayerPrefs.SetString(urls.Count.ToString(), url);
            urls.Add(url);
            PlayerPrefs.Save();

            Button b = Instantiate(linkButton, bButtonParent);
            buttons.Add(b);

            TextMeshProUGUI urlText = b.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            Button removalButton = b.transform.GetChild(1).GetComponent<Button>();

            urlText.text = uri.Host.Replace("www.", "");
            urlText.color = link;

            b.onClick.AddListener(() => { OpenURL(url); });
            removalButton.onClick.AddListener(() => { removeLink(buttons.IndexOf(b)); });

            PlayerPrefs.SetInt("urlsAmount", urls.Count);
            PlayerPrefs.Save();
        }
    }
    public void removeLink(int index)
    {
        Destroy(buttons[index].gameObject);

        buttons.RemoveAt(index);
        urls.RemoveAt(index);

        for (int i = 0; i < urls.Count; i++)
        {
            PlayerPrefs.SetString(buttons[i].name, urls[i]);
        }
        PlayerPrefs.SetInt("urlsAmount", urls.Count);
        PlayerPrefs.Save();
    }
    public void OpenURL(string url)
    {
        Application.OpenURL(url);
    }
    public void OpenFolder(string path)
    {
        Process.Start(new ProcessStartInfo() { FileName = path, UseShellExecute = true });
    }
}

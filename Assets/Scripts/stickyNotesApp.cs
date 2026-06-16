using System.Collections;
using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.UI;

public class stickyNotesApp : MonoBehaviour
{
    [Header("spawning")]
    public TMP_InputField note;
    public Transform StickyNotesParent;

    [Header("offsetubg")]
    public float offset;

    [Header("saving notes")]
    public List<TMP_InputField> notes;
    List<string> oldText = new List<string>();

    app me;
    private void Awake()
    {
        me = GetComponent<app>();
    }

    private void Start()
    {
        for (int i = 0; i < PlayerPrefs.GetInt("notesCount"); i++)
        {
            TMP_InputField newNote = Instantiate(note, new Vector2(notes.Count * offset, notes.Count * offset), Quaternion.identity);

            newNote.transform.SetParent(StickyNotesParent, false);
            newNote.text = PlayerPrefs.GetString("note" + i);
            
            Button close = newNote.transform.GetChild(1).GetComponent<Button>();
            close.onClick.AddListener(() => { deletNote(newNote); });

            notes.Add(newNote);
            oldText.Add(newNote.text);
        }
    }

    private void Update()
    {
        StickyNotesParent.gameObject.SetActive(me.isOpen);
        for (int i = 0; i < notes.Count; i++)
        {
            notes[i].gameObject.SetActive(me.isOpen);
            if (notes[i].text != oldText[i])
            {
                PlayerPrefs.SetString("note" + i, notes[i].text);
                PlayerPrefs.Save();
            }
            oldText[i] = notes[i].text;
        }
    }
    public void creatNote()
    {
        TMP_InputField newNote = Instantiate(note, new Vector2(notes.Count * offset, notes.Count * offset), Quaternion.identity);
        
        newNote.transform.SetParent(StickyNotesParent, false);
        newNote.Select();

        Button close = newNote.transform.GetChild(1).GetComponent<Button>();
        close.onClick.AddListener(() => { deletNote(newNote); });

        notes.Add(newNote);
        oldText.Add(newNote.text);

        PlayerPrefs.SetInt("notesCount", notes.Count);
        PlayerPrefs.Save();
    }

    void deletNote(TMP_InputField noteToDelete)
    {
        notes.Remove(noteToDelete);
        Destroy(noteToDelete.gameObject);

        PlayerPrefs.SetInt("notesCount", notes.Count);
        PlayerPrefs.Save();
    }
}

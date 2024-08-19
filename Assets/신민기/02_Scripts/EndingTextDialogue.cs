using System.Collections;
using TMPro;
using UnityEngine;

public class EndingTextDialogue : MonoBehaviour
{
    TextMeshProUGUI text;
    public string[] lines;
    public WaitForSeconds textSpeed;
    public Ending ending;
    [SerializeField] GameObject toggle;
    int index = 0;

    private void Awake()
    {
        text = GetComponentInChildren<TextMeshProUGUI>();
        toggle = GameObject.Find("Toggle").gameObject;
    }

    private void Start()
    {
        text.text = string.Empty;
        toggle.SetActive(false);
        StartDialogue();
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (text.text == lines[index] && !ending.isTransitioning)
            {
                NextLine();
                ending.NextImage();
            }
            else
            {
                StopAllCoroutines();
                text.text = lines[index];
                toggle.SetActive(true);
            }
        }
    }
    IEnumerator TypeLine()
    {
        toggle.SetActive(false);
        yield return new WaitForSeconds(1f);
        foreach (char line in lines[index].ToCharArray())
        {
            text.text += line;
            yield return textSpeed;
        }
        toggle.SetActive(true);
    }

    void NextLine()
    {
        if (index >= lines.Length)
            return;

        if (index < lines.Length - 1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            ending.LastEndingScene();
            gameObject.SetActive(false);
        }
    }
}

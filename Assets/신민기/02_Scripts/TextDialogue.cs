using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using UnityEngine.UIElements;

public class TextDialogue : MonoBehaviour
{
    TextMeshProUGUI text;
    public string[] lines;
    public WaitForSeconds textSpeed;
    public ImageSwitcher image;
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
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (text.text == lines[index] && !image.isTransitioning)
            {
                NextLine();
                image.NextImage();
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

        if (index <  lines.Length-1)
        {
            index++;
            text.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            image.GoToMainGame();
            gameObject.SetActive(false);
        }
    }
}

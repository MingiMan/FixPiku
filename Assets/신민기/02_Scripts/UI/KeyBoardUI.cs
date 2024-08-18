using UnityEngine;

public class KeyBoardUI : MonoBehaviour
{
    [SerializeField] GameObject keyBoardUI;
    PlayerMovement player; 

    private bool isUIActive = false; 

    private void Awake()
    {
        player = FindObjectOfType<PlayerMovement>(); 
    }

    private void Start()
    {
        keyBoardUI.SetActive(!isUIActive);
    }

    void Update()
    {
        HandleEscapeKey();
        HandleMouseClick();
        player.IsActive = !keyBoardUI.activeSelf;
    }

    private void HandleEscapeKey()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isUIActive = !isUIActive;
            keyBoardUI.SetActive(isUIActive);
        }
    }

    private void HandleMouseClick()
    {
        if (Input.GetMouseButtonUp(0) && isUIActive)
        {
            isUIActive = false;
            keyBoardUI.SetActive(false);
        }
    }
}

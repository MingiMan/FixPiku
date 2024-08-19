using UnityEngine;
using UnityEngine.UI;

public class MouserCursor : MonoBehaviour
{
    public static MouserCursor Instance;

    [SerializeField] GameObject cursorObject;
    [SerializeField] Sprite cursorBasic;
    [SerializeField] Sprite cursorClick;
    [SerializeField] Sprite cursorWheel;
    [SerializeField] Sprite cursorZoomOut;
    [SerializeField] Sprite cursorZoomIn;
    [SerializeField] Image cursorImage;

    private void Awake()
    {
        cursorImage = GetComponent<Image>();
        cursorObject.GetComponent<RectTransform>().pivot = new Vector2(0, 1);
    }

    private void Start()
    {
        Cursor.visible = false;
    }

    private void Update()
    {
        cursorObject.transform.position = Input.mousePosition;

        if (Input.GetMouseButtonDown(0))
            cursorImage.color = Color.gray;

        if (Input.GetMouseButtonUp(0))
            cursorImage.color = Color.white;

        if (Input.GetMouseButton(1))
            cursorImage.sprite = cursorClick;

        if (Input.GetMouseButtonUp(1))
            CursorBasic();

        if (Input.GetMouseButton(2))
            cursorImage.sprite = cursorWheel;

        if (Input.GetMouseButtonUp(2))
            CursorBasic();
    }

    public void CursorBasic()
    {
        cursorImage.sprite = cursorBasic;
    }
}

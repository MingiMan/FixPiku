using UnityEngine;
using UnityEngine.EventSystems;

public class CursorOver : MonoBehaviour, IPointerEnterHandler
{
    PlayerMovement player;

    void Awake()
    {
        player = FindObjectOfType<PlayerMovement>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Time.timeScale == 1)
        {
            player.IsActive = false;
            player.moveAmount = 0;
        }
    }
}

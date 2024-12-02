using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    List<Transform> points = new List<Transform>();
    PlayerMovement player;

    private void Awake()
    {
        player = GetComponent<PlayerMovement>();
        GameObject.Find("PlayerSpawnPos").GetComponentsInChildren<Transform>(points);
        points.RemoveAt(0);
    }

    public void PlayerSpawn()
    {
        StartCoroutine(PlayerSpawnCoroutine());
    }

    IEnumerator PlayerSpawnCoroutine()
    {
        yield return new WaitForSeconds(3f);

        if (TimeManager.Instance.nightCheck) // 버그 수정
            GameManager.Instance.LevelUp();

        player.IsActive = false;
        GameManager.Instance.theCircleFade.FadeOut();
        yield return new WaitForSeconds(3f);
        player.gameObject.SetActive(false);
        Transform randomPos = points[Random.Range(0, points.Count)].transform;
        player.transform.position = randomPos.position;
        player.gameObject.SetActive(true);
        GameManager.Instance.theCircleFade.FadeIn();
    }
}

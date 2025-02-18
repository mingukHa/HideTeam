using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    public Transform player; // 플레이어 위치
    public Image playerIcon; // 미니맵 내 플레이어 아이콘
    public Image npcIconPrefab; // NPC 아이콘 프리팹
    private List<Transform> npcs = new List<Transform>(); // NPC 목록
    private List<Image> npcIcons = new List<Image>(); // NPC 아이콘 목록

    public RectTransform miniMapPanel; // 미니맵 UI 패널
    public float mapScale = 0.1f; // 월드 좌표 -> 미니맵 좌표 변환 비율
    public string npcTag = "NPC"; // NPC 태그

    private void Start()
    {
        FindAllNPCs(); // ??씬에서 모든 NPC 가져오기
    }

    private void Update()
    {
        UpdatePlayerPosition();
        UpdateNPCPositions();
    }

    // 씬 내 모든 NPC를 찾아 미니맵에 반영
    private void FindAllNPCs()
    {
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag(npcTag); // 태그로 NPC 찾기

        foreach (GameObject npc in npcObjects)
        {
            npcs.Add(npc.transform);
            Image npcIcon = Instantiate(npcIconPrefab, miniMapPanel); // 아이콘 생성
            npcIcons.Add(npcIcon);
        }
    }

    private void UpdatePlayerPosition()
    {
        if (player == null) return;

        // 월드 위치를 미니맵 위치로 변환
        Vector2 miniMapPos = new Vector2(player.position.x * mapScale, player.position.z * mapScale);
        playerIcon.rectTransform.anchoredPosition = miniMapPos;
    }

    private void UpdateNPCPositions()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            if (npcs[i] == null) continue;

            // NPC의 월드 위치를 미니맵 좌표로 변환
            Vector2 miniMapPos = new Vector2(npcs[i].position.x * mapScale, npcs[i].position.z * mapScale);
            npcIcons[i].rectTransform.anchoredPosition = miniMapPos;
        }
    }
}

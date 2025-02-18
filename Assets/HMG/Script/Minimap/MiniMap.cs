using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Transform player; // 플레이어 위치
    public Image playerIcon; // 미니맵 내 플레이어 아이콘
    public Transform[] npcs; // NPC 목록
    public Image npcIconPrefab; // NPC 아이콘 프리팹
    private Image[] npcIcons; // NPC 아이콘 배열

    public RectTransform miniMapPanel; // 미니맵 UI 패널
    public float mapScale = 0.1f; // 월드 좌표 -> 미니맵 좌표 변환 비율

    private void Start()
    {
        // NPC 아이콘을 동적으로 생성하여 관리
        npcIcons = new Image[npcs.Length];
        for (int i = 0; i < npcs.Length; i++)
        {
            npcIcons[i] = Instantiate(npcIconPrefab, miniMapPanel);
        }
    }

    private void Update()
    {
        UpdatePlayerPosition();
        UpdateNPCPositions();
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
        for (int i = 0; i < npcs.Length; i++)
        {
            if (npcs[i] == null) continue;

            // NPC의 월드 위치를 미니맵 좌표로 변환
            Vector2 miniMapPos = new Vector2(npcs[i].position.x * mapScale, npcs[i].position.z * mapScale);
            npcIcons[i].rectTransform.anchoredPosition = miniMapPos;
        }
    }
}

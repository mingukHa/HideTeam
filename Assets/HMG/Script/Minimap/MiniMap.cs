using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    public Transform player; // 플레이어 위치
    public Vector2 worldSize; // 건물 내부의 실제 크기
    public Vector2 miniMapSize; // 미니맵 UI 크기

    public RectTransform playerIcon; // 미니맵 위의 플레이어 아이콘
    private GameObject activeMiniMap; // 현재 활성화된 미니맵

    public Dictionary<string, GameObject> miniMaps = new Dictionary<string, GameObject>(); // 건물별 미니맵 관리

    void Update()
    {
        if (activeMiniMap == null) return;

        // 플레이어 좌표를 미니맵 UI 좌표로 변환
        Vector2 normalizedPos = new Vector2(
            (player.position.x / worldSize.x) + 0.5f,
            (player.position.z / worldSize.y) + 0.5f
        );

        Vector2 miniMapPos = new Vector2(
            (normalizedPos.x * miniMapSize.x) - (miniMapSize.x * 0.5f),
            (normalizedPos.y * miniMapSize.y) - (miniMapSize.y * 0.5f)
        );

        playerIcon.anchoredPosition = miniMapPos; // 플레이어 아이콘 위치 적용
    }

    // 건물 미니맵을 변경하는 함수
    public void ActivateMiniMap(string buildingName)
    {
        if (miniMaps.ContainsKey(buildingName))
        {
            if (activeMiniMap != null) activeMiniMap.SetActive(false); // 이전 미니맵 숨기기
            activeMiniMap = miniMaps[buildingName];
            activeMiniMap.SetActive(true); // 새로운 미니맵 활성화
        }
    }
}
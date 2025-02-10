using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ
    public Vector2 worldSize; // �ǹ� ������ ���� ũ��
    public Vector2 miniMapSize; // �̴ϸ� UI ũ��

    public RectTransform playerIcon; // �̴ϸ� ���� �÷��̾� ������
    private GameObject activeMiniMap; // ���� Ȱ��ȭ�� �̴ϸ�

    public Dictionary<string, GameObject> miniMaps = new Dictionary<string, GameObject>(); // �ǹ��� �̴ϸ� ����

    void Update()
    {
        if (activeMiniMap == null) return;

        // �÷��̾� ��ǥ�� �̴ϸ� UI ��ǥ�� ��ȯ
        Vector2 normalizedPos = new Vector2(
            (player.position.x / worldSize.x) + 0.5f,
            (player.position.z / worldSize.y) + 0.5f
        );

        Vector2 miniMapPos = new Vector2(
            (normalizedPos.x * miniMapSize.x) - (miniMapSize.x * 0.5f),
            (normalizedPos.y * miniMapSize.y) - (miniMapSize.y * 0.5f)
        );

        playerIcon.anchoredPosition = miniMapPos; // �÷��̾� ������ ��ġ ����
    }

    // �ǹ� �̴ϸ��� �����ϴ� �Լ�
    public void ActivateMiniMap(string buildingName)
    {
        if (miniMaps.ContainsKey(buildingName))
        {
            if (activeMiniMap != null) activeMiniMap.SetActive(false); // ���� �̴ϸ� �����
            activeMiniMap = miniMaps[buildingName];
            activeMiniMap.SetActive(true); // ���ο� �̴ϸ� Ȱ��ȭ
        }
    }
}
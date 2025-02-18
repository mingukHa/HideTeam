using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MiniMap : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ
    public Image playerIcon; // �̴ϸ� �� �÷��̾� ������
    public Image npcIconPrefab; // NPC ������ ������
    private List<Transform> npcs = new List<Transform>(); // NPC ���
    private List<Image> npcIcons = new List<Image>(); // NPC ������ ���

    public RectTransform miniMapPanel; // �̴ϸ� UI �г�
    public float mapScale = 0.1f; // ���� ��ǥ -> �̴ϸ� ��ǥ ��ȯ ����
    public string npcTag = "NPC"; // NPC �±�

    private void Start()
    {
        FindAllNPCs(); // ??������ ��� NPC ��������
    }

    private void Update()
    {
        UpdatePlayerPosition();
        UpdateNPCPositions();
    }

    // �� �� ��� NPC�� ã�� �̴ϸʿ� �ݿ�
    private void FindAllNPCs()
    {
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag(npcTag); // �±׷� NPC ã��

        foreach (GameObject npc in npcObjects)
        {
            npcs.Add(npc.transform);
            Image npcIcon = Instantiate(npcIconPrefab, miniMapPanel); // ������ ����
            npcIcons.Add(npcIcon);
        }
    }

    private void UpdatePlayerPosition()
    {
        if (player == null) return;

        // ���� ��ġ�� �̴ϸ� ��ġ�� ��ȯ
        Vector2 miniMapPos = new Vector2(player.position.x * mapScale, player.position.z * mapScale);
        playerIcon.rectTransform.anchoredPosition = miniMapPos;
    }

    private void UpdateNPCPositions()
    {
        for (int i = 0; i < npcs.Count; i++)
        {
            if (npcs[i] == null) continue;

            // NPC�� ���� ��ġ�� �̴ϸ� ��ǥ�� ��ȯ
            Vector2 miniMapPos = new Vector2(npcs[i].position.x * mapScale, npcs[i].position.z * mapScale);
            npcIcons[i].rectTransform.anchoredPosition = miniMapPos;
        }
    }
}

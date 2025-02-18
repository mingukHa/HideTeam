using UnityEngine;
using UnityEngine.UI;

public class MiniMap : MonoBehaviour
{
    public Transform player; // �÷��̾� ��ġ
    public Image playerIcon; // �̴ϸ� �� �÷��̾� ������
    public Transform[] npcs; // NPC ���
    public Image npcIconPrefab; // NPC ������ ������
    private Image[] npcIcons; // NPC ������ �迭

    public RectTransform miniMapPanel; // �̴ϸ� UI �г�
    public float mapScale = 0.1f; // ���� ��ǥ -> �̴ϸ� ��ǥ ��ȯ ����

    private void Start()
    {
        // NPC �������� �������� �����Ͽ� ����
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

        // ���� ��ġ�� �̴ϸ� ��ġ�� ��ȯ
        Vector2 miniMapPos = new Vector2(player.position.x * mapScale, player.position.z * mapScale);
        playerIcon.rectTransform.anchoredPosition = miniMapPos;
    }

    private void UpdateNPCPositions()
    {
        for (int i = 0; i < npcs.Length; i++)
        {
            if (npcs[i] == null) continue;

            // NPC�� ���� ��ġ�� �̴ϸ� ��ǥ�� ��ȯ
            Vector2 miniMapPos = new Vector2(npcs[i].position.x * mapScale, npcs[i].position.z * mapScale);
            npcIcons[i].rectTransform.anchoredPosition = miniMapPos;
        }
    }
}

using System.Collections;
using UnityEngine;

public class NPCReturnHandler : MonoBehaviour
{
    private Vector3 npcPosition;
    private Quaternion npcRotation;
    private GameObject player;
    public ReturnManager returnManager;
    public float recordTime = 3f; // �⺻ ���� �ð�
    private SceneDataStack sceneDataStack;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        sceneDataStack = returnManager.GetSceneDataStack();
    }

    public IEnumerator ReturnStack(float maxTime)
    {
        Debug.Log($"{gameObject.name} - ���� ���� ��� ����");
        float currentTime = 0f;

        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;

            // ���� NPC �� �÷��̾� ��ġ ����
            npcPosition = transform.position;
            Quaternion npcRotation = transform.rotation;
            Vector3 playerPosition = player.transform.position;
            Quaternion playerRotation = player.transform.rotation;

            // ���ÿ� ������ ����
            SceneData returnData = new SceneData(npcPosition, playerPosition, npcRotation, playerRotation, "Talk", "Walk", maxTime);
            
            if (sceneDataStack != null)
            {
                sceneDataStack.PushSceneData(returnData);
            }
            else
            {
                Debug.LogError("SceneDataStack�� �������� �ʽ��ϴ�!");
            }

            yield return null;
        }

        Debug.Log($"{gameObject.name} - ���� ���� ��� ����");
    }
}

using System.Collections;
using UnityEngine;

public class NPCReturnHandler : MonoBehaviour
{
    private Vector3 npcPosition;
    private Quaternion npcRotation;
    private GameObject player;
    public ReturnManager returnManager;
    public float recordTime = 3f; // 기본 저장 시간
    private SceneDataStack sceneDataStack;

    private void Start()
    {
        player = GameObject.FindWithTag("Player");
        sceneDataStack = returnManager.GetSceneDataStack();
    }

    public IEnumerator ReturnStack(float maxTime)
    {
        Debug.Log($"{gameObject.name} - 리턴 스택 기록 시작");
        float currentTime = 0f;

        while (currentTime < maxTime)
        {
            currentTime += Time.deltaTime;

            // 현재 NPC 및 플레이어 위치 저장
            npcPosition = transform.position;
            Quaternion npcRotation = transform.rotation;
            Vector3 playerPosition = player.transform.position;
            Quaternion playerRotation = player.transform.rotation;

            // 스택에 데이터 저장
            SceneData returnData = new SceneData(npcPosition, playerPosition, npcRotation, playerRotation, "Talk", "Walk", maxTime);
            
            if (sceneDataStack != null)
            {
                sceneDataStack.PushSceneData(returnData);
            }
            else
            {
                Debug.LogError("SceneDataStack이 존재하지 않습니다!");
            }

            yield return null;
        }

        Debug.Log($"{gameObject.name} - 리턴 스택 기록 종료");
    }
}

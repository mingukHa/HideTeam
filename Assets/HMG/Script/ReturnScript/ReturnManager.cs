using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnManager : MonoBehaviour
{
    private Dictionary<GameObject, SceneDataStack> npcSceneStacks = new Dictionary<GameObject, SceneDataStack>(); // NPC별 스택 관리
    private GameObject Player;
    public GameObject fadeout;
    private CharacterController PlayerControllers;
    [SerializeField] private GameObject Post;
    [SerializeField] private PlayerController mouse;
    private RootMotionController RootMotionController;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        PlayerControllers = Player.GetComponent<CharacterController>();
        RootMotionController = Player.GetComponent<RootMotionController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(SaveAllNPCData(4f)); // NPC별 데이터를 저장 (지속적으로)
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            StartCoroutine(ReturnAllNPCs()); // 모든 NPC 데이터 복원
        }
    }

    // ✅ 일정 시간 동안 지속적으로 모든 NPC 및 플레이어의 데이터를 저장
    public IEnumerator SaveAllNPCData(float duration)
    {
        Debug.Log("모든 NPC 데이터 저장 시작");

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC")) // 모든 NPC 탐색
            {
                if (!npcSceneStacks.ContainsKey(npc))
                {
                    npcSceneStacks[npc] = new SceneDataStack(); // NPC별로 스택 생성
                    Debug.Log($"NPC 스택 갯수: {npcSceneStacks.Count}");
                }

                Vector3 npcPosition = npc.transform.position;
                Quaternion npcRotation = npc.transform.rotation;
                Vector3 playerPosition = Player.transform.position;
                Quaternion playerRotation = Player.transform.rotation;

                //  각 프레임마다 새로운 데이터를 스택에 저장
                SceneData returnData = new SceneData(npcPosition, playerPosition, npcRotation, playerRotation,"Talk","Talk", duration);
                npcSceneStacks[npc].PushSceneData(returnData);
            }

            elapsedTime += Time.deltaTime;
            yield return null; // **한 프레임 대기 후 다시 저장**
        }

        Debug.Log("모든 NPC 데이터 저장 완료");
    }

    //  모든 NPC와 플레이어를 한 번에 복원하는 함수 (애니메이션 제거)
    public IEnumerator ReturnAllNPCs()
    {
        yield return new WaitForSeconds(0.8f); // 초기 대기 시간
        Debug.Log("모든 NPC 복원 시작");

        bool hasData = false;

        foreach (var kvp in npcSceneStacks)
        {
            if (kvp.Value.GetSceneCount() > 0)
            {
                hasData = true;
                break;
            }
        }

        if (!hasData)
        {
            Debug.Log("스택이 비어 있음 - 복원할 데이터가 없습니다.");
            yield break;
        }

        PlayerOff();
        Post.SetActive(true);

        

        // ✅ 모든 NPC 스택에서 가장 많은 데이터를 저장한 개수 찾기
        int maxFrames = 0;
        foreach (var kvp in npcSceneStacks)
        {
            if (kvp.Value.GetSceneCount() > maxFrames)
            {
                maxFrames = kvp.Value.GetSceneCount();
            }
        }

        while (maxFrames > 0) // **최대 저장된 프레임 개수만큼 반복**
        {
            bool anyDataLeft = false;

            foreach (var kvp in npcSceneStacks)
            {
                GameObject npc = kvp.Key;
                SceneDataStack stack = kvp.Value;

                if (stack.GetSceneCount() > 0)
                {
                    anyDataLeft = true; // **적어도 하나의 NPC는 복원이 가능함**
                    SceneData popReturn = stack.PopSceneData();

                    npc.transform.position = popReturn.NpcPosition;
                    npc.transform.rotation = popReturn.NpcRotation;
                }
            }

            // **플레이어 위치 복원 (NPC 중 하나의 스택 기준)**
            GameObject firstNPC = GameObject.FindGameObjectsWithTag("NPC")[0];
            if (npcSceneStacks.ContainsKey(firstNPC) && npcSceneStacks[firstNPC].GetSceneCount() > 0)
            {
                SceneData playerData = npcSceneStacks[firstNPC].PopSceneData();
                Player.transform.position = playerData.PlayerPosition;
                Player.transform.rotation = playerData.PlayerRotation;

                // ✅ 장면 간격(duration) 적용하여 자연스럽게 복원
                yield return new WaitForSeconds(playerData.Duration / maxFrames);
            }

            maxFrames--; // 남은 프레임 수 감소

            // **모든 데이터가 복원되었다면 종료**
            if (!anyDataLeft) break;
        }

        SceneManager.LoadScene("MainScene"); // 씬 복원
    }

    public void PlayerOff()
    {
        PlayerControllers.enabled = false;
        mouse.enabled = false;
        RootMotionController.enabled = false;
    }
}

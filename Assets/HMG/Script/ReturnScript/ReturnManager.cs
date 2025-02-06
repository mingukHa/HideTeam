using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnManager : MonoBehaviour
{
    private SceneDataStack sceneDataStack = new SceneDataStack();
    private GameObject player;
    private CharacterController playerController;

    public GameObject fadeOut;
    [SerializeField] private GameObject postEffect;
    [SerializeField] private PlayerController mouseController;

    private void Awake()
    {
        player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            playerController = player.GetComponent<CharacterController>();
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(StartNPCRecording()); //테스트용임 콜백으로 변경 해야함
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            if (sceneDataStack.GetSceneCount() > 0)
            {
                PlayerOff();
                StartCoroutine(ReturnPlay());
            }
            else
            {
                Debug.Log("스택이 비어 있음");
            }
        }
    }

    // 모든 NPC가 ReturnStack()을 실행하도록 명령
    public IEnumerator StartNPCRecording()
    {
        Debug.Log("모든 NPC에게 ReturnStack() 실행 명령");

        // 씬 내 모든 NPC를 찾음
        //NPCReturnHandler[] npcs = FindObjectsOfType<NPCReturnHandler>();
        GameObject[] npcObjects = GameObject.FindGameObjectsWithTag("NPC");
        Debug.Log($"{npcObjects.Length}");
        NPCReturnHandler[] npcs = System.Array.ConvertAll(npcObjects, obj => obj.GetComponent<NPCReturnHandler>()).Where(npc => npc != null).ToArray();

        foreach (NPCReturnHandler npc in npcs)
        {
            StartCoroutine(npc.ReturnStack(3f));
        }

        yield return new WaitForSeconds(3f);
    }

    // 씬 복구 실행
    public IEnumerator ReturnPlay()
    {
        while (sceneDataStack.GetSceneCount() > 0)
        {
            SceneData popReturn = sceneDataStack.PopSceneData();

            if (popReturn != null)
            {
                postEffect.SetActive(true);

                transform.position = popReturn.NpcPosition;
                transform.rotation = popReturn.NpcRotation;
                player.transform.position = popReturn.PlayerPosition;
                player.transform.rotation = popReturn.PlayerRotation;

                yield return null;
            }
        }

        SceneManager.LoadScene("MainScene");
    }

    // 플레이어 움직임 비활성화
    public void PlayerOff()
    {
        if (playerController != null)
            playerController.enabled = false;

        if (mouseController != null)
            mouseController.enabled = false;
    }

    // SceneDataStack을 다른 스크립트에서 접근할 수 있도록 제공
    public SceneDataStack GetSceneDataStack()
    {
        return sceneDataStack;
    }
}

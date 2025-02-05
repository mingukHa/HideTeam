using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnManager : MonoBehaviour
{
    private SceneDataStack sceneDataStack = new SceneDataStack();
    private GameObject Player;
    private CharacterController playerController;
    [SerializeField]
    private GameObject Post;

    public PlayerController Mouse;
    private Vector3 Playerposition;
    private Vector3 Npcposition;
    private Quaternion Npcquaternion;
    private Quaternion Playerquaternion;
    private Animator npcAnimator;
    
    private Animator playerAnimator;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        playerController = Player.GetComponent<CharacterController>();
        npcAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            StartCoroutine(ReturnStack(3f)); // 1초 동안 데이터 저장
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneData popedScene = sceneDataStack.PopSceneData();

            if (popedScene != null)
            {
                StartCoroutine(ReturnPlay());
            }
            else
            {
                Debug.Log("스택이 비어 있음! 위치를 복원할 수 없음.");
            }
        }
    }

    public IEnumerator ReturnPlay()
    {
        Debug.Log("빠른 재생 시작");

        while (sceneDataStack.GetSceneCount() > 0)
        {
            SceneData popedScene = sceneDataStack.PopSceneData();

            if (popedScene != null)
            {
                Post.SetActive(true);
                Mouse.enabled = true;
                // 플레이어 및 NPC 위치, 회전 복원
                playerController.enabled = false;
                transform.position = popedScene.NpcPosition;
                transform.rotation = popedScene.NpcRotation;
                Player.transform.position = popedScene.PlayerPosition;
                Player.transform.rotation = popedScene.PlayerRotation;

                // NPC 애니메이션 실행
                npcAnimator.Play(popedScene.NpcAnimation);

                Debug.Log($"재생: Player 위치: {popedScene.PlayerPosition}, NPC 위치: {popedScene.NpcPosition}");

                yield return null; // 지정된 속도로 반복
            }
        }
        SceneManager.LoadScene("MGRealTest");
        playerController.enabled = true;
    }


    public IEnumerator ReturnStack(float MaxTime)
    {
        Debug.Log("리턴 코루틴 실행");
        float CurrentTime = 0f;

        while (CurrentTime < MaxTime) 
        {
            CurrentTime += Time.deltaTime; 

            Npcposition = transform.position;
            Playerposition = Player.transform.position;
            Npcquaternion = transform.rotation;
            Playerquaternion = Player.transform.rotation;

            SceneData scene1 = new SceneData(Npcposition, Playerposition, Npcquaternion, Playerquaternion, "Talk", "Walk"/*플레이어 애니메이션*/, MaxTime, MaxTime);
            sceneDataStack.PushSceneData(scene1);

            Debug.Log("스택에 저장됨! Player 위치: " + Playerposition + ", NPC 위치: " + Npcposition);

            yield return null; // 매 프레임 대기
        }

        Debug.Log("리턴 코루틴 종료");
    }
}

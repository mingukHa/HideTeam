using UnityEngine;

public class ReturnManager : MonoBehaviour
{
    private SceneDataStack sceneDataStack = new SceneDataStack(); // 스택 데이터 저장
    private GameObject Player;

    private Vector3 Playerposition;
    private Vector3 Npcposition;
    private Quaternion Npcquaternion;
    private Quaternion Playerquaternion;
    private Animator npcAnimator;
    private Animator playerAnimator;
    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
             
    }
    private void Start()
    {
        npcAnimator = GetComponent<Animator>();
        playerAnimator = GetComponent<Animator>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            ReturnLong();
            Debug.Log("스택에 저장됨");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneData popedScene = sceneDataStack.PopSceneData();
            ReturnPlay(popedScene);
        }
    }
    public void ReturnPlay(SceneData popedScene) //리턴 재생
    {        
        if (popedScene != null)
        {
            // NPC와 Player 위치 및 회전 복원
            transform.position = popedScene.NpcPosition;
            transform.rotation = popedScene.NpcRotation;
            Player.transform.position = popedScene.PlayerPosition;
            Player.transform.rotation = popedScene.PlayerRotation;
            npcAnimator.Play(popedScene.NpcAnimation);
            playerAnimator.Play(popedScene.PlayerAnimation);
        }
        else
        {
            Debug.Log("스택이 비어 있음!");
        }
    }

    public void ReturnLong() //리턴 정보 남기기
    {
        // 현재 위치 및 회전 저장
        Npcposition = transform.position;
        Playerposition = Player.transform.position;
        Npcquaternion = transform.rotation;
        Playerquaternion = Player.transform.rotation;
        string npcAnimation = GetCurrentAnimation(npcAnimator);
        string playerAnimation = GetCurrentAnimation(playerAnimator);

        SceneData scene1 = new SceneData(Npcposition, Playerposition, Npcquaternion, Playerquaternion, npcAnimation, playerAnimation, 3f, 3f);

        sceneDataStack.PushSceneData(scene1);
    }
    private string GetCurrentAnimation(Animator animator) //애니메이션 정보 받기
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); //에니메이터 받아오기
        Debug.Log($"{animator.GetCurrentAnimatorClipInfo(0)[0].clip.name}");
        return animator.GetCurrentAnimatorClipInfo(0).Length > 0 ? animator.GetCurrentAnimatorClipInfo(0)[0].clip.name : "No Animation";
    }
}

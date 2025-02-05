using UnityEngine;

public class ReturnManager : MonoBehaviour
{
    private SceneDataStack sceneDataStack = new SceneDataStack(); // ���� ������ ����
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
            Debug.Log("���ÿ� �����");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneData popedScene = sceneDataStack.PopSceneData();
            ReturnPlay(popedScene);
        }
    }
    public void ReturnPlay(SceneData popedScene) //���� ���
    {        
        if (popedScene != null)
        {
            // NPC�� Player ��ġ �� ȸ�� ����
            transform.position = popedScene.NpcPosition;
            transform.rotation = popedScene.NpcRotation;
            Player.transform.position = popedScene.PlayerPosition;
            Player.transform.rotation = popedScene.PlayerRotation;
            npcAnimator.Play(popedScene.NpcAnimation);
            playerAnimator.Play(popedScene.PlayerAnimation);
        }
        else
        {
            Debug.Log("������ ��� ����!");
        }
    }

    public void ReturnLong() //���� ���� �����
    {
        // ���� ��ġ �� ȸ�� ����
        Npcposition = transform.position;
        Playerposition = Player.transform.position;
        Npcquaternion = transform.rotation;
        Playerquaternion = Player.transform.rotation;
        string npcAnimation = GetCurrentAnimation(npcAnimator);
        string playerAnimation = GetCurrentAnimation(playerAnimator);

        SceneData scene1 = new SceneData(Npcposition, Playerposition, Npcquaternion, Playerquaternion, npcAnimation, playerAnimation, 3f, 3f);

        sceneDataStack.PushSceneData(scene1);
    }
    private string GetCurrentAnimation(Animator animator) //�ִϸ��̼� ���� �ޱ�
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); //���ϸ����� �޾ƿ���
        Debug.Log($"{animator.GetCurrentAnimatorClipInfo(0)[0].clip.name}");
        return animator.GetCurrentAnimatorClipInfo(0).Length > 0 ? animator.GetCurrentAnimatorClipInfo(0)[0].clip.name : "No Animation";
    }
}

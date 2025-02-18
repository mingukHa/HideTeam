using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnManagerinit : MonoBehaviour
{
    private Dictionary<GameObject, SceneDataStack> npcSceneStacks = new Dictionary<GameObject, SceneDataStack>();
    private GameObject Player;
    private Animator playerAnimator;
    public GameObject fadeout;
    private CharacterController PlayerControllers;
    [SerializeField] private GameObject Post;
    [SerializeField] private PlayerController mouse;
    private RootMotionController RootMotionController;

    private void Awake()
    {
        Player = GameObject.FindWithTag("Player");
        playerAnimator = Player.GetComponent<Animator>(); // �÷��̾� �ִϸ����� ��������
        PlayerControllers = Player.GetComponent<CharacterController>();
        RootMotionController = Player.GetComponent<RootMotionController>();
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Q))
        //{
        //    StartCoroutine(SaveAllNPCData(4f)); // NPC ������ ����
        //}

        //if (Input.GetKeyDown(KeyCode.K))
        //{
        //    StartCoroutine(ReturnAllNPCs()); // NPC �� �÷��̾� ������ ����
        //}
    }

    public IEnumerator SaveAllNPCData(float duration)
    {
        Debug.Log("��� NPC ������ ���� ����");

        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            foreach (GameObject npc in GameObject.FindGameObjectsWithTag("NPC"))
            {
                if (!npcSceneStacks.ContainsKey(npc))
                {
                    npcSceneStacks[npc] = new SceneDataStack();
                }

                Vector3 npcPosition = npc.transform.position;
                Quaternion npcRotation = npc.transform.rotation;
                Vector3 playerPosition = Player.transform.position;
                Quaternion playerRotation = Player.transform.rotation;

                // **NPC �ִϸ��̼� ����**
                Animator npcAnimator = npc.GetComponent<Animator>();
                string npcAnimation = npcAnimator != null ? GetCurrentAnimation(npcAnimator) : "Idle";

                // **�÷��̾� �ִϸ��̼� ����**
                string playerAnimation = playerAnimator != null ? GetCurrentAnimation(playerAnimator) : "Idle";

                SceneData returnData = new SceneData(npcPosition, playerPosition, npcRotation, playerRotation, npcAnimation, playerAnimation, duration);
                npcSceneStacks[npc].PushSceneData(returnData);
            }

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Debug.Log("��� NPC ������ ���� �Ϸ�");
    }

    public IEnumerator ReturnAllNPCs()
    {
        yield return new WaitForSeconds(0.8f);
        Debug.Log("��� NPC ���� ����");

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
            Debug.Log("������ ��� ���� - ������ �����Ͱ� �����ϴ�.");
            yield break;
        }

        PlayerOff();
        Post.SetActive(true);

        int maxFrames = 0;
        foreach (var kvp in npcSceneStacks)
        {
            if (kvp.Value.GetSceneCount() > maxFrames)
            {
                maxFrames = kvp.Value.GetSceneCount();
            }
        }

        while (maxFrames > 0)
        {
            bool anyDataLeft = false;

            foreach (var kvp in npcSceneStacks)
            {
                GameObject npc = kvp.Key;
                SceneDataStack stack = kvp.Value;

                if (stack.GetSceneCount() > 0)
                {
                    anyDataLeft = true;
                    SceneData popReturn = stack.PopSceneData();

                    npc.transform.position = popReturn.NpcPosition;
                    npc.transform.rotation = popReturn.NpcRotation;

                    // **NPC �ִϸ��̼� ����**
                    Animator npcAnimator = npc.GetComponent<Animator>();
                    if (npcAnimator != null)
                    {
                        npcAnimator.Play(popReturn.NpcAnimation);
                    }
                }
            }

            // **�÷��̾� ��ġ �� �ִϸ��̼� ����**
            GameObject firstNPC = GameObject.FindGameObjectsWithTag("NPC")[0];
            if (npcSceneStacks.ContainsKey(firstNPC) && npcSceneStacks[firstNPC].GetSceneCount() > 0)
            {
                SceneData playerData = npcSceneStacks[firstNPC].PopSceneData();
                Player.transform.position = playerData.PlayerPosition;
                Player.transform.rotation = playerData.PlayerRotation;

                // **�÷��̾� �ִϸ��̼� ����**
                if (playerAnimator != null)
                {
                    playerAnimator.Play(playerData.PlayerAnimation);
                }

                yield return new WaitForSeconds(playerData.Duration / maxFrames);
            }

            maxFrames--;

            if (!anyDataLeft) break;
        }

        SceneManager.LoadScene("MainScene");
    }

    public void PlayerOff()
    {
        PlayerControllers.enabled = false;
        mouse.enabled = false;
        RootMotionController.enabled = false;
    }

    // ���� ��� ���� �ִϸ��̼� �̸� ��������
    private string GetCurrentAnimation(Animator animator)
    {
        if (animator != null)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
            return stateInfo.IsName("Talk") ? "Talk" : "Idle";
        }
        return "Idle";
    }
}

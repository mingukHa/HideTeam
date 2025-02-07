using Unity.VisualScripting;
using UnityEngine;

public class NPC_OldMan : NPCFSM
{
    [SerializeField] private GameObject select;
    private NPCChatTest chat;
    public GameObject npcchatbox;

    private string npc = "NPC3";
    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();
        select.SetActive(false);
    }

    protected override void Update()
    {
        base.Update(); 
    }

    protected override void IdleBehavior()
    {
        base.IdleBehavior();
    }

    protected override void LookBehavior()
    {
        base.LookBehavior();
    }

    protected override void WalkBehavior()
    {
        base.WalkBehavior();
    }

    protected override void RunBehavior()
    {
        base.RunBehavior();
    }

    protected override void TalkBehavior()
    {
        base.TalkBehavior();
    }

    protected override void DeadBehavior()
    {
        base.DeadBehavior();
        npcchatbox.SetActive(false);
        chat.LoadNPCDialogue("NULL", 0); //�����ڴ� ���� ����
    }

    private void OnTriggerEnter(Collider other) //��ȭ ����
    {
        if (isDead == false)
        {
            if (other.CompareTag("Player"))
            {
                select.SetActive(true);
                ChangeState(State.Talk);
                chat.LoadNPCDialogue(npc, 0);
            }
        }
    }
    private void OnTriggerStay(Collider other) //��ȭ ������
    {
        if (isDead == false)
        {
            if (Input.GetKey(KeyCode.O))
            {
                chat.LoadNPCDialogue(npc, 2);
            }
            if (Input.GetKey(KeyCode.P))
            {
                chat.LoadNPCDialogue(npc, 1);
            }
        }
    }
    private void OnTriggerExit(Collider other) //��ȭ ����
    {
        if (other.CompareTag("Player"))
        {
            ChangeState(State.Idle);
            select.SetActive(false);
            chat.LoadNPCDialogue("NULL", 0);
        }
    }
}

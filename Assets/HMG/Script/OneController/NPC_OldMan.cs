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
        chat.LoadNPCDialogue("NULL", 0); //죽은자는 말이 없다
    }

    private void OnTriggerEnter(Collider other) //대화 시작
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
    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other); 

        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 2);
            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 1);
            }
        }
    }

    private void OnTriggerExit(Collider other) //대화 종료
    {
        if (other.CompareTag("Player"))
        {
            ChangeState(State.Idle);
            select.SetActive(false);
            chat.LoadNPCDialogue("NULL", 0);
        }
    }
    
}

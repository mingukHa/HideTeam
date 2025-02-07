using UnityEngine;

using Unity.VisualScripting;

public class NPC_NPC2 : NPCFSM
{
    [SerializeField] private GameObject select;
    private NPCChatTest chat;
    public GameObject npcchatbox;
    private string npc = "NPC2";
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
        chat.LoadNPCDialogue("NULL", 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isDead == false)
        {
            if (other.CompareTag("Player"))
            {
                select.SetActive(true);
                ChangeState(State.Talk);
                chat.LoadNPCDialogue(npc, 0);
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {

                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeState(State.Idle);
            select.SetActive(false);
            chat.LoadNPCDialogue("NULL", 0);
        }
    }
}

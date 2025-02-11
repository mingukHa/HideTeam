using Unity.VisualScripting;
using UnityEngine;
using static EventManager;

public class NPC_OldMan : NPCFSM
{
    [SerializeField] private GameObject select;
    private NPCChatTest chat;
    public GameObject npcchatbox;
    public ReturnManager returnManager;
    private string npc = "NPC3";

    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.NPC1Talk, StartTalking);
        EventManager.Subscribe(GameEventType.NPC1Fun, StartLaughing);
    }

    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.NPC1Talk, StartTalking);
        EventManager.Unsubscribe(GameEventType.NPC1Fun, StartLaughing);
    }
    private void StartTalking()
    {
        //�̷� �ൿ�� �Ұ���
    }
    private void StartLaughing()
    {
        //�̷� �ൿ�� �Ұ���
    }
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
    protected override void OnTriggerStay(Collider other)
    {
        base.OnTriggerStay(other); 

        if (other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 2);
                EventManager.Trigger(GameEventType.NPC1Talk);
                

            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 1); 
                EventManager.Trigger(GameEventType.NPC1Fun);
                //EventManager.Unsubscribe(GameEventType.Talk, StartTalking); �ٽô� �� �� �̺�Ʈ�� ����
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

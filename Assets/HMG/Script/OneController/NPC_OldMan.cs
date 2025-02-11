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
        //이런 행동을 할거임
    }
    private void StartLaughing()
    {
        //이런 행동을 할거임
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
                EventManager.Trigger(GameEventType.NPC1Talk);
                

            }
            if (Input.GetKey(KeyCode.Alpha2))
            {
                chat.LoadNPCDialogue(npc, 1); 
                EventManager.Trigger(GameEventType.NPC1Fun);
                //EventManager.Unsubscribe(GameEventType.Talk, StartTalking); 다시는 안 쓸 이벤트는 해제
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

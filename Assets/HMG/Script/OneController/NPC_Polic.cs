using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;


public class NPC_Polic : NPCFSM
{

    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    private string npc = "NPC5";
    public Transform PolicPos; //이동 할 위치    
    private bool isPolicTlak = false;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.OldManHelp, StartPolicTlak);
    }
    private void StartPolicTlak()
    {
        isPolicTlak = true;       
    }
 
    private void StopNpc()
    {
        StopCoroutine(TalkView());
        transform.rotation = initrotation;
        NPCCollider.radius = 0.01f;
        animator.SetTrigger("Idel");
        select.SetActive(false);
    }

    protected override void Start()
    {
        base.Start();
        select.SetActive(false);
        chat = GetComponent<NPCChatTest>();

        agent = GetComponent<NavMeshAgent>();
        NPCCollider = GetComponent<SphereCollider>();
    }

    protected override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.F))
        {
            isDead = true;
        }
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

    protected override void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue(npc, 0);
        }

    }
    protected override void OnTriggerStay(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {           
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                chat.LoadNPCDialogue(npc, 1);
                EventManager.Trigger(GameEventType.policeTalk);
                StopCoroutine(TalkView());
                returnManager.StartCoroutine(returnManager.SaveAllNPCData(4f));
                Invoke("StopNpc", 2f);
            }
        }
        if (Input.GetKey(KeyCode.F))
        {
            isDead = true;
            EventManager.Trigger(GameEventType.NPCKill);
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue("NULL", 0);
        }

    }
}


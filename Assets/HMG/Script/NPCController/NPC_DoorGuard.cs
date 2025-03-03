using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;

public class NPC_DoorGaurd : NPCFSM

{
    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    public SphereCollider sphereCollider;

    private string npc = "NPC1";
    private void OnDisable()
    {
        EventManager.Unsubscribe(GameEventType.NPCKill, StartNPCKill);
    }
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.NPCKill, StartNPCKill);
    }
    protected override void Start()
    {
        base.Start();
        select.SetActive(false);
        chat = GetComponent<NPCChatTest>();
        agent = GetComponent<NavMeshAgent>();
        NPCCollider = GetComponent<SphereCollider>();
    }
    protected override void OnTriggerEnter(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            ChangeState(State.Talk);
            chat.LoadNPCDialogue(npc, 0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            chat.LoadNPCDialogue(npc, 1);
        }
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue("NULL", 0);
        }
    }
    private void StartNPCKill()
    {
        Debug.Log("NPCKill호출 달려감");
        StartCoroutine(moutline.EventOutLine());
        ChangeState(State.Run);
        agent.SetDestination(player.position);
        sphereCollider.radius = 3.5f;
        StartCoroutine(DoorGuardRun());
    }
    private IEnumerator DoorGuardRun()
    {
        yield return new WaitForSeconds(2f);
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            yield return null;
        }
        Debug.Log("NPC가 도착했습니다!");
        StartCoroutine(TalkView());
        animator.SetTrigger("Talk");
        chat.LoadNPCDialogue(npc, 1);
        yield return new WaitForSeconds(2f);
        EventManager.Trigger(GameEventType.GameOver);
    }
    
}
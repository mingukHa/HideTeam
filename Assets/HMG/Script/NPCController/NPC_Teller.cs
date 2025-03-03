using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;


public class NPC_Teller : NPCFSM
{
    public GameObject npcchatbox; //NPC의 메인 채팅 최상위
    private string npc = "NPC6";

    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();
        agent = GetComponent<NavMeshAgent>();
    }
    
    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        if (!isDead && other.CompareTag("Player"))
        {
            chat.LoadNPCDialogue(npc, 0);
        }
    }

    private void ReturnOldMan()
    {
        if (!isDead)
        {
            animator.SetTrigger("Idel");
        }
    }
    
}


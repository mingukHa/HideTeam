using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class NPCRichMan : MonoBehaviour
{
    public NPCIdentifier script1;
    public NamedNPC script2;
    public NPCStateMachine script3;
    public RoutineInvoker script4;
    public NamedController script5;
    public NavMeshAgent script6;
    public SphereCollider NPCCollider;
    public Moutline moutline;
    public NavMeshAgent agent;
    public bool isDead = false;
    public bool isToilet = false;
    private Animator animator;
    private Rigidbody[] rigidbodies;
    private bool isRagdollActivated = false;

    

    private void Start()
    {
        animator = GetComponent<Animator>();
        rigidbodies = GetComponentsInChildren<Rigidbody>();
        moutline = GetComponent<Moutline>();
        SetRagdollState(false);
    }

    private void OnTriggerStay(Collider other)
    {
        if (!isDead && other.CompareTag("Player"))
        {
            if (Input.GetKey(KeyCode.F))
            {
                KillNPC();
            }
        }
    }

    private void KillNPC() //죽었을 경우 모든 스크립트를 종료한다
    {
        isDead = true;
        animator.SetTrigger("Dead");
        
        script1.enabled = false;
        script2.enabled = false;
        script3.enabled = false;
        script4.enabled = false;
        script5.enabled = false;
        script6.enabled = false;
        if (moutline != null) moutline.enabled = false;

        if (isToilet == false) //화장실이 아닌 경우
        {
            EventManager.Trigger(EventManager.GameEventType.NPCKill);
        }
        else if (isToilet == true) //화장실인 경우
        {
            EventManager.Trigger(EventManager.GameEventType.RichKill);
        }
        
        StartCoroutine(ActivateRagdollAfterDeath());
    }
    private IEnumerator ActivateRagdollAfterDeath() //레그돌 발동 코루틴
    {
        yield return new WaitForSeconds(1f); 

        if (!isRagdollActivated)
        {
            ActivateRagdoll();
        }
    }
    private void ActivateRagdoll() //레그돌이 되면 실행되는 함수
    {
        isRagdollActivated = true;
        animator.enabled = false; 
        SetRagdollState(true); 
        
        foreach (Transform child in GetComponentsInChildren<Transform>())
       {
            child.gameObject.tag = "Ragdoll";
            child.gameObject.layer = 15;
       }
    }
    private void SetRagdollState(bool state) //레그돌이 되었을 때 키네메틱 해제
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = !state; 
        }
    }
}


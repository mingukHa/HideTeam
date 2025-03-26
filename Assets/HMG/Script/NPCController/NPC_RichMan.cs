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

    private void KillNPC() //�׾��� ��� ��� ��ũ��Ʈ�� �����Ѵ�
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

        if (isToilet == false) //ȭ����� �ƴ� ���
        {
            EventManager.Trigger(EventManager.GameEventType.NPCKill);
        }
        else if (isToilet == true) //ȭ����� ���
        {
            EventManager.Trigger(EventManager.GameEventType.RichKill);
        }
        
        StartCoroutine(ActivateRagdollAfterDeath());
    }
    private IEnumerator ActivateRagdollAfterDeath() //���׵� �ߵ� �ڷ�ƾ
    {
        yield return new WaitForSeconds(1f); 

        if (!isRagdollActivated)
        {
            ActivateRagdoll();
        }
    }
    private void ActivateRagdoll() //���׵��� �Ǹ� ����Ǵ� �Լ�
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
    private void SetRagdollState(bool state) //���׵��� �Ǿ��� �� Ű�׸�ƽ ����
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = !state; 
        }
    }
}


using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class NPCRichMan : MonoBehaviour
{
    private Animator animator;
    private Rigidbody[] rigidbodies;
    public bool isDead = false;
    public bool isToilet = false;
    private bool isRagdollActivated = false;

    public SphereCollider NPCCollider;
    public Moutline moutline;

    public NPCIdentifier script1;
    public NamedNPC script2;
    public NPCStateMachine script3;
    public RoutineInvoker script4;
    public NamedController script5;
    public NavMeshAgent script6;

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

    private void KillNPC()
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
        if (NPCCollider != null) NPCCollider.enabled = false;

        EventManager.Trigger(EventManager.GameEventType.RichKill);

        StartCoroutine(ActivateRagdollAfterDeath());
    }

    private IEnumerator ActivateRagdollAfterDeath()
    {
        yield return new WaitForSeconds(1f); 

        if (!isRagdollActivated)
        {
            ActivateRagdoll();
        }
    }

    private void ActivateRagdoll()
    {
        isRagdollActivated = true;
        animator.enabled = false; 
        SetRagdollState(true); // 물리 적용
        if (isToilet == false)
        {
            EventManager.Trigger(EventManager.GameEventType.GameOver);
        }
        foreach (Transform child in GetComponentsInChildren<Transform>())
       {
           child.gameObject.tag = "Ragdoll";
       }
    }

    private void SetRagdollState(bool state)
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = !state; 
        }
    }
}


using UnityEngine;
using System.Collections;

public class NPCRichMan : MonoBehaviour
{
    private Animator animator;
    private Rigidbody[] rigidbodies;
    public bool isDead = false;
    private bool isRagdollActivated = false;

    public SphereCollider NPCCollider;
    public Moutline moutline;

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


        if (moutline != null) moutline.enabled = false;
        if (NPCCollider != null) NPCCollider.enabled = false;

        EventManager.Trigger(EventManager.GameEventType.NPCKill);

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

         //foreach (Transform child in GetComponentsInChildren<Transform>())
        //{
        //    child.gameObject.tag = "Ragdoll";
        //}
    }

    private void SetRagdollState(bool state)
    {
        foreach (var rb in rigidbodies)
        {
            rb.isKinematic = !state; 
        }
    }
}


using System.Collections;
using UnityEngine;

public class NPCHideEvent : MonoBehaviour
{
    private Animator playerAnimator; 
    private bool isRagdollDisposing = false; 
    private readonly int disposeRagdollHash = Animator.StringToHash("Dispose Ragdoll"); 
    private Coroutine trashBinCoroutine;

    [SerializeField] private string gameevent; 

    private void OnTriggerEnter(Collider other) //�÷��̾� �ִϸ��̼����� ����
    {
        if (other.CompareTag("Player"))
        {
            playerAnimator = other.GetComponent<Animator>();

            if (trashBinCoroutine == null)
            {
                trashBinCoroutine = StartCoroutine(HideNPC());
            }
        }
    }

    private void OnTriggerExit(Collider other) //�ڷ�ƾ ����
    {
        if (other.CompareTag("Player"))
        {
            playerAnimator = null;
            isRagdollDisposing = false;

            if (trashBinCoroutine != null)
            {
                StopCoroutine(trashBinCoroutine);
                trashBinCoroutine = null;
            }
        }
    }

    private IEnumerator HideNPC() //enum �����̶� string���� �� �޾ƿɴϴ�. Ÿ�� ��ȯ �߰�
    {
        while (playerAnimator != null)
        {
            AnimatorStateInfo stateInfo = playerAnimator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.shortNameHash == disposeRagdollHash)
            {
                if (!isRagdollDisposing)
                {
                    isRagdollDisposing = true;

                    if (System.Enum.TryParse(gameevent, out EventManager.GameEventType eventType))
                    {
                        EventManager.Trigger(eventType);
                        Debug.Log($"{eventType}�� �ߵ���");
                    }
                    else
                    {
                        Debug.LogWarning($"�׵����� �̺�Ʈ ������ �ȵ�");
                    }
                }
            }
            else
            {
                isRagdollDisposing = false; 
            }
            yield return null;
        }
        trashBinCoroutine = null;
    }
}

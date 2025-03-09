using System.Collections;
using UnityEngine;

public class NPCHideEvent : MonoBehaviour
{
    private Animator playerAnimator; 
    private bool isRagdollDisposing = false; 
    private readonly int disposeRagdollHash = Animator.StringToHash("Dispose Ragdoll"); 
    private Coroutine trashBinCoroutine;

    [SerializeField] private string gameevent; 

    private void OnTriggerEnter(Collider other) //플레이어 애니메이션으로 감지
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

    private void OnTriggerExit(Collider other) //코루틴 해제
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

    private IEnumerator HideNPC() //enum 형식이라 string으로 못 받아옵니다. 타입 변환 추가
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
                        Debug.Log($"{eventType}이 발동됨");
                    }
                    else
                    {
                        Debug.LogWarning($"그따구로 이벤트 적으면 안됨");
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

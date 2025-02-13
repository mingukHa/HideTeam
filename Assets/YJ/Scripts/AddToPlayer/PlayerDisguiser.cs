using UnityEngine;
using UnityEngine.UI;

public class PlayerDisguiser : MonoBehaviour
{
    public GameObject defaultCharacter; // 기본 캐릭터
    public Avatar defaultAvatar; // 기본 아바타
    //public Transform defaultLeftHandIKTarget; // 기본 왼손 IK 타겟
    //public Transform defaultRightHandIKTarget; // 기본 오른손 IK 타겟
    public GameObject[] characterVariants; // NPC 이름과 같은 캐릭터 리스트

    private Animator anim;
    private GameObject currentCharacter; // 현재 활성화된 캐릭터 추적
    private RagdollGrabber ragdollGrabber;


    private void Start()
    {
        anim = GetComponent<Animator>(); // Animator 컴포넌트 가져오기
        ragdollGrabber = GetComponent<RagdollGrabber>(); // RagdollGrabbber 클래스 가져오기
        currentCharacter = defaultCharacter; // 초기 상태에서 기본 캐릭터가 활성화됨
    }

    public void ChangeAppearance(NPCIdentifier npc)
    {
        if (npc == null) return;

        // 1. 이전 캐릭터 비활성화
        if (currentCharacter != null)
        {
            currentCharacter.SetActive(false);
        }

        // 2. NPC 이름에 맞는 캐릭터 활성화
        GameObject newCharacter = FindCharacterByName(npc.nPCName);
        if (newCharacter != null)
        {
            newCharacter.SetActive(true);
            currentCharacter = newCharacter; // 활성화된 캐릭터를 저장
        }
        else
        {
            Debug.LogWarning($"{npc.nPCName}에 해당하는 캐릭터를 찾을 수 없습니다!");
            // 캐릭터를 찾지 못하면 기본 캐릭터 복구
            if (defaultCharacter != null)
            {
                defaultCharacter.SetActive(true);
                currentCharacter = defaultCharacter;
            }
        }

        // 3. PlayerHolder의 Avatar 변경
        if (anim != null && npc.nPCAvatar != null)
        {
            anim.avatar = npc.nPCAvatar;
        }
        else
        {
            Debug.LogWarning("Animator 또는 NPC Avatar가 설정되지 않았습니다.");
        }

        //// 4. Ragdoll Grabber의 Hand IK Target 활성화된 NPC로 교체
        //if (npc.nPCLeftHandIK != null && npc.nPCRightHandIK != null && newCharacter != null)
        //{
        //    Transform leftHand = FindDeepChild(newCharacter.transform, npc.nPCLeftHandIK);
        //    Transform rightHand = FindDeepChild(newCharacter.transform, npc.nPCRightHandIK);
        //    if (leftHand != null && rightHand != null)
        //    {
        //        ragdollGrabber.leftHandIKTarget = leftHand;
        //        ragdollGrabber.rightHandIKTarget = rightHand;
        //    }
        //    else
        //    {
        //        Debug.LogWarning($"{npc.nPCLeftHandIK} 오브젝트를 찾을 수 없습니다.");
        //        Debug.LogWarning($"{npc.nPCRightHandIK} 오브젝트를 찾을 수 없습니다.");
        //    }
        //}
    }

    public void ResetToDefaultCharacter()
    {
        if (currentCharacter != null)
        {
            currentCharacter.SetActive(false); // 현재 변장한 캐릭터 비활성화
        }

        if (defaultCharacter != null)
        {
            defaultCharacter.SetActive(true); // 기본 캐릭터 활성화
            currentCharacter = defaultCharacter;
        }

        if (anim != null)
        {
            anim.avatar = defaultAvatar; // 기본 Avatar로 변경
        }

        //ragdollGrabber.leftHandIKTarget = defaultLeftHandIKTarget;
        //ragdollGrabber.rightHandIKTarget = defaultRightHandIKTarget;
    }

    private GameObject FindCharacterByName(string npcName)
    {
        foreach (GameObject character in characterVariants)
        {
            if (character.name == npcName)
            {
                return character;
            }
        }
        return null;
    }

    //// 재귀적으로 자식 오브젝트 찾기 (오른손 탐색용)
    //private Transform FindDeepChild(Transform parent, string childName)
    //{
    //    foreach (Transform child in parent)
    //    {
    //        if (child.name == childName)
    //        {
    //            return child;
    //        }
    //        Transform found = FindDeepChild(child, childName);
    //        if (found != null)
    //        {
    //            return found;
    //        }
    //    }
    //    return null;
    //}
}

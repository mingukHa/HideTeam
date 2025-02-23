using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCFinalCutscene : MonoBehaviour
{
    public Transform destination; // NPC가 이동할 목표 위치 (게임 오브젝트 위치)
    public float sceneDuration = 3f; // 엔딩 컷씬 지속 시간 (설정 가능)
    public Animator npcAnimator; // NPC 애니메이터
    public DoorController doorController; // 문 컨트롤러 참조
    private NavMeshAgent agent;
    private void OnEnable()
    {
        EventManager.Subscribe(EventManager.GameEventType.Ending, ending);
    }
    private void OnDisable()
    {
        EventManager.Unsubscribe(EventManager.GameEventType.Ending, ending);
    }

    private void ending()
    {
        StartCoroutine(StartCutscene());
    }
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        
    }

    private IEnumerator StartCutscene()
    {
        // 1️⃣ NPC 이동 시작
        npcAnimator.SetTrigger("Walk"); // 달리는 애니메이션 실행
        agent.SetDestination(destination.position);

        // 2️⃣ 목표 지점 도착까지 대기
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

      
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        npcAnimator.SetTrigger("Talk"); // Idle 애니메이션 전환

        
        yield return new WaitForSeconds(1.5f);
        npcAnimator.SetTrigger("Walk"); // 버튼 누르는 애니메이션 실행

        
        yield return new WaitForSeconds(sceneDuration);

        
        doorController.OpenDoorBasedOnView(this.transform);

        Debug.Log("엔딩 컷씬 완료!");
    }
}

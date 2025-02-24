using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class NPCFinalCutscene : MonoBehaviour
{
    public Transform destination; // NPC가 이동할 목표 위치 (게임 오브젝트 위치)
    public Transform destination2;
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
        // 1️ NPC 이동 시작
        yield return new WaitForSeconds(1.5f);
        npcAnimator.SetTrigger("Walk"); 
        agent.SetDestination(destination.position);

        // 2️ 목표 지점 도착까지 대기
        while (agent.pathPending || agent.remainingDistance > 0.1f)
        {
            yield return null;
        }

      
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        npcAnimator.SetTrigger("Talk"); 

        
        yield return new WaitForSeconds(1.5f);
        agent.SetDestination(destination2.position);
        npcAnimator.SetTrigger("Walk"); 

        
        yield return new WaitForSeconds(sceneDuration);

        
        doorController.OpenDoorBasedOnView(this.transform);

        Debug.Log("엔딩 컷씬 완료!");
    }
}

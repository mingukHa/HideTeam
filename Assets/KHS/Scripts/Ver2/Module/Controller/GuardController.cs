using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(GuardNPC))]
public class GuardController : TNPCController
{
    [Header("순찰 설정")]
    public PatrolRoute patrolRoute;
    public float patrolSpeed = 2f;
    public float lookAroundDelay = 1f; // 회전 후 대기 시간
    private Animator Animator;
    private int currentWaypointIndex = 0;
    private bool isLookingAround = false;
    private NavMeshAgent agent;

    private void Awake()
    {
        Animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolRoute.waypoints[currentWaypointIndex]);
        agent.autoBraking = false;
        agent.speed = patrolSpeed;
    }   
    public override void Start()
    {
        base.Start();
        Animator.SetTrigger("Walk");
    }
    private void Update()
    {
        Patrol();
    }

    public bool Patrol()
    {
        if (!agent.pathPending && agent.remainingDistance < 0.1f && !isLookingAround)
        {
            StartCoroutine(LookAroundRoutine());
        }
        return false;
    }

    private void NextWaypoint()
    {
        Animator.SetTrigger("Walk");
        
        currentWaypointIndex = (currentWaypointIndex + 1) % patrolRoute.waypoints.Count;
        agent.SetDestination(patrolRoute.waypoints[currentWaypointIndex]);
    }

    public IEnumerator LookAroundRoutine()
    {
        Animator.SetTrigger("Look");
        isLookingAround = true;
        agent.isStopped = true; //  회전 중 이동 정지

        for (int i = 0; i < 2; i++) // 2번 회전
        {
            float randomYaw = Random.Range(-60f, 60f);
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomYaw, 0);

            float elapsedTime = 0f;
            Quaternion startRotation = transform.rotation;

            while (elapsedTime < 1f)
            {
                float t = elapsedTime / 1f; //  `t` 값을 0~1로 정규화
                transform.rotation = Quaternion.Slerp(startRotation, newRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = newRotation;
            yield return new WaitForSeconds(lookAroundDelay); // 일정 시간 대기
        }

        isLookingAround = false;
        agent.isStopped = false; //  회전 후 이동 재개
        NextWaypoint(); //  이제야 다음 웨이포인트로 이동
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Door"))
        {
            SoundManager.instance.SFXPlay("DoorSound");
            Animator.SetTrigger("DoorOpen");
        }
    }
}

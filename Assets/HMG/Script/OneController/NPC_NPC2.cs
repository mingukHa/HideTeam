using UnityEngine;
using System.Collections;
using Unity.VisualScripting;
using static EventManager;
using UnityEngine.AI;

public class NPC_NPC2 : NPCFSM
{
    [SerializeField] private GameObject select;
    private NPCChatTest chat;
    public GameObject npcchatbox;
    private string npc = "NPC2";
    
    private int currentWaypointIndex;
    private bool isLookingAround = false;
    private NavMeshAgent agent;
    public Transform carpos;
    private void OnEnable()
    {
        EventManager.Subscribe(GameEventType.Carkick, StartKar);    
    }
    private void StartKar()
    {
        Debug.Log("��ű �̺�Ʈ �޾Ƽ� �޷���");
        agent.SetDestination(carpos.transform.position);
    }
    protected override void Start()
    {
        base.Start();
        chat = GetComponent<NPCChatTest>();
        select.SetActive(false);
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(patrolRoute.waypoints[currentWaypointIndex]);
        agent.autoBraking = false;
    }

    protected override void Update()
    {
        base.Update();
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
    protected override void IdleBehavior()
    {
        base.IdleBehavior();
    }

    protected override void LookBehavior()
    {
        base.LookBehavior();
    }

    protected override void WalkBehavior()
    {
        base.WalkBehavior();
    }

    protected override void RunBehavior()
    {
        base.RunBehavior();
    }

    protected override void TalkBehavior()
    {
        base.TalkBehavior();
    }

    protected override void DeadBehavior()
    {
        base.DeadBehavior();
        npcchatbox.SetActive(false);
        chat.LoadNPCDialogue("NULL", 0);
    }
    public IEnumerator LookAroundRoutine()
    {
        animator.SetTrigger("Look");
        isLookingAround = true;
        agent.isStopped = true; //  ȸ�� �� �̵� ����

        for (int i = 0; i < 2; i++) // 2�� ȸ��
        {
            float randomYaw = Random.Range(-60f, 60f);
            Quaternion newRotation = Quaternion.Euler(0, transform.eulerAngles.y + randomYaw, 0);

            float elapsedTime = 0f;
            Quaternion startRotation = transform.rotation;

            while (elapsedTime < 1f)
            {
                float t = elapsedTime / 1f; //  `t` ���� 0~1�� ����ȭ
                transform.rotation = Quaternion.Slerp(startRotation, newRotation, t);
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            transform.rotation = newRotation;
            yield return new WaitForSeconds(1f); // ���� �ð� ���
        }

        isLookingAround = false;
        agent.isStopped = false; //  ȸ�� �� �̵� �簳
        NextWaypoint(); //  ������ ���� ��������Ʈ�� �̵�
    }
    private void NextWaypoint()
    {
        animator.SetTrigger("Walk");

        currentWaypointIndex = (currentWaypointIndex + 1) % patrolRoute.waypoints.Count;
        agent.SetDestination(patrolRoute.waypoints[currentWaypointIndex]);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (isDead == false)
        {
            if (other.CompareTag("Player"))
            {
                select.SetActive(true);
                ChangeState(State.Talk);
                chat.LoadNPCDialogue(npc, 0);
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {

                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {

                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ChangeState(State.Idle);
            select.SetActive(false);
            chat.LoadNPCDialogue("NULL", 0);
        }
    }
}

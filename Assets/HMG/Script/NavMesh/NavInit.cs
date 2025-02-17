using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class AgentInitializer : MonoBehaviour
{
    public NavMeshAgent agent;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        ResetAgent();
    }
    void Start()
    {

        if (agent == null)
        {
            Debug.LogError("NavMeshAgent�� �������� �ʽ��ϴ�!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent�� NavMesh ���� ���� �ʽ��ϴ�!");
            return;
        }
        
    }
    void ResetAgent()
    {
        if (agent != null)
        {
            agent.enabled = false;
            agent.enabled = true;
        }
    }
}

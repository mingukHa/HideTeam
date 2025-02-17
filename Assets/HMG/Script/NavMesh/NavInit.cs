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
            Debug.LogError("NavMeshAgent가 존재하지 않습니다!");
            return;
        }

        if (!agent.isOnNavMesh)
        {
            Debug.LogError("NavMeshAgent가 NavMesh 위에 있지 않습니다!");
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

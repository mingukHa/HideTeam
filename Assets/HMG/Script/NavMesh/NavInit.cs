using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using System.Collections;
public class AgentInitializer : MonoBehaviour
{
    private NavMeshAgent agent;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        StartCoroutine(ReenableAgent());
    }
    void ResetAgentState()
    {
        if (agent != null)
        {
            agent.enabled = false;
            StartCoroutine(ReenableAgent());
        }
    }

    IEnumerator ReenableAgent()
    {
        yield return new WaitForSeconds(0.1f);  // NavMesh ���� �ð� Ȯ��
        agent.enabled = true;
        Debug.Log("NavMeshAgent Ȱ��ȭ �Ϸ�");
    }
}

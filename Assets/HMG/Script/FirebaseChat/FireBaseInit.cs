using UnityEngine;

public class FireBaseInit : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("����");
        
    }
    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase �ʱ�ȭ ��...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase �ʱ�ȭ ����!");
            database = FirebaseDatabase.DefaultInstance.RootReference;

            if (database != null)
                Debug.Log("Firebase Database �ʱ�ȭ ����!");
            else
                Debug.LogError("Firebase Database �ʱ�ȭ ����!");
        }
        else
        {
            Debug.LogError($"Firebase �ʱ�ȭ ����: {dependencyTask.Result}");
            yield break;
        }
    }
}

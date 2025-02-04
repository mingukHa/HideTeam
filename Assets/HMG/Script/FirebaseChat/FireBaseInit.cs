using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Collections;

public class FireBaseInit : MonoBehaviour
{
    private DatabaseReference database;
    
    private void Start()
    {
        Debug.Log("Firebase �ʱ�ȭ ����...");
        StartCoroutine(InitializeFirebase());
    }

    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase ������ üũ ��...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase �ʱ�ȭ ����!");
            FirebaseApp app = FirebaseApp.DefaultInstance;
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

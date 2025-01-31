using UnityEngine;
using Firebase;
using Firebase.Database;
using System.Collections;

public class FireBaseInit : MonoBehaviour
{
    private DatabaseReference database;
    
    private void Start()
    {
        Debug.Log("Firebase 초기화 시작...");
        StartCoroutine(InitializeFirebase());
    }

    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase 의존성 체크 중...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase 초기화 성공!");
            FirebaseApp app = FirebaseApp.DefaultInstance;
            database = FirebaseDatabase.DefaultInstance.RootReference;

            if (database != null)
                Debug.Log("Firebase Database 초기화 성공!");
            else
                Debug.LogError("Firebase Database 초기화 실패!");
        }
        else
        {
            Debug.LogError($"Firebase 초기화 실패: {dependencyTask.Result}");
            yield break;
        }
    }
}

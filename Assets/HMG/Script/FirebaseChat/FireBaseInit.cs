using UnityEngine;

public class FireBaseInit : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("켜짐");
        
    }
    private IEnumerator InitializeFirebase()
    {
        Debug.Log("Firebase 초기화 중...");
        var dependencyTask = FirebaseApp.CheckAndFixDependenciesAsync();
        yield return new WaitUntil(() => dependencyTask.IsCompleted);

        if (dependencyTask.Result == DependencyStatus.Available)
        {
            Debug.Log("Firebase 초기화 성공!");
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

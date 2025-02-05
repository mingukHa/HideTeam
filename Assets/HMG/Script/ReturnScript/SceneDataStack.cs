using System.Collections.Generic;
using UnityEngine;
//스택 관리 데이터 클래스
public class SceneDataStack
{
    private Stack<SceneData> sceneStack = new Stack<SceneData>(); //스택 데이터 선언
    
    public void PushSceneData(SceneData data) //데이터 추가 
    {
        sceneStack.Push(data);
    }
    
    public SceneData PopSceneData() //마지막 데이터를 가져오며 비워주는 함수
    {
        if (sceneStack.Count > 0)
        {
            return sceneStack.Pop();
        }
        return null;
    }
    public SceneData PeekSceneData() //현재 스택의 가장 위 데이터를 조회
    {
        if(sceneStack.Count > 0)
        {
            return sceneStack.Peek();
        }
        return null;
    }
    public int GetSceneCount() //스택에 저장된 데이터 수
    {
        return sceneStack.Count;
    }
}

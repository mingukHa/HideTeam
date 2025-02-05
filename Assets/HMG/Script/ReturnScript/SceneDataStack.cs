using System.Collections.Generic;
using UnityEngine;
//���� ���� ������ Ŭ����
public class SceneDataStack
{
    private Stack<SceneData> sceneStack = new Stack<SceneData>(); //���� ������ ����
    
    public void PushSceneData(SceneData data) //������ �߰� 
    {
        sceneStack.Push(data);
    }
    
    public SceneData PopSceneData() //������ �����͸� �������� ����ִ� �Լ�
    {
        if (sceneStack.Count > 0)
        {
            return sceneStack.Pop();
        }
        return null;
    }
    public SceneData PeekSceneData() //���� ������ ���� �� �����͸� ��ȸ
    {
        if(sceneStack.Count > 0)
        {
            return sceneStack.Peek();
        }
        return null;
    }
    public int GetSceneCount() //���ÿ� ����� ������ ��
    {
        return sceneStack.Count;
    }
}

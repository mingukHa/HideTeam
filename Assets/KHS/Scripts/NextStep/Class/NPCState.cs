using System.Collections.Generic;

public abstract class NPCState
{
    protected TNPCController npc;
    protected List<ICommand> actions = new List<ICommand>();

    public NPCState(TNPCController npc)
    {
        this.npc = npc;
    }

    public virtual void Enter()
    {
        foreach (var action in actions)
            action.Execute();   // ���¿� �����ϸ� �ൿ ����
    }
    public virtual void Exit()
    {
        foreach (var action in actions)
            action.End();       // ���¿��� ������ �ൿ ����
    }

    public abstract void Update();
}

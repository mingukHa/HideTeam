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
            action.Execute();   // 상태에 진입하면 행동 실행
    }
    public virtual void Exit()
    {
        foreach (var action in actions)
            action.End();       // 상태에서 나가면 행동 종료
    }

    public abstract void Update();
}

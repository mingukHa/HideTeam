using System.Collections.Generic;
using UnityEngine;

public class RoutineInvoker : MonoBehaviour
{
    public NPCRoutine npcRoutine;
    private List<ICommand> routineCommands = new List<ICommand>();
    private int currentCommandIndex = 0;
    private bool routineFinished = false;

    private void Start()
    {
        if (npcRoutine != null)
        {
            LoadRoutine(npcRoutine);
        }
    }
    public void LoadRoutine(NPCRoutine routine)
    {
        routineCommands.Clear();
        foreach (var action in routine.actions)
        {
            switch (action.actionType)
            {
                case RoutineActionType.Move:
                    routineCommands.Add(new MoveToCommand(GetComponent<NPCController>(), action.targetPosition));
                    break;
                case RoutineActionType.Wait:
                    routineCommands.Add(new WaitCommand(GetComponent<NPCController>(), action.waitTime));
                    break;
            }
        }
        currentCommandIndex = 0;
    }

    public void SetRoutine(List<ICommand> commands)
    {
        routineCommands = commands;
        currentCommandIndex = 0;
    }

    public void ExcuteRoutine()
    {
        if (routineCommands.Count == 0)
            return;

        if(currentCommandIndex < routineCommands.Count)
        {
            ICommand command = routineCommands[currentCommandIndex];
            command.Execute();

            if(command.IsFinished())
            {
                command.End();
                ++currentCommandIndex;
            }
        }
        else
        {
            currentCommandIndex = 0;
            routineFinished = true;
        }
    }
    public bool RoutineEnd()
    {
        return routineFinished;
    }
}

using System.Collections.Generic;
using UnityEngine;

public class RoutineInvoker : MonoBehaviour
{
    public List<NPCRoutine> npcRoutines;
    public List<ICommand> routineCommands = new List<ICommand>();
    private int currentCommandIndex = 0;
    private bool routineFinished = false;
    public int curRoutineIdx = 0;

    private void Start()
    {
        curRoutineIdx = 0;

        if (npcRoutines.Count != 0)
        {
            LoadRoutine(npcRoutines[curRoutineIdx]);
        }
    }
    public void LoadRoutine(NPCRoutine routine)
    {
        routineCommands.Clear();
        routineFinished = false;
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
                case RoutineActionType.Run:
                    routineCommands.Add(new RunToCommand(GetComponent<NPCController>(), action.targetPosition));
                    break;
            }
        }
        currentCommandIndex = 0;
    }

    public void ExcuteRoutine()
    {
        if (routineFinished || routineCommands.Count == 0)
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
            routineFinished = true;
            //RoutineChange(curRoutineIdx);
            Debug.Log("루틴 종료 or 반복");
        }
    }
    public bool RoutineEnd()
    {
        return routineFinished;
    }
    public void RoutineChange(int nextIdx)
    {
        curRoutineIdx = nextIdx;
        LoadRoutine(npcRoutines[nextIdx]);
    }
}

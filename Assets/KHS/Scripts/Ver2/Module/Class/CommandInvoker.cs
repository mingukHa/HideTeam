using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour 
{
    private Queue<ICommand> commandQueue = new Queue<ICommand>();

    public void AddCommand(ICommand command)
    {
        Debug.Log($"Command �߰���: {command.GetType().Name}");
        commandQueue.Enqueue(command);
    }

    public void ExecuteCommands()
    {
        if (commandQueue.Count > 0)
        {
            ICommand command = commandQueue.Peek();
            Debug.Log($"Command �����: {command.GetType().Name}");
            command.Execute();
            if (command.IsFinished())
            {
                command.End();
                commandQueue.Dequeue();
            }
        }
    }
}

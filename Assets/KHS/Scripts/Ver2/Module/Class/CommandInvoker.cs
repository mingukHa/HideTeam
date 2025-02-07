using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour 
{
    private Queue<ICommand> commandQueue = new Queue<ICommand>();

    public void AddCommand(ICommand command)
    {
        commandQueue.Enqueue(command);
    }

    public void ExecuteCommands()
    {
        if (commandQueue.Count > 0)
        {
            ICommand command = commandQueue.Peek();
            command.Execute();
            if (command.IsFinished())
            {
                command.End();
                commandQueue.Dequeue();
            }
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandInvoker : MonoBehaviour
{
    ICommand moveCommand;
    ICommand shootCommand;

    public void SetMoveCommand(ICommand _moveCommand)
    {
        moveCommand = _moveCommand;
    }

    public void SetShootCommand(ICommand _shootCommand)
    {
        shootCommand = _shootCommand;
    }

    public void ExecuteMoveCommand()
    {
        moveCommand?.Execute();//If moveCommand is not null, then execute
    }
    public void ExecuteShootCommand()
    {
        shootCommand?.Execute();//If shootCommand is not null, then execute
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICommand
{
    void Do();

    //Patron memento
    // void Undo();
}

public interface IMemento : ICommand
{
    bool CanUndo { get; }
    float TimeToUndo { get; set; }
    void Undo();
}
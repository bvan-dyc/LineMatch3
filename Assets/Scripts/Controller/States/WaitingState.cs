using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingState : State
{
    protected BoardController board;
    protected InputController input;

    protected virtual void Awake()
    {
        board = GetComponent<BoardController>();
        input = InputController.Instance;
    }

    public override void Enter()
    {
        base.Enter();
        input.OnPress.AddListener(board.SelectTiles);
    }

    public override void Exit()
    {
        base.Exit();
        input.OnPress.RemoveListener(board.SelectTiles);
    }
}

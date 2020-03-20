using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VictoryController : MonoBehaviour
{
    [SerializeField] private bool limitedMoves = false;
    [SerializeField] private float maximumMoves = 20;
    [SerializeField] private bool isTimed = false;
    [SerializeField] private float maximumTime = 30;
    private float totalMoves;
    private float timer;

    private void Start()
    {
        BoardController.Instance.OnMatch.AddListener(AddMatchMove);
    }

    private void AddMatchMove(int matchAmount)
    {
        totalMoves += 1;
    }
}

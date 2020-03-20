using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField] private float scoreAddition = 100;
    [SerializeField] private float scoreMultiplier = 1.2f;
    private int score = 0;
    public int Score { get { return score; } }

    private void Start()
    {
        BoardController.Instance.OnMatch.AddListener(AddMatchScore);
    }

    public void AddMatchScore(int matchCount)
    {
        score += Mathf.RoundToInt(scoreAddition * matchCount * scoreMultiplier);
    }
}

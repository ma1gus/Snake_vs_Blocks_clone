using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameOverView : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Snake _snake;

    private void OnEnable()
    {
        _snake.SnakeSegmentsIsOver += OnGameOverMessage;
    }

    private void OnDisable()
    {
        _snake.SnakeSegmentsIsOver -= OnGameOverMessage;
    }

    private void OnGameOverMessage()
    {
        _text.SetText("Game Over");
    }
}

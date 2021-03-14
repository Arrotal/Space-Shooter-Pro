using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverContainer, _GameUIContainer;
    [SerializeField] private Text _scoreText, _gameOverScore;
    [SerializeField] private Sprite[] _lives;
    [SerializeField] private Image _livesVisual;
    private int _scoreValue;
    void Start()
    {
        _GameUIContainer.SetActive(true);
        _scoreText.text = _scoreValue.ToString();
    }

    public void CurrentLives(int livesLeft)
    {
        _livesVisual.sprite = _lives[livesLeft];
    }

    public void AddScore(int points)
    {
        _scoreValue = points;
        _scoreText.text = _scoreValue.ToString();
    }

    public void GameOver()
    {
        _GameUIContainer.SetActive(false);
        _gameOverScore.text = _scoreValue.ToString();
        _gameOverContainer.SetActive(true);
    }
}

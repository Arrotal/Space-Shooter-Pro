using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverContainer, _GameUIContainer,_BossInfo;
    [SerializeField] private Text _scoreText, _gameOverScore, _ammoAmount,_ammoMax;
    [SerializeField] private Sprite[] _lives;
    [SerializeField] private Image _livesVisual;
    [SerializeField] private Slider _speedBoost, _bossHealthBar;
    [SerializeField] private Image _speedColour;
    private bool _gameOver;
    private int _scoreValue;
    void Start()
    {
        _GameUIContainer.SetActive(true);
        _gameOverContainer.SetActive(false);
        _scoreText.text = _scoreValue.ToString();
        _BossInfo.SetActive(false);
        _gameOver = false;
    }

    public void CurrentLives(int livesLeft)
    {
        if (livesLeft < 0)
        {
            livesLeft = 0;
        }
        _livesVisual.sprite = _lives[livesLeft];
    }

    public void AddScore(int points)
    {
        _scoreValue = points;
        _scoreText.text = _scoreValue.ToString();
    }

    public void GameOver()
    {
        _gameOver = true;
        _GameUIContainer.SetActive(false);
        _gameOverScore.text = _scoreValue.ToString();
        StartCoroutine(GameOverFlickerRoutine());
    }
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameOverContainer.SetActive(!_gameOverContainer.activeSelf);
            yield return new WaitForSeconds(.4f);
        }
    }

    public bool IsGameOver()
    {
        return _gameOver;
    }

    public void AmmoCount(int ammoIncoming)
    {
        _ammoAmount.text = ammoIncoming.ToString();
    }
    public void AmmoMax(int AmmoMaxIncoming)
    {
        _ammoMax.text = AmmoMaxIncoming.ToString();
    }

    public void SpeedBoostDuration(float SpeedBoost)
    {
        _speedBoost.value = SpeedBoost;
    }

    public void SpeedBoostOnCooldown(bool isIt)
    {
        if (isIt)
        {
            _speedColour.color = Color.gray;
        }
        if (!isIt)
        {
            _speedColour.color = Color.green;
        }
    
    }
    public void BossHealth(int Damage)
    {
        _bossHealthBar.value = Damage;
    }
    public void BossStatus(bool isIt)
    {
        if (isIt)
        {
            _BossInfo.SetActive(true);
        }
        else
        {
            _BossInfo.SetActive(false);
        }
    }
}

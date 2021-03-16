using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject _gameOverContainer, _GameUIContainer;
    [SerializeField] private Text _scoreText, _gameOverScore, _ammoAmount;
    [SerializeField] private Sprite[] _lives;
    [SerializeField] private Image _livesVisual;
    [SerializeField] private Slider _speedBoost;
    [SerializeField] private Image _speedColour;
    private int _scoreValue;
    void Start()
    {
        _GameUIContainer.SetActive(true);
        _gameOverContainer.SetActive(false);
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


    public void AmmoCount(int ammoIncoming)
    {
        _ammoAmount.text = ammoIncoming.ToString();
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
}

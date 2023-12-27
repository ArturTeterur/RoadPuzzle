using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishTrigger : MonoBehaviour
{
    [SerializeField] GameObject _canvasFinish;
    [SerializeField] GameObject _firstStar;
    [SerializeField] GameObject _secondStar;
    [SerializeField] GameObject _thirdStar;
    [SerializeField] private SpawnBalls _spawn;
    [SerializeField] private GameObject _canvasGameOver;
    private int _totalNumberStars = 0;
    private int _currentAmountBalls = 0;
    private int _spawnCount;
    private int _currentPercent;
    private int _currentSpawnCount;

    private void Start()
    {
        if (PlayerPrefs.HasKey("_currentStars"))
        {
            _totalNumberStars = PlayerPrefs.GetInt("_currentStars");
        }
        _spawnCount = _spawn.SpawnCount;
        Debug.Log(_spawnCount);
        _currentSpawnCount = _spawn.SpawnCount;
    }

    private void Finish()
    {
        _currentPercent =  _currentAmountBalls / _spawnCount * 100;        
        if (_currentPercent < 30)
        {
            _canvasGameOver.SetActive(true);
        }
        if (_currentPercent >= 30)
        {
            _canvasFinish.SetActive(true);
            _firstStar.SetActive(true);
            ChargingStats();
        }
        if (_currentPercent >= 60)
        {
            _secondStar.SetActive(true);
            ChargingStats();

        }
        if (_currentPercent >= 90)
        {
            _thirdStar.SetActive(true);
            ChargingStats();

        }
        Time.timeScale = 0f;
    }

    public void CountBalls()
    {
        _currentAmountBalls++;
        Debug.Log(_currentAmountBalls);
        Debug.Log(_currentSpawnCount);
        if (_currentSpawnCount == _currentAmountBalls)
        {
            Finish();
        }
    }

    public void TakeAwayBall()
    {
        _currentSpawnCount--;
    }
    
    private void ChargingStats()
    {
        if (PlayerPrefs.HasKey("_currentStars"))
        {
            _totalNumberStars = PlayerPrefs.GetInt("_currentStars");
            PlayerPrefs.SetInt("_currentStars", _totalNumberStars++);
            PlayerPrefs.Save();
        }
        else
        {
            PlayerPrefs.SetInt("_CurrentStars", _totalNumberStars++);
            PlayerPrefs.Save();
        }
    }
}

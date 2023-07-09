using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class StartStateManager : MonoBehaviour, IGameStateManager {
    
    [Header("Countdown configurations")]
    [SerializeField]
    private GameObject _countdownOverlayLayer;
    [SerializeField]
    private TextMeshProUGUI _countdownText;

    [Header("State configurations")]
    [SerializeField]
    public Slider _heroHealthBar;
    [SerializeField]
    public Slider _enemiesHealthBar;
    [SerializeField]
    private List<GameObject> _monsters;
    [SerializeField]
    [Range(1, 5)]
    private int _countdownSeconds = 3;
    [SerializeField]
    [Range(1, 3)]
    private int _countdownSecondsBetweenChanges = 1;
    [SerializeField]
    [Range(0.5f, 10f)]
    private float _secondsToWaitForStateChange = 1.0f;

    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitStartStateEvent, SetUpStartState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitStartStateEvent, SetUpStartState);
    }

    public void FinishState() {
        void BroadcastFinishStateEvent() {
            Messenger.Broadcast(GameEvents.FinishGameStateEvent);
        }
        
        this.StartTaskAfter(_secondsToWaitForStateChange, BroadcastFinishStateEvent);
    }

    public void StartState() {

    }

    public void SetUpStartState(LevelConfiguration levelConfiguration) {
        void SetUpEnemiesConfiguration(LevelConfiguration levelConfiguration) {
            // Enemies HealthBar
            var enemiesHP = levelConfiguration.ActionEvents.Count(actionEvent => actionEvent == ActionStateEvents.DO_DAMAGE_TO_ENEMIES);
            var enemiesCount = levelConfiguration.ActionEvents.Count(actionEvent => actionEvent == ActionStateEvents.KILL_ENEMY);
            _enemiesHealthBar.maxValue = enemiesHP + enemiesCount;
            _enemiesHealthBar.value = _enemiesHealthBar.maxValue;

            // Active Enemies
            foreach (var monster in _monsters) {
                monster.SetActive(false);
            }
            
            var activeEnemies = 0;
            while (activeEnemies < enemiesCount) {
                var inactiveMonsters = _monsters.Where(monster => !monster.activeSelf).ToList();
                if (inactiveMonsters.Count > 0) {
                    var monsterToActivateIndex = UnityEngine.Random.Range(0, inactiveMonsters.Count);
                    inactiveMonsters[monsterToActivateIndex].SetActive(true);
                    activeEnemies++;
                }
            }
        }

        void SetUpHeroHealthBar(LevelConfiguration levelConfiguration) {
            _heroHealthBar.maxValue = levelConfiguration.HeroMaxHP;
            _heroHealthBar.value = _heroHealthBar.maxValue;
        }

        SetUpHeroHealthBar(levelConfiguration);
        SetUpEnemiesConfiguration(levelConfiguration);
        StartCountdown();
    }

    private void StartCountdown() {
        StartCoroutine(
            ChangeCountDownText(_countdownSeconds, _countdownSecondsBetweenChanges, FinishState)
        );
    }

    private IEnumerator ChangeCountDownText(int countdownSeconds, int countdownSecondsBetweenChanges, Action doAfterFinish) {
        _countdownOverlayLayer.SetActive(true);
        _countdownText.gameObject.SetActive(true);

        for (int counter = countdownSeconds; counter > 0; counter--) {
            _countdownText.text = counter.ToString();

            yield return new WaitForSeconds(countdownSecondsBetweenChanges);
        }

        _countdownText.text = GameTexts.ActionText;
        yield return new WaitForSeconds(countdownSecondsBetweenChanges);

        _countdownOverlayLayer.SetActive(false);
        _countdownText.gameObject.SetActive(false);

        doAfterFinish();
    }
}

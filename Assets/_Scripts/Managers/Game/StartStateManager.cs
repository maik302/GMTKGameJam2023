using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartStateManager : MonoBehaviour, IGameStateManager {
    
    [SerializeField]
    [Range(0.5f, 10f)]
    private float _secondsToWaitForStateChange = 1.0f;
    [SerializeField]
    public Slider _heroHealthBar;
    [SerializeField]
    public Slider _enemiesHealthBar;

    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitStartStateEvent, SetUpStartState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitStartStateEvent, SetUpStartState);
    }

    public void FinishState() {
        Messenger.Broadcast(GameEvents.FinishGameStateEvent);
    }

    public void StartState() {

    }

    public void SetUpStartState(LevelConfiguration levelConfiguration) {
        void SetUpEnemiesHealthBar(LevelConfiguration levelConfiguration) {
            var enemiesHP = levelConfiguration.ActionEvents.Count(actionEvent => actionEvent == ActionStateEvents.DO_DAMAGE_TO_ENEMIES);
            var enemiesCount = levelConfiguration.ActionEvents.Count(actionEvent => actionEvent == ActionStateEvents.KILL_ENEMY);
            _enemiesHealthBar.maxValue = enemiesHP + enemiesCount;
            _enemiesHealthBar.value = _enemiesHealthBar.maxValue;
        }

        void SetUpHeroHealthBar(LevelConfiguration levelConfiguration) {
            _heroHealthBar.maxValue = levelConfiguration.HeroMaxHP;
            _heroHealthBar.value = _heroHealthBar.maxValue;
        }

        SetUpHeroHealthBar(levelConfiguration);
        SetUpEnemiesHealthBar(levelConfiguration);
        
        this.StartTaskAfter(_secondsToWaitForStateChange, FinishState);
    }
}

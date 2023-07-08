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
    [SerializeField]
    private List<GameObject> _monsters;

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

        this.StartTaskAfter(_secondsToWaitForStateChange, FinishState);
    }
}

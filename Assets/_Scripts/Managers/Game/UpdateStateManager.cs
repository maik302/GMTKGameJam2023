using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;
using System.Linq;

public class UpdateStateManager : MonoBehaviour, IGameStateManager {

    [Header("Outer game elements")]
    [SerializeField]
    private GameObject _outerGameElementsContainer;
    [SerializeField]
    private TextMeshProUGUI _elapsedTimeCounter;
    [SerializeField]
    private OuterGameSpritesHolder _outerGameSpritesHolder;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private Slider _heroHealthBar;
    [SerializeField]
    private Slider _enemiesHealthBar;
    [SerializeField]
    private List<GameObject> _monsters;

    private List<ActionStateEvents> _actionEvents;
    private int _playerLives;
    private bool _isStateActive = false;
    private int _currentActionIndex = 0;
    private float _elapsedTimeInSeconds;

    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitUpdateStateEvent, SetUpUpdateState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitUpdateStateEvent, SetUpUpdateState);
    }

    private void OnAwake() {
        _elapsedTimeInSeconds = 0f;
    }

    private void Update() {
        if (_isStateActive) {
            _elapsedTimeInSeconds += Time.deltaTime;
            _elapsedTimeCounter.text = TimeSpan.FromSeconds(_elapsedTimeInSeconds).ToString(@"mm\:ss\:ff");
        }
    }

    public void FinishState() {
        ReturnToIdle();
        HideOverlayLayer();
        _isStateActive = false;
    }

    private void HideOverlayLayer() {
        _outerGameElementsContainer.SetActive(false);
    }

    public void StartState() {
        _isStateActive = true;
        _currentActionIndex = 0;
        BringOverlayLayerUpfront();
    }

    private void BringOverlayLayerUpfront() {
        _outerGameElementsContainer.SetActive(true);
    }

    private void SetUpUpdateState(LevelConfiguration levelConfiguration) {
        _actionEvents = levelConfiguration.ActionEvents;
        _playerLives = levelConfiguration.PlayerLives;
        StartState();
    }

    private void OnButtonPress() {
        if (_isStateActive) {
            if (Keyboard.current.zKey.wasPressedThisFrame) {
                Heal();
            } else if (Keyboard.current.xKey.wasPressedThisFrame) {
                TakeDamage();
            } else if (Keyboard.current.nKey.wasPressedThisFrame) {
                DoDamageToEnemies();
            } else if (Keyboard.current.mKey.wasPressedThisFrame) {
                KillEnemy();
            } else if (
                Keyboard.current.zKey.wasReleasedThisFrame ||
                Keyboard.current.xKey.wasReleasedThisFrame ||
                Keyboard.current.nKey.wasReleasedThisFrame ||
                Keyboard.current.mKey.wasReleasedThisFrame
            ) {
                ReturnToIdle();
            }
        }
    }

    private void DoDamageToEnemies() {
        MakePlayerPressRight();
        ConsumeActionEvent(ActionStateEvents.DO_DAMAGE_TO_ENEMIES, ReduceEnemiesHP);
    }

    private void TakeDamage() {
        MakePlayerPressLeft();
        ConsumeActionEvent(ActionStateEvents.TAKE_DAMAGE, ReduceHeroHP);
    }

    private void Heal() {
        MakePlayerPressLeft();
        ConsumeActionEvent(ActionStateEvents.HEAL, RestoreHeroHP);
    }

    private void KillEnemy() {
        void DeativateRandomEnemy() {
            var activeMonsters = _monsters.Where(monster => monster.activeSelf).ToList();
            if (activeMonsters.Count > 0) {
                var monsterToKillIndex = UnityEngine.Random.Range(0, activeMonsters.Count);
                activeMonsters[monsterToKillIndex].SetActive(false);
            }
        }

        MakePlayerPressRight();
        DeativateRandomEnemy();

        ConsumeActionEvent(ActionStateEvents.KILL_ENEMY, ReduceEnemiesHP);
    }

    private void ConsumeActionEvent(ActionStateEvents actionEvent, Action doAfterConsume) {
        if (actionEvent == _actionEvents[_currentActionIndex]) {
            Debug.Log("CORRECT ACTION!");
            _currentActionIndex++;
            doAfterConsume();
        } else {
            Debug.Log("INCORRECT ACTION!");
            _playerLives--;
        }

        if (_currentActionIndex >= _actionEvents.Count) {
            FinishUpdateStateOk();
        } else if (_playerLives <= 0) {
            FinishUpdateStateKo();
        }
    }

    private void FinishUpdateStateOk() {
        FinishState();
        Messenger.Broadcast(GameEvents.FinishUpdateStateOkEvent);
    }

    private void FinishUpdateStateKo() {
        FinishState();
        Messenger.Broadcast(GameEvents.FinishUpdateStateKoEvent);
    }

    private void ChangePlayerSprite(Sprite sprite) {
        var playerSpriteRenderer = _player.GetComponent<SpriteRenderer>();
        if (playerSpriteRenderer != null) {
            playerSpriteRenderer.sprite = sprite;
        }
    }

    private void MakePlayerPressRight() {
        ChangePlayerSprite(_outerGameSpritesHolder.PlayerPressingRight);
    }

    private void MakePlayerPressLeft() {
        ChangePlayerSprite(_outerGameSpritesHolder.PlayerPressingLeft);
    }

    private void ReturnToIdle() {
        ChangePlayerSprite(_outerGameSpritesHolder.PlayerIdle);
    }

    private void ReduceHeroHP() {
        _heroHealthBar.value--;
    }

    private void RestoreHeroHP() {
        _heroHealthBar.value = _heroHealthBar.maxValue;
    }

    private void ReduceEnemiesHP() {
        _enemiesHealthBar.value--;
    }
}

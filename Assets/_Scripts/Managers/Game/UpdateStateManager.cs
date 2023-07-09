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
    private GameObject _outerGameOverlayLayer;
    [SerializeField]
    private GameObject _player;
    [SerializeField]
    private GameObject _playerLivesContainer;
    [SerializeField]
    private List<GameObject> _playerLives;
    [SerializeField]
    private SpriteRenderer _actionSpriteIndicator;
    [SerializeField]
    private GameObject _actionsHelpUI;

    [Header("General state configurations")]
    [SerializeField]
    private TextMeshProUGUI _elapsedTimeCounter;
    [SerializeField]
    private OuterGameSpritesHolder _outerGameSpritesHolder;
    [SerializeField]
    private Slider _heroHealthBar;
    [SerializeField]
    private Slider _enemiesHealthBar;
    [SerializeField]
    private List<GameObject> _monsters;
    [SerializeField]
    [Range(0.5f, 2f)]
    private float _secondsBetweenStates = 1.2f;

    private List<ActionStateEvents> _actionEvents;
    private bool _isStateActive = false;
    private int _currentActionIndex = 0;
    private float _elapsedTimeInSeconds;
    private int _playerLivesCounter;
    private bool _stateIsFinishing;

    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitUpdateStateEvent, SetUpUpdateState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitUpdateStateEvent, SetUpUpdateState);
    }

    private void Awake() {
        _elapsedTimeInSeconds = 0f;
        _playerLivesCounter = 0;
    }

    private void Update() {
        if (_isStateActive && !_stateIsFinishing) {
            _elapsedTimeInSeconds += Time.deltaTime;
            _elapsedTimeCounter.text = TimeSpan.FromSeconds(_elapsedTimeInSeconds).ToString(@"mm\:ss\:ff");
        }
    }

    public void FinishState() {
        HideOuterGameElements();
        _isStateActive = false;
    }

    private void HideOuterGameElements() {
        _outerGameOverlayLayer.SetActive(false);
        _player.SetActive(false);
        _playerLivesContainer.SetActive(false);
        _elapsedTimeCounter.gameObject.SetActive(false);
        _actionSpriteIndicator.gameObject.SetActive(false);
        _actionsHelpUI.SetActive(false);
    }

    public void StartState() {
        _isStateActive = true;
        _currentActionIndex = 0;
        _stateIsFinishing = false;
        ReturnToIdle();
        ShowOuterGameElements();
        AudioUtils.PlayStartStateInitSFX();
    }

    private void ShowOuterGameElements() {
        _outerGameOverlayLayer.SetActive(true);
        _player.SetActive(true);
        _playerLivesContainer.SetActive(true);
        _elapsedTimeCounter.gameObject.SetActive(true);
        _actionsHelpUI.SetActive(true);
    }

    private void SetUpUpdateState(LevelConfiguration levelConfiguration) {
        _actionEvents = levelConfiguration.ActionEvents;
        _playerLivesCounter = _playerLives.Count;
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
        AudioUtils.PlayMonsterDamageSFX();
        ConsumeActionEvent(ActionStateEvents.DO_DAMAGE_TO_ENEMIES, ReduceEnemiesHP);
    }

    private void TakeDamage() {
        MakePlayerPressLeft();
        AudioUtils.PlayHeroDamageSFX();
        ConsumeActionEvent(ActionStateEvents.TAKE_DAMAGE, ReduceHeroHP);
    }

    private void Heal() {
        MakePlayerPressLeft();
        AudioUtils.PlayHeroHealSFX();
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
        AudioUtils.PlayMonsterDeathSFX();
        ConsumeActionEvent(ActionStateEvents.KILL_ENEMY, ReduceEnemiesHP);
    }

    private void ConsumeActionEvent(ActionStateEvents actionEvent, Action doAfterConsume) {
        ReportActionOnIndicator(actionEvent);
        if (actionEvent == _actionEvents[_currentActionIndex]) {
            _currentActionIndex++;
            ReportCorrectAction(actionEvent);
            doAfterConsume();
        } else {
            ReportIncorrectAction(actionEvent);
            RemoveAPlayerLife();
        }

        if (_currentActionIndex >= _actionEvents.Count) {
            _stateIsFinishing = true;
            this.StartTaskAfter(_secondsBetweenStates, FinishUpdateStateOk);
        } else if (_playerLivesCounter <= 0) {
            _stateIsFinishing = true;
            this.StartTaskAfter(_secondsBetweenStates, FinishUpdateStateKo);
        }
    }

    private void RemoveAPlayerLife() {
        _playerLivesCounter--;
        if (_playerLivesCounter > 0) {
            var activePlayerLives = _playerLives.Where(life => life.activeSelf).ToList();
            if (activePlayerLives.Count > 0) {
                activePlayerLives[activePlayerLives.Count - 1].SetActive(false);
            }
        }
    }

    private void FinishUpdateStateOk() {
        FinishState();
        Messenger<float>.Broadcast(GameEvents.FinishUpdateStateOkEvent, _elapsedTimeInSeconds);
    }

    private void FinishUpdateStateKo() {
        FinishState();
        Messenger<float>.Broadcast(GameEvents.FinishUpdateStateKoEvent, _elapsedTimeInSeconds);
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
        if (!_stateIsFinishing) {
            ChangePlayerSprite(_outerGameSpritesHolder.PlayerIdle);
            ResetOuterGameOverlayColor();
            _actionSpriteIndicator.gameObject.SetActive(false);
        }
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

    private void ReportCorrectAction(ActionStateEvents actionEvent) {
        ChangeOuterGameOverlayColor(new Color(0.32f, 0.66f, 0.16f, 0.63f));
    }

    private void ChangeOuterGameOverlayColor(Color color) {
        var outerGameOverlayLayerSpriteRenderer = _outerGameOverlayLayer.GetComponent<SpriteRenderer>();
        if (outerGameOverlayLayerSpriteRenderer != null) {
            outerGameOverlayLayerSpriteRenderer.color = color;
        }
    }

    private void ReportActionOnIndicator(ActionStateEvents actionEvent) {
        void ChangeActionIndicatorSprite(Sprite sprite) {
            _actionSpriteIndicator.gameObject.SetActive(true);
            _actionSpriteIndicator.sprite = sprite;
        }

        switch (actionEvent) {
            case ActionStateEvents.DO_DAMAGE_TO_ENEMIES:
                ChangeActionIndicatorSprite(_outerGameSpritesHolder.MonsterDamageAction);
                break;
            case ActionStateEvents.TAKE_DAMAGE:
                ChangeActionIndicatorSprite(_outerGameSpritesHolder.HeroDamageAction);
                break;
            case ActionStateEvents.HEAL:
                ChangeActionIndicatorSprite(_outerGameSpritesHolder.HeroHealAction);
                break;
            case ActionStateEvents.KILL_ENEMY:
                ChangeActionIndicatorSprite(_outerGameSpritesHolder.MonsterDeathAction);
                break;
            default:
                break;
        }
    }

    private void ReportIncorrectAction(ActionStateEvents actionEvent) {
        ChangeOuterGameOverlayColor(new Color(0.70f, 0.40f, 0.40f, 0.63f));
    }

    private void ResetOuterGameOverlayColor() {
        ChangeOuterGameOverlayColor(new Color(1f, 1f, 1f, 0.63f));
    }
}

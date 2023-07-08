using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

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
        _isStateActive = false;
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
                ConsumeActionEvent(ActionStateEvents.HEAL);
                MakePlayerPressLeft();
            } else if (Keyboard.current.xKey.wasPressedThisFrame) {
                ConsumeActionEvent(ActionStateEvents.TAKE_DAMAGE);
                MakePlayerPressLeft();
            } else if (Keyboard.current.nKey.wasPressedThisFrame) {
                ConsumeActionEvent(ActionStateEvents.DO_DAMAGE_TO_ENEMIES);
                MakePlayerPressRight();
            } else if (Keyboard.current.mKey.wasPressedThisFrame) {
                ConsumeActionEvent(ActionStateEvents.KILL_ENEMY);
                MakePlayerPressRight();
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

    private void ConsumeActionEvent(ActionStateEvents actionEvent) {
        if (actionEvent == _actionEvents[_currentActionIndex]) {
            Debug.Log("CORRECT ACTION!");
            _currentActionIndex++;
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
}

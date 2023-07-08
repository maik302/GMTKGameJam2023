using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UpdateStateManager : MonoBehaviour, IGameStateManager {

    private List<ActionStateEvents> _actionEvents;
    private int _playerLives;
    private bool _isStateActive = false;
    private int _currentActionIndex = 0;

    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitUpdateStateEvent, SetUpUpdateState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitUpdateStateEvent, SetUpUpdateState);
    }

    public void FinishState() {
        _isStateActive = false;
    }

    public void StartState() {
        _isStateActive = true;
        _currentActionIndex = 0;
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
            } else if (Keyboard.current.xKey.wasPressedThisFrame) {
                ConsumeActionEvent(ActionStateEvents.TAKE_DAMAGE);
            } else if (Keyboard.current.nKey.wasPressedThisFrame) {
                ConsumeActionEvent(ActionStateEvents.DO_DAMAGE_TO_ENEMIES);
            } else if (Keyboard.current.mKey.wasPressedThisFrame) {
                ConsumeActionEvent(ActionStateEvents.KILL_ENEMY);
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
        _isStateActive = false;
        Messenger.Broadcast(GameEvents.FinishUpdateStateOkEvent);
    }

    private void FinishUpdateStateKo() {
        _isStateActive = false;
        Messenger.Broadcast(GameEvents.FinishUpdateStateKoEvent);
    }
}

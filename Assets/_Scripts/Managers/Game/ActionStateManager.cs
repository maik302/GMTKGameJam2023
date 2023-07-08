using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class ActionStateManager : MonoBehaviour, IGameStateManager {

    [Header("Actions configurations")]
    [SerializeField]
    [Range(0.1f, 1.0f)]
    private float _timeBeforeReturningToIdle = 1.0f;

    [Header("Inner game elements")]
    [SerializeField]
    private InnerGameSpritesHolder _innerGameSpritesHolder;
    [SerializeField]
    private GameObject _hero;
    [SerializeField]
    private List<GameObject> _monsters;
    private List<ActionStateEvents> _actionEvents;


    private float _timeBetweenEachAction;

    private void OnEnable() {
        Messenger<LevelConfiguration>.AddListener(GameEvents.InitActionStateEvent, SetUpActionState);
    }

    private void OnDisable() {
        Messenger<LevelConfiguration>.RemoveListener(GameEvents.InitActionStateEvent, SetUpActionState);
    }

    private void OnAwake() {
        _actionEvents = new List<ActionStateEvents>();
    }
    
    public void FinishState() {
        ReturnToIdle();
        Messenger.Broadcast(GameEvents.FinishGameStateEvent);
    }

    public void StartState() {
        StartCoroutine(MakeActions(_actionEvents));
    }

    private void SetUpActionState(LevelConfiguration levelConfiguration) {
        _actionEvents = levelConfiguration.ActionEvents;
        _timeBetweenEachAction = levelConfiguration.TimeBetweenEachAction;
        StartState();
    }

    private IEnumerator MakeActions(List<ActionStateEvents> actionEvents) {
        void MakeAction(ActionStateEvents actionEvent) {
            Debug.Log($"{actionEvent}");
            switch (actionEvent) {
                case ActionStateEvents.DO_DAMAGE_TO_ENEMIES:
                    DoDamageToEnemies();
                    break;
                case ActionStateEvents.TAKE_DAMAGE:
                    TakeDamage();
                    break;
                case ActionStateEvents.HEAL:
                    Heal();
                    break;
                case ActionStateEvents.KILL_ENEMY:
                    KillEnemy();
                    break;
            }
        }

        foreach (var actionEvent in actionEvents) {
            yield return new WaitForSeconds(_timeBeforeReturningToIdle);
            MakeAction(actionEvent);
            yield return new WaitForSeconds(_timeBetweenEachAction);
            ReturnToIdle();
        }

        FinishState();
    }

    private void ChangeHeroSprite(Sprite sprite) {
        var heroSpriteRenderer = _hero.GetComponent<SpriteRenderer>();
        if (heroSpriteRenderer != null) {
            heroSpriteRenderer.sprite = sprite;
        }
    }

    private void ChangeMonstersSprites(Sprite sprite) {
        foreach (var monster in _monsters) {
            if (monster.activeSelf) {
                var monsterSpriteRenderer = monster.GetComponent<SpriteRenderer>();
                if (monsterSpriteRenderer != null) {
                    monsterSpriteRenderer.sprite = sprite;
                }
            }
        }
    }

    private void DoDamageToEnemies() {
        ChangeHeroSprite(_innerGameSpritesHolder.HeroAttack);
        ChangeMonstersSprites(_innerGameSpritesHolder.MonsterDamage);
    }

    private void TakeDamage() {
        ChangeHeroSprite(_innerGameSpritesHolder.HeroDamage);
        ChangeMonstersSprites(_innerGameSpritesHolder.MonsterAttack);
    }

    private void Heal() {
        ChangeHeroSprite(_innerGameSpritesHolder.HeroHeal);
        ChangeMonstersSprites(_innerGameSpritesHolder.MonsterIdle);
    }

    private void KillEnemy() {
        ChangeHeroSprite(_innerGameSpritesHolder.HeroAttack);
        ChangeMonstersSprites(_innerGameSpritesHolder.MonsterDamage);
    }

    private void ReturnToIdle() {
        ChangeHeroSprite(_innerGameSpritesHolder.HeroIdle);
        ChangeMonstersSprites(_innerGameSpritesHolder.MonsterIdle);
    }
}

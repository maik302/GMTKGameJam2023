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
    private SpritesHolder _spritesHolder;
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

    void ChangeHeroSprite(Sprite sprite) {
        var heroSpriteRenderer = _hero.GetComponent<SpriteRenderer>();
        if (heroSpriteRenderer != null) {
            heroSpriteRenderer.sprite = sprite;
        }
    }

    void ChangeMonstersSprites(Sprite sprite) {
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
        ChangeHeroSprite(_spritesHolder.HeroAttack);
        ChangeMonstersSprites(_spritesHolder.MonsterDamage);
    }

    private void TakeDamage() {
        ChangeHeroSprite(_spritesHolder.HeroDamage);
        ChangeMonstersSprites(_spritesHolder.MonsterAttack);
    }

    private void Heal() {
        ChangeHeroSprite(_spritesHolder.HeroHeal);
        ChangeMonstersSprites(_spritesHolder.MonsterIdle);
    }

    private void KillEnemy() {
        ChangeHeroSprite(_spritesHolder.HeroAttack);
        ChangeMonstersSprites(_spritesHolder.MonsterDamage);

        // TODO: This has to be done by the player in the UPDATE state
        // var activeMonsters = _monsters.Select(monster => monster.activeSelf).ToList();
        // if (activeMonsters.Count > 0) {
        //     var monsterToKillIndex = Random.Range(0, activeMonsters.Count);
        //     _monsters[monsterToKillIndex].SetActive(false);
        // }
    }

    private void ReturnToIdle() {
        ChangeHeroSprite(_spritesHolder.HeroIdle);
        ChangeMonstersSprites(_spritesHolder.MonsterIdle);
    }
}

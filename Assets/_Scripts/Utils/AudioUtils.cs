using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioUtils {

    public static void PlayBackgroundMusic() {
        AudioManager.Instance.Play(AudioTracksNames.BackgroundMusic);
    }

    public static void PlayOneAnnouncement() {
        AudioManager.Instance.Play(AudioTracksNames.OneAnnouncement);
    }

    public static void PlayTwoAnnouncement() {
        AudioManager.Instance.Play(AudioTracksNames.TwoAnnouncement);
    }

    public static void PlayThreeAnnouncement() {
        AudioManager.Instance.Play(AudioTracksNames.ThreeAnnouncement);
    }

    public static void PlayHeroDamageSFX() {
        AudioManager.Instance.Play(AudioTracksNames.HeroDamageSFX);
    }

    public static void PlayHeroHealSFX() {
        AudioManager.Instance.Play(AudioTracksNames.HeroHealSFX);
    }

    public static void PlayMonsterDamageSFX() {
        AudioManager.Instance.Play(AudioTracksNames.MonsterDamageSFX);
    }

    public static void PlayMonsterDeathSFX() {
        AudioManager.Instance.Play(AudioTracksNames.MonsterDeathSFX);
    }

    public static void PlayStartStateInitSFX() {
        AudioManager.Instance.Play(AudioTracksNames.StartStateInitSFX);
    }

    public static void PlayYouLoseAnnouncement() {
        AudioManager.Instance.Play(AudioTracksNames.YouLoseAnnouncement);
    }

    public static void PlayYouWinAnnouncement() {
        AudioManager.Instance.Play(AudioTracksNames.YouWinAnnouncement);
    }
}

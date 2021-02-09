using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip arrowMoveSound;
    public static AudioClip arrowSelectSound;
    public static AudioClip cutsceneMusic;
    public static AudioClip battleMusic;
    public static AudioClip victoryMusic;
    public static AudioClip lossMusic;
    static AudioSource audioSrc;

    // Start is called before the first frame update
    void Start()
    {
        arrowMoveSound = Resources.Load<AudioClip>("arrowMoveSound");
        arrowSelectSound = Resources.Load<AudioClip>("arrowSelectSound");
        cutsceneMusic = Resources.Load<AudioClip>("cutsceneTheme");
        battleMusic = Resources.Load<AudioClip>("battleMusic");
        victoryMusic = Resources.Load<AudioClip>("victoryMusic");
        lossMusic = Resources.Load<AudioClip>("lossMusic");
        audioSrc = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public static void playArrowMoveSound()
    {
        audioSrc.PlayOneShot(arrowMoveSound);
    }

    public static void playArrowSelectSound()
    {
        audioSrc.PlayOneShot(arrowSelectSound);
    }

    public static void playCutsceneMusic()
    {
        audioSrc.PlayOneShot(cutsceneMusic);
    }

    public static void playBattleMusic()
    {
        audioSrc.PlayOneShot(battleMusic);
    }

    public static void playVictoryMusic()
    {
        audioSrc.PlayOneShot(victoryMusic);
    }

    public static void playLossMusic()
    {
        audioSrc.PlayOneShot(lossMusic);
    }

}

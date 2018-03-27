using UnityEngine;

public class GlobalEffectSound : MonoBehaviour {
    public static GlobalEffectSound instance = null;
    public AudioClip clickSound, clickSound2, brickClearSound, missionClearSound;
    public AudioSource mySound, mySound2, brickClear, missionClear;
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }
    void Start()
    {
        mySound = GetComponent<AudioSource>();
        mySound2 = GetComponent<AudioSource>();
        brickClear = GetComponent<AudioSource>();
        missionClear = GetComponent<AudioSource>();
    }
    public void playSound()
    {
        mySound.PlayOneShot(clickSound);
    }
    public void playSound2()
    {
        mySound2.PlayOneShot(clickSound2);
    }
    public void playBrickClear()
    {
        brickClear.PlayOneShot(brickClearSound);
    }
    public void playMissionClear()
    {
        missionClear.PlayOneShot(missionClearSound);
    }
}

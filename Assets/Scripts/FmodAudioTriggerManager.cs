using UnityEngine;
using FMODUnity;

public class FmodAudioTriggerManager : Singleton<FmodAudioTriggerManager>{

   
    [EventRef]
    public string negativeSound;
	FMOD.Studio.EventInstance negativeSoundInstance;

    [EventRef]
    public string cashSound;
	FMOD.Studio.EventInstance cashSoundInstance;


    [EventRef]
    public string bombSound;
	FMOD.Studio.EventInstance bombSoundInstance;



    public override void Awake(){
        base.Awake();
        if( negativeSound   == null ){
            Debug.LogError("Somebody forgot to link sounds to the fmod audio trigger manager. *disappointed look* ");
        }

        InitializeSounds();
        
    }

    void InitializeSounds(){
        negativeSoundInstance = RuntimeManager.CreateInstance(negativeSound);
        negativeSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));
    
        cashSoundInstance = RuntimeManager.CreateInstance(cashSound);
        cashSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

    }

    public void PlayCashSound(){
        cashSoundInstance.start();
    }

    public void PlayNegativeSound(){
        negativeSoundInstance.start();   
    }

    public void PlayBombSound(){
        bombSoundInstance.start();
    }

   
}


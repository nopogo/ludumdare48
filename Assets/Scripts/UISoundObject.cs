using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class UISoundObject : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler{

    [EventRef]
    public string hoverSound;
	FMOD.Studio.EventInstance hoverSoundInstance;

    [EventRef]
    public string clickSound;
	FMOD.Studio.EventInstance clickSoundInstance;


    void Awake(){

        hoverSoundInstance = RuntimeManager.CreateInstance(hoverSound);
        hoverSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

        clickSoundInstance = RuntimeManager.CreateInstance(clickSound);
        clickSoundInstance.set3DAttributes(RuntimeUtils.To3DAttributes(gameObject));

    }

	public void OnPointerClick(PointerEventData eventData) {
		clickSoundInstance.start();
	}

	public void OnPointerEnter(PointerEventData eventData) {
		hoverSoundInstance.start();
	}
}

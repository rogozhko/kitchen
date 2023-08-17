using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { get; private set; }

    [SerializeField] private AudioClipRefsSO audioClipRefsSO;

    private void Awake() {
        Instance = this;
    }

    private void Start() {
        DeliveryManager.Instance.OnRecipeSuccess += DeliveryManager_OnOnRecipeSuccess;
        DeliveryManager.Instance.OnRecipeFailed += DeliveryManager_OnOnRecipeFailed;
        CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
        Player.Instance.OnPickedSomething += Player_OnPickedSomething;
        BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
        TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
    }

    private void TrashCounter_OnAnyObjectTrashed(object sender, EventArgs e) {
        TrashCounter trashCounter = sender as TrashCounter;
        PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
    }

    private void BaseCounter_OnAnyObjectPlacedHere(object sender, EventArgs e) {
        BaseCounter baseCounter = sender as BaseCounter;
        PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
    }

    private void Player_OnPickedSomething(object sender, EventArgs e) {
        Player player = sender as Player;
        PlaySound(audioClipRefsSO.objectPickup, player.transform.position);
    }


    private void CuttingCounter_OnAnyCut(object sender, EventArgs e) {
        CuttingCounter cuttingCounter = sender as CuttingCounter;
        PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
    }

    private void DeliveryManager_OnOnRecipeFailed(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliveryFail, deliveryCounter.transform.position);   
    }

    private void DeliveryManager_OnOnRecipeSuccess(object sender, EventArgs e) {
        DeliveryCounter deliveryCounter = DeliveryCounter.Instance;
        PlaySound(audioClipRefsSO.deliverySuccess,  deliveryCounter.transform.position); 
    }

    // PlaySound Methods
    private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volume = 1f) {
        PlaySound(audioClipArray[Random.Range(0,audioClipArray.Length)], position, volume);
    }
    
    private void PlaySound(AudioClip audioClip, Vector3 position, float volume = 1f) {
        AudioSource.PlayClipAtPoint(audioClip, position, volume);
    }

    public void PlayFootStepSound(Vector3 position, float volume) {
        PlaySound(audioClipRefsSO.footStep, position, volume);
    }
    
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class HidingE : MonoBehaviour, IInteractable
{
    [SerializeField] private string _prompt;
    public string InteractionPrompt => _prompt;

    public AudioClip hideAudio;

    public bool Interact(Interactor interactor){
        // Play sound effect if assigned
        if (hideAudio != null){
            AudioSource.PlayClipAtPoint(hideAudio, transform.position);
        }

        //if e is pressed +  not hiding
            //delete player
        // else (already hiding)
            //respawn in past position

        return true;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    //    inv.AddFlowerAmount(1);
    //    Debug.Log("Flower Amount" + inv.flowerAmount);

    //    // Destroy the GameObject
    //    Destroy(gameObject);

}

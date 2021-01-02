using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX_Doors : MonoBehaviour
{
    public Object objectReferenceParameter;
    FMODUnity.StudioEventEmitter openSound;
    FMODUnity.StudioEventEmitter closeSound;
    FMODUnity.StudioEventEmitter lockedSound;
    // Start is called before the first frame update
    void Start()
    {
        openSound = GameObject.Find("Open Door Sound").GetComponent<FMODUnity.StudioEventEmitter>();
        closeSound = GameObject.Find("Close Door Sound").GetComponent<FMODUnity.StudioEventEmitter>();
        lockedSound = GameObject.Find("Door Locked Sound").GetComponent<FMODUnity.StudioEventEmitter>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OpenDoorEventHandler(AnimationEvent animationEvent)
    {
        openSound.Play();
    }

    public void CloseDoorEventHandler(AnimationEvent animationEvent)
    {
        closeSound.Play();
    }

    public void LockedDoorEventHandler(AnimationEvent animationEvent)
    {
        lockedSound.Play();
    }








}

using UnityEngine;

public class Sound : MonoBehaviour
{
    public static Sound Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            // DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this);
        }
    }

    [Header("Volume")]
    [Range(0f, 1f)] public float volMax = 1f;
    [Range(0f, 1f)] public float volNormal = .75f;
    [Range(0f, 1f)] public float volMid = .5f;

    [Header("Audio Clips")]
    public AudioClip botonPlay;
    public AudioClip colisionCajas;
    public AudioClip respuestaCorrecta;
    public AudioClip salto;

    [Header("Audio Source")]
    public AudioSource asMusicaDelMenu;
    public AudioSource asMusicaDelJuego;
    public AudioSource asFinDelJuego;

    public void PlayBotonPlay()
    {
        SoundManager.PlayUISound(botonPlay, volMid);
    }
    public void PlayColisionCajas()
    {
        SoundManager.PlayUISound(colisionCajas, volMid);
    }
    public void PlayRespuestaCorrecta()
    {
        SoundManager.PlayUISound(respuestaCorrecta, volMid);
    }
    public void PlaySalto()
    {
        SoundManager.PlayUISound(salto, volMid);
    }

}
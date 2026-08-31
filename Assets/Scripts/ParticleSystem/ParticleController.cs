using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Purchasing.Extension;

public class ParticleController : MonoBehaviour
{
    public ParticleSystem confettiEffect;

    private void Start()
    {
        confettiEffect.Stop();
    }
    public void PlayPlayerWinEffect()
    {
        confettiEffect.Play();
    }
}

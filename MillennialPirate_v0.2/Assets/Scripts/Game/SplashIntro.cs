﻿using UnityEngine;

public class SplashIntro : MonoBehaviour {

    private Renderer        r     = null;
    private MovieTexture    movie = null;


    private void Start()
    {
        r       = GetComponent<Renderer>();
        movie   = (MovieTexture)r.material.mainTexture;
        movie.Play();
    }

    private void Update()
    {
        if (!movie.isPlaying)
        {
            Game.Inst.levelSelect = SetManager.OpenSet<LevelSelectionSet>((lss) => Game.Inst.WantsToBeInWaitState = true);
            Destroy(gameObject);
        }
    }

}

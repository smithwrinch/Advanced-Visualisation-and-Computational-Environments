﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeOutCamera : MonoBehaviour
{
    public AnimationCurve FadeCurve = new AnimationCurve(new Keyframe(1, 0), new Keyframe(0.6f, 0.7f, -1.8f, -1.2f), new Keyframe(0, 1));
    
    private float _alpha = 1;
    private Texture2D _texture;
    private bool _done;
    private float _time;

    private void Start() {
        _done = true;    
    }

    public void Reset()
    {
        _done = false;
        _alpha = 1;
        _time = 0;
    }

    [RuntimeInitializeOnLoadMethod]
    public void RedoFade()
    {
        Reset();
    }

    public void OnGUI()
    {  
        if(OceanSceneManager.instance.levelEnd()){
            _done = false;
        }
        if (_done) return;
        if (_texture == null) _texture = new Texture2D(1, 1);

        _texture.SetPixel(0, 0, new Color(0, 0, 0, _alpha));
        _texture.Apply();

        _time += Time.deltaTime/10f;
        _alpha = FadeCurve.Evaluate(_time);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _texture);

        if (_alpha <= 0) _done = true;
    }
}
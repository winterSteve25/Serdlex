﻿using UnityEngine;
using AudioType = Utils.AudioType;

namespace Game
{
    public class GameOverScreen : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup group;
        [SerializeField] protected AudioType sound;

        public virtual void GameOver()
        {
            LeanTween.value(gameObject, f => group.alpha = f, 0, 1, 0.5f);
            AudioManager.Instance.Play(sound);
        }
    }
}
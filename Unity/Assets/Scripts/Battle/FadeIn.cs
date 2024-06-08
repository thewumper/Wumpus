using System;
using UnityEngine;

namespace WumpusUnity.Battle
{
    public class FadeIn : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer renderer;
        [SerializeField] private Color lossColor;
        [SerializeField] private Color winColor;
        [SerializeField] private float fadeTime;
        private float fadePoint = 0f;
        public bool FadeComplete
        {
            get => fadePoint >= fadeTime;
        }

        private void Awake()
        {
            renderer.color = new Color(0f, 0f, 0f, 0f);
        }

        private void Update()
        {
            if (BattlePlayerController.GameEnded)
            {
                fadePoint += Time.deltaTime;
                float alpha = fadePoint / fadeTime;

                if (BattlePlayerController.Won == false)
                {
                    // Loss
                    renderer.color = new Color(lossColor.r, lossColor.g, lossColor.b, alpha);
                }
                if (BattlePlayerController.Won == true)
                {
                    // Win
                    renderer.color = new Color(winColor.r, winColor.g, winColor.b, alpha);
                }
            }
        }
    }
}
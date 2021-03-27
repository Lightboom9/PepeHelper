using System;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

namespace PepeHelper
{
    [RequireComponent(typeof(Image))]
    public class ImageSpriteAnimator : MonoBehaviour
    {
        public Sprite[] Sprites;
        public float FullCycleTime;
        public bool Animate = true;

        private Image _image;
        private int _nextIndex;
        private float _waitTime;

        private void Awake()
        {
            if (Sprites.Length < 2) throw new Exception();

            _image = GetComponent<Image>();
            _waitTime = (float)FullCycleTime / Sprites.Length;

            StartCoroutine(Animator());
        }

        private IEnumerator Animator()
        {
            while (true)
            {
                if (Animate)
                {
                    _nextIndex++;
                    if (_nextIndex == Sprites.Length) _nextIndex = 0;

                    _image.sprite = Sprites[_nextIndex];
                }

                yield return new WaitForSecondsRealtime(_waitTime);
            }
        }
    }
}
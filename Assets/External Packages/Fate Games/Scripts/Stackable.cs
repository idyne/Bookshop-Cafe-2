using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FateGames
{
    public class Stackable : MonoBehaviour
    {
        [SerializeField] private string stackableTag;
        [SerializeField] private float identicalMargin, differentMargin;
        private Transform _transform;
        public Tween WaveScaleTween = null;
        public string StackableTag { get => stackableTag; }
        public float IdenticalMargin { get => identicalMargin; }
        public float DifferentMargin { get => differentMargin; }
        public Transform Transform { get => _transform; }

        private void Awake()
        {
            _transform = transform;
        }

    }

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;

namespace Klonk
{
    public class Rotator : MonoBehaviour
    {
        private Vector3 _originalPosition;

        private void Awake()
        {
            _originalPosition = transform.position;
        }

        // Update is called once per frame
        void Update()
        {
            Vector3 position = _originalPosition;
            position.y += Mathf.Sin(Time.time);
            transform.position = position;
            transform.Rotate(Vector3.up, Time.deltaTime * 45);
        }
    }
}

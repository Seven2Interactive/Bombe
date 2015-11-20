//
// Flambe - Rapid game development
// https://github.com/aduros/flambe/blob/master/LICENSE.txt
using System;
using UnityEngine;
using UnityEngine.UI;

namespace Bombe
{
    /**
     * A component that uses its gameObject's Text UI component to display an FPS log.
     */
    class FpsDisplay : MonoBehaviour
    {

        private int _fpsFrames;
        private float _fpsTime;

        private Text _txtDisplay;

        /* ---------------------------------------------------------------------------------------- */
        
        public void Awake()
        {
            Reset();
            _txtDisplay = gameObject.GetComponent<Text>();
        }

        /* ---------------------------------------------------------------------------------------- */
        
        public void Update()
        {
            float dt = Time.deltaTime;

            ++_fpsFrames;
            _fpsTime += dt;
            if (_fpsTime > 1)
            {
                var fps = _fpsFrames / _fpsTime;
                var text = "FPS: " + Mathf.FloorToInt(fps * 100) / 100;

                // Use our owner's Text if available, otherwise just log it
                if (_txtDisplay != null)
                {
                    _txtDisplay.text = text;
                }
                else
                {
                    Debug.Log(text);
                }

                Reset();
            }
        }

        /* ---------------------------------------------------------------------------------------- */
        
        private void Reset()
        {
            _fpsTime = _fpsFrames = 0;
        }

        /* ---------------------------------------------------------------------------------------- */
        
    }
}


using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRStandardAssets.Utils;
using UnityEngine.VR;

namespace VRStandardAssets.Menu
{
    // This script is for loading scenes from the main menu.
    // Each 'button' will be a rendering showing the scene
    // that will be loaded and use the SelectionRadial.
    public class WarpPosition : MonoBehaviour
    {
        public event Action<WarpPosition> OnButtonSelected;                   // This event is triggered when the selection of the button has finished.


        //[SerializeField]
        //private string m_SceneToLoad;                      // The name of the scene to load.
        [SerializeField]
        private VRCameraFade m_CameraFade;                 // This fades the scene out when a new scene is about to be loaded.
        [SerializeField]
        private SelectionRadial m_SelectionRadial;         // This controls when the selection is complete.
        [SerializeField]
        private VRInteractiveItem m_InteractiveItem;       // The interactive item for where the user should click to load the level.
        public Camera cam;
        public GameObject top_cam;

        private bool m_GazeOver;                                            // Whether the user is looking at the VRInteractiveItem currently.


        private void OnEnable()
        {
            m_InteractiveItem.OnOver += HandleOver;
            m_InteractiveItem.OnOut += HandleOut;
            m_SelectionRadial.OnSelectionComplete += HandleSelectionComplete;
        }


        private void OnDisable()
        {
            m_InteractiveItem.OnOver -= HandleOver;
            m_InteractiveItem.OnOut -= HandleOut;
            m_SelectionRadial.OnSelectionComplete -= HandleSelectionComplete;
        }


        private void HandleOver()
        {
            // When the user looks at the rendering of the scene, show the radial.
            m_SelectionRadial.Show();

            m_GazeOver = true;
        }


        private void HandleOut()
        {
            // When the user looks away from the rendering of the scene, hide the radial.
            m_SelectionRadial.Hide();

            m_GazeOver = false;
        }


        private void HandleSelectionComplete()
        {
            // If the user is looking at the rendering of the scene when the radial's selection finishes, activate the button.
            if (m_GazeOver)
                StartCoroutine(ActivateButton());
        }


        private IEnumerator ActivateButton()
        {
            // If the camera is already fading, ignore.
            //if (m_CameraFade.IsFading)
            //    yield break;

            // If anything is subscribed to the OnButtonSelected event, call it.
            if (OnButtonSelected != null)
                OnButtonSelected(this);

            // Wait for the camera to fade out.
            //yield return StartCoroutine(m_CameraFade.BeginFadeOut(true));

            // Load the level.
            //SceneManager.LoadScene(m_SceneToLoad, LoadSceneMode.Single);
            Vector3 pos = top_cam.transform.position;
            pos.x = pos.x + (transform.position.x - pos.x) * 0.6f;
            pos.z = pos.z + (transform.position.z - pos.z) * 0.6f;

            top_cam.transform.position = pos;
            
            yield return null;
        }

        void Update()
        {
            float dx = Input.GetAxis("Horizontal");
            float dy = Input.GetAxis("Vertical");

            Vector3 pos = top_cam.transform.position;
            pos.x = Mathf.Clamp(pos.x + dx * 0.01f, -100.0f, 100.0f); ;
            pos.y = Mathf.Clamp(pos.y + dy * 0.01f, 1.6f, 50.0f); ;
            top_cam.transform.position = pos;
            Debug.Log(dy);
        }
    }
}
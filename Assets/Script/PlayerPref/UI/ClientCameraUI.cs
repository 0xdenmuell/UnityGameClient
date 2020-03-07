using UnityEngine;

namespace Assets.Script.Player
{
    class ClientCameraUI : MonoBehaviour
    {
        public static ClientCameraUI instancePlayerUI;
        public Camera clientCamera;
        public PlayerMovement playerMovement;
        public PauseMenu_UI pauseMenu_UI;
        public CameraController cameraController;
        public Camera WeaponCam;
        public DeathMenu__UI Deathmenu;
        public Camera DeathCam;


        private void Awake()
        {
            instancePlayerUI = this;
        }

        public void enableClientCamera()
        {
            clientCamera.enabled = true;
            playerMovement.enabled = true;
            pauseMenu_UI.enabled = true;
            cameraController.enabled = true;
            WeaponCam.enabled = true;
        }

        public void switchToTeamSites(bool active)
        {

            Cursor.visible = active;

            clientCamera.enabled = !active;
            pauseMenu_UI.enabled = !active;
            cameraController.enabled = !active;
            WeaponCam.enabled = !active;
            StartMenuUI.instanceUI.RespawnAndGetTeamsites();
        }
    }
}
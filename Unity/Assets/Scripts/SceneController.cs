using UnityEngine;
using UnityEngine.SceneManagement;
using WumpusCore.Controller;

namespace WumpusUnity
{
    public class SceneController
    {
        /// <summary>
        /// The current Scene.
        /// </summary>
        private Scene currentScene;
        
        /// <summary>
        /// The global Controller object.
        /// </summary>
        private Controller controller;
        
        /// <summary>
        /// The Main Menu Scene.
        /// </summary>
        private Scene mainMenu;
        /// <summary>
        /// The Main Scene.
        /// </summary>
        private Scene main;
        
        /// <summary>
        /// The internal reference to the global SceneController.
        /// </summary>
        internal static SceneController controllerReference;
        
        public static SceneController GlobalSceneController
        {
            get
            {
                if (controllerReference==null)
                {
                    controllerReference = new SceneController();
                }

                return controllerReference;
            }
        }

        private SceneController()
        {
            mainMenu = SceneManager.GetSceneByName("Main Menu");
            main = SceneManager.GetSceneByName("Main");

            controller = Controller.GlobalController;
        }
        
        /// <summary>
        /// Sets the current Scene in Unity.
        /// </summary>
        /// <param name="scene">The new Scene.</param>
        public void SetScene(Scene scene)
        {
            currentScene = scene;
            SceneManager.SetActiveScene(currentScene);
        }

        public Scene GetCorrectScene()
        {
            ControllerState state = controller.GetState();
            switch (state)
            {
                case ControllerState.StartScreen:
                    return mainMenu;
                default:
                    return main;
            }
        }

        public void GotoCorrectScene()
        {
            SetScene(GetCorrectScene());
        }
    }
}
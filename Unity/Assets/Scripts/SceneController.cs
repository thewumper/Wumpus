using UnityEngine.SceneManagement;
using WumpusCore.Controller;

namespace WumpusUnity
{
    public class SceneController
    {
        /// <summary>
        /// The current Scene.
        /// </summary>
        private string currentScene;
        
        /// <summary>
        /// The global Controller object.
        /// </summary>
        private Controller controller;
        
        /// <summary>
        /// The internal reference to the global SceneController.
        /// </summary>
        internal static SceneController controllerReference;
        
        /// <summary>
        /// The global SceneController.
        /// </summary>
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
        
        /// <summary>
        /// Controls everything to do with scenes.
        /// </summary>
        private SceneController()
        {
            controller = Controller.GlobalController;
        }
        
        /// <summary>
        /// Sets the current Scene in Unity.
        /// </summary>
        /// <param name="scene">The new Scene.</param>
        public void SetScene(string scene)
        {
            currentScene = scene;
            SceneManager.LoadScene(scene);
        }
        
        /// <summary>
        /// Gets the correct scene based off of the global Controller's state.
        /// </summary>
        /// <returns>The correct scene based off of the global Controller's state.</returns>
        public string GetCorrectScene()
        {
            ControllerState state = controller.GetState();
            switch (state)
            {
                case ControllerState.StartScreen:
                    return "Main Menu";
                default:
                    return "Main";
            }
        }
        
        /// <summary>
        /// Goes to the correct scene based off of the global Controller's state.
        /// </summary>
        public void GotoCorrectScene()
        {
            SetScene(GetCorrectScene());
        }
    }
}
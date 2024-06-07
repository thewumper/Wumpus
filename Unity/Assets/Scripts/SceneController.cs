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
                if (controllerReference == null)
                {
                    controllerReference = new SceneController();
                }

                return controllerReference;
            }
        }
        
        /// <summary>
        /// Redefines the controller so it has the new controller
        /// </summary>
        public void Reinitialize(Controller newController)
        {
            controller = newController;
        }
        
        /// <summary>
        /// Sets the current Scene in Unity.
        /// </summary>
        /// <param name="scene">The new Scene.</param>
        private void SetScene(string scene)
        {
            if (currentScene == scene) SceneManager.LoadScene("Main Menu");
            currentScene = scene;
            SceneManager.LoadScene(scene);
        }
        
        /// <summary>
        /// Gets the correct scene based off of the global Controller's state.
        /// </summary>
        /// <returns>The correct scene based off of the global Controller's state.</returns>
        private string GetCorrectScene(ControllerState state)
        {
            switch (state)
            {
                case ControllerState.StartScreen:
                    return "Main Menu";
                case ControllerState.InBetweenRooms:
                    return "Hallway";
                case ControllerState.VatRoom:
                    return "Vats";
                case ControllerState.Rats:
                    return "Rats";
                case ControllerState.BatTransition:
                    return "Bat";
                case ControllerState.Acrobat:
                    return "Acrobat";
                case ControllerState.GameOver:
                    return "Game Over";
                case ControllerState.Trivia:
                    return "Trivia";
                case ControllerState.AmmoRoom:
                case ControllerState.GunRoom:
                    return "StoresRoom";
                default:
                    return "Main";
            }
        }
        
        /// <summary>
        /// Goes to the correct scene based off of the global Controller's state.
        /// </summary>
        public void GotoCorrectScene()
        {
            SetScene(GetCorrectScene(controller.GetState()));
        }

        /// <summary>
        /// Loads trivia on top of the current scene
        /// </summary>
        public void LoadTrivia()
        {
            SceneManager.LoadScene(GetCorrectScene(ControllerState.Trivia), LoadSceneMode.Additive);
        }
    }
}
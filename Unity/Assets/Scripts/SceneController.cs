using UnityEngine.SceneManagement;
namespace WumpusUnity
{
    public class SceneController
    {
        /// <summary>
        /// The current Scene.
        /// </summary>
        private Scene currentScene;

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
    }
}
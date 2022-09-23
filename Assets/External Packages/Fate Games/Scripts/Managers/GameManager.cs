using UnityEngine;
using UnityEngine.SceneManagement;

namespace FateGames
{
    public class GameManager : MonoBehaviour
    {
        #region Properties
        private LevelManager levelManager;
        [SerializeField] private bool loadCurrentLevel = true;
        [SerializeField] private ControlType controlType;
        [SerializeField] private int targetFrameRate = -1;

        [SerializeField] private bool showLevelText = true;

        [HideInInspector] public LevelManager LevelManager { get => levelManager; }
        #endregion

        private void Initialize()
        {
            if (!AvoidDuplication()) return;
            Application.targetFrameRate = targetFrameRate;
            PlayerProgression.InitializePlayerData();
            AnalyticsManager.Initialize();
            if (loadCurrentLevel)
                FacebookManager.Initialize(SceneManager.LoadCurrentLevel);
        }

        void OnEnable()
        {
            //Tell our 'OnLevelFinishedLoading' function to start listening for a scene change as soon as this script is enabled.
            UnityEngine.SceneManagement.SceneManager.sceneLoaded += OnLevelFinishedLoading;
        }

        void OnDisable()
        {
            //Tell our 'OnLevelFinishedLoading' function to stop listening for a scene change as soon as this script is disabled. Remember to always have an unsubscription for every delegate you subscribe to!
            UnityEngine.SceneManagement.SceneManager.sceneLoaded -= OnLevelFinishedLoading;
        }

        void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
        {
            levelManager = FindObjectOfType<LevelManager>();
            GlobalEventDispatcher.Emit("UPDATE_MONEY");
        }


        #region Unity Callbacks

        private void Awake()
        {
            Initialize();
        }
        private void Update()
        {
            if (State != GameState.LOADING_SCREEN)
                CheckInput();
        }

        #endregion

        #region Singleton
        private static GameManager instance;
        public static GameManager Instance { get => instance; }


        private bool AvoidDuplication()
        {
            if (!instance)
            {
                DontDestroyOnLoad(gameObject);
                instance = this;
                return true;
            }
            else
                DestroyImmediate(gameObject);
            return false;
        }

        #endregion

        private void CheckInput()
        {
            if (Input.GetKey(KeyCode.LeftControl))
            {
                if (Input.GetKeyDown(KeyCode.X) && State == GameState.IN_GAME)
                    SceneManager.FinishLevel(true);
                else if (Input.GetKeyDown(KeyCode.C) && State == GameState.IN_GAME)
                    SceneManager.FinishLevel(false);
            }
            if (Input.GetMouseButtonDown(0) && State == GameState.START_SCREEN)
                SceneManager.StartLevel();
        }

        #region State Management
        private GameState state = GameState.LOADING_SCREEN;
        public GameState State { get => state; }
        public bool ShowLevelText { get => showLevelText; }
        public ControlType ControlType { get => controlType; }

        public void UpdateGameState(GameState newState)
        {
            state = newState;
        }
        #endregion

    }
    public enum GameState { LOADING_SCREEN, START_SCREEN, IN_GAME, PAUSE_SCREEN, FINISHED, COMPLETE_SCREEN }
    public enum ControlType { JOYSTICK, SWIPE }
}
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GenericHighlightButton : MonoBehaviour
{
    enum EMenuOptions : int
    {
        StartGame = 0,
        ExitGame = 1,
        MainMenu = 2,
        OptionCount,
    }

    [SerializeField] private SoundController soundController = null;

    [SerializeField] private KeyCode upKeyCode = KeyCode.W;
    [SerializeField] private KeyCode downKeyCode = KeyCode.S;
    [SerializeField] private KeyCode selectKeyCode = KeyCode.Space;

    [SerializeField] private AudioClip selectAudioClip;
    [SerializeField] private AudioClip moveAudioClip;

    [SerializeField] private TMP_Text startGameText = null;
    [SerializeField] private TMP_Text exitGameText = null;
    [SerializeField] private TMP_Text mainMenuText = null;

    private FontStyles normalMenuTextFontStyle = FontStyles.Normal;
    private Color normalMenuTextColor = Color.white;
    private FontStyles selectedMenuTextFontStyle = FontStyles.Bold;
    private Color selectedMenuTextColor = new Color(0.8f, 0.1f, 0.1f);

    private EMenuOptions selectedOption;

    private void SelectMenuText(TMP_Text text)
    {
        // Check the text is valid
        if (text)
        {
            text.fontStyle = selectedMenuTextFontStyle;
            text.color = selectedMenuTextColor;
        }
    }

    private void DeselectMenuText(TMP_Text text)
    {
        // Check the text is valid
        if (text)
        {
            text.fontStyle = normalMenuTextFontStyle;
            text.color = normalMenuTextColor;
        }
    }

    private void SelectMenuOption(EMenuOptions option)
    {
        // Select corresponding option
        switch (option)
        {
            case EMenuOptions.StartGame: SelectStartGame(); break;
            case EMenuOptions.ExitGame: SelectExitGame(); break;
            case EMenuOptions.MainMenu: SelectMainMenu(); break;
        }
    }

    private void StartGameSelected()
    {
        Debug.Log("Start game selected");
        SceneManager.LoadScene(1); //jump into first level
    }

    private void ExitGameSelected()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        Debug.Log("Exit game selected");
    }

    private void MainMenuSelected()
    {
        Debug.Log("Main Menu selected");
        SceneManager.LoadScene(0); //jumps to menu
    }

    void SelectStartGame()
    {
        // Deselect all other menu text
        DeselectAllMenuText();

        // Select the start game text
        SelectMenuText(startGameText);

        // Set the selected menu option to start game
        selectedOption = EMenuOptions.StartGame;
    }

    void SelectExitGame()
    {
        // Deselect all other menu text
        DeselectAllMenuText();

        // Select the exit game text
        SelectMenuText(exitGameText);

        // Set the selected menu option to exit game
        selectedOption = EMenuOptions.ExitGame;
    }

    void SelectMainMenu()
    {
        // Deselects all other menu text
        DeselectAllMenuText();

        // Selects the Main Menu Text
        SelectMenuText(mainMenuText);
        
        // set the selected menu option to main menu
        selectedOption = EMenuOptions.MainMenu;
    }

    void DeselectAllMenuText()
    {
        // Deselect the start menu text
        DeselectMenuText(startGameText);

        // Deselect the exit menu text
        DeselectMenuText(exitGameText);

        // Deselects the main menu text
        DeselectMenuText(mainMenuText);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Start with start game selected
        SelectMenuOption(EMenuOptions.StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        // If up is pressed
        if (Input.GetKeyDown(upKeyCode))
        {
            // Play the menu move sound
            soundController.PlaySoundClipOneShot(moveAudioClip);

            // Scroll up through the menu
            selectedOption = (EMenuOptions)Mathf.Abs(((int)selectedOption - ((selectedOption == 0) ? ((int)EMenuOptions.OptionCount - 1) : 1)) % ((int)EMenuOptions.OptionCount));
            SelectMenuOption(selectedOption);
        }

        // If down is pressed
        if (Input.GetKeyDown(downKeyCode))
        {
            // Play the menu move sound
            soundController.PlaySoundClipOneShot(moveAudioClip);

            // Scroll down through the menu
            //selectedOption = (EMenuOptions)Mathf.Abs((((int)selectedOption - 1) % ((int)EMenuOptions.OptionCount)));
            selectedOption = (EMenuOptions)(((int)selectedOption + 1) % (int)EMenuOptions.OptionCount);
            SelectMenuOption(selectedOption);
        }

        // If select is pressed
        if (Input.GetKeyDown(selectKeyCode))
        {
            // Play the menu select sound
            soundController.PlaySoundClipOneShot(selectAudioClip);

            // Select the menu option
            switch (selectedOption)
            {
                case EMenuOptions.StartGame: StartGameSelected(); break;
                case EMenuOptions.ExitGame: ExitGameSelected(); break;
                case EMenuOptions.MainMenu: MainMenuSelected(); break;
            }
        }
    }
}

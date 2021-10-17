using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    enum EMenuOptions : int
    {
        StartGame = 0,
        ExitGame,
        OptionCount,
    }

    [SerializeField] private KeyCode upKeyCode = KeyCode.W;
    [SerializeField] private KeyCode downKeyCode = KeyCode.S;
    [SerializeField] private KeyCode selectKeyCode = KeyCode.Return;

    [SerializeField] private AudioClip selectAudioClip;
    [SerializeField] private AudioClip moveAudioClip;

    [SerializeField] private TMP_Text startGameText = null;
    [SerializeField] private TMP_Text exitGameText = null;

    [SerializeField] private string InGameSceneName = "";

    private AudioSource audioSource = null;

    private FontStyles normalMenuTextFontStyle = FontStyles.Normal;
    private Color normalMenuTextColor = Color.white;
    private FontStyles selectedMenuTextFontStyle = FontStyles.Bold;
    private Color selectedMenuTextColor = new Color(0.8f, 0.1f, 0.1f);

    private EMenuOptions selectedOption;

    private void PlaySoundClipOneShot(AudioClip clip)
    {
        // Check the audio source component is valid
        if (audioSource)
        {
            // Check the clip is valid
            if(clip)
            {
                // Play the clip one shot
                audioSource.PlayOneShot(clip);
            }
        }
    }

    private void SelectMenuText(TMP_Text text)
    {
        // Check the text is valid
        if(text)
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
        switch(option)
        {
            case EMenuOptions.StartGame: SelectStartGame(); break;
            case EMenuOptions.ExitGame: SelectExitGame(); break;
        }
    }

    private void StartGameSelected()
    {
        Debug.Log("Start game selected");
    }

    private void ExitGameSelected()
    {
        Debug.Log("Exit game selected");
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

    void DeselectAllMenuText()
    {
        // Deselect the start menu text
        DeselectMenuText(startGameText);

        // Deselect the exit menu text
        DeselectMenuText(exitGameText);
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get the audio source component
        audioSource = GetComponent<AudioSource>();

        // Start with start game selected
        SelectMenuOption(EMenuOptions.StartGame);
    }

    // Update is called once per frame
    void Update()
    {
        // If up is pressed
        if(Input.GetKeyDown(upKeyCode))
        {
            // Play the menu move sound
            PlaySoundClipOneShot(moveAudioClip);

            // Scroll up through the menu
            selectedOption = (EMenuOptions)(((int)selectedOption + 1) % (int)EMenuOptions.OptionCount);
            SelectMenuOption(selectedOption);
        }

        // If down is pressed
        if (Input.GetKeyDown(downKeyCode))
        {
            // Play the menu move sound
            PlaySoundClipOneShot(moveAudioClip);

            // Scroll down through the menu
            selectedOption = (EMenuOptions)Mathf.Abs((((int)selectedOption -1) % ((int)EMenuOptions.OptionCount)));
            SelectMenuOption(selectedOption);
        }

        // If select is pressed
        if (Input.GetKeyDown(selectKeyCode))
        {
            // Play the menu select sound
            PlaySoundClipOneShot(selectAudioClip);

            // Select the menu option
            switch(selectedOption)
            {
                case EMenuOptions.StartGame: StartGameSelected(); break;
                case EMenuOptions.ExitGame: ExitGameSelected(); break;
            }
        }
    }
}

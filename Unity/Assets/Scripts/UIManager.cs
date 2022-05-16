using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
    {
    //Create a variable that is the same across all instances of UIManager
    public static UIManager instance;

    [Header("Gameplay Panel")]
    public RectTransform gameplayPanel;
    [Header("Timer Text")]
    public Text timerText;
    [Header("Item List Object Template")]
    public Text itemListTemplate;
    [Header("Padding for the Strikethrough")]
    public int strikethroughPadding;
    [Header("Time it takes to strikethrough")]
    public float strikethroughTime;

    //Creates a dictionary that allows us to find the Text object associated with the SceneObject in our Check List
    private Dictionary<SceneObject, Text> checkListOfSceneObjects = new Dictionary<SceneObject, Text>();

    [Header("Win and Lose panels")]
    public RectTransform winPanel;
    public RectTransform losePanel;


    // Start is called before the first frame update
    void Start()
        {
        //Sets the static instance variable equal to this instance of the UIManager
        instance = this;

        //Disable the Win/Lose panels when the game starts
        winPanel.gameObject.SetActive(false);
        losePanel.gameObject.SetActive(false);
        //Enable the Gameplay panel when the game starts
        gameplayPanel.gameObject.SetActive(true);

        //Go through the list of sceneObjects in the GameManager to add it's name to the List
        foreach (SceneObject sceneObject in GameManager.instance.sceneObjects)
            {
            //Create a new text object for the list
            Text newTextForList = Instantiate(itemListTemplate, itemListTemplate.transform.parent);

            //Position it further down the list as more items get added
            newTextForList.rectTransform.anchoredPosition -= new Vector2(0, newTextForList.rectTransform.sizeDelta.y * checkListOfSceneObjects.Count);

            //Change the text on the checklist to have the scene objects name
            newTextForList.text = sceneObject.name;

            //Make sure the newTextForList is active in the Hierarchy
            newTextForList.gameObject.SetActive(true);

            //Add the sceneObject and text object to the dictionary
            checkListOfSceneObjects.Add(sceneObject, newTextForList);
            }

        //Start the countdown for the gameplay
        StartCoroutine(GameplayTimer());
        }

    //A coroutine that counts down the seconds until the end of the game
    IEnumerator GameplayTimer()
        {
        //Create a variable for how many seconds until the game ends
        int secondsUntilGameEnds = GameManager.instance.secondsUntilGameEnds;

        //if the secondsUntilGameEnds is greater than 0 then continue looping
        while (secondsUntilGameEnds > 0)
            {
            //Subtract 1 from the number of seconds
            secondsUntilGameEnds -= 1;

            //Set the text for our timer
            timerText.text = "Time Left: " + secondsUntilGameEnds;

            //Wait 1 second before continuing the loop
            yield return new WaitForSeconds(1);
            }

        //The timer has run out, deactive the gameplay panel
        gameplayPanel.gameObject.SetActive(false);
        //Activate the lose panel
        losePanel.gameObject.SetActive(true);

        yield return null;
        }

    //A function that is called to start the coroutine to strike through the text in the Check List
    public void StrikeThrough(SceneObject selectedObject)
        {
        //Starts the coroutine called "Strike"
        StartCoroutine(Strike(selectedObject));
        }

    //The couroutine that will strike through the text in the Check List when the object is selected
    IEnumerator Strike(SceneObject selectedObject)
        {
        //The text object associated with the scene object
        Text textObject = checkListOfSceneObjects[selectedObject];
        //The image being used to strike through the text object
        Image strikeImage = textObject.GetComponentInChildren<Image>();
        //The timer for the strikethrough
        float t = 0;
        //Add padding to the strike image
        strikeImage.rectTransform.anchoredPosition += new Vector2(strikethroughPadding, 0);

        //Get the original size of the strike image to transition from
        Vector2 originalSize = new Vector2(0, strikeImage.rectTransform.sizeDelta.y);

        //Get the newSize of the strikeImage to transition to
        Vector2 newSize = new Vector2(textObject.rectTransform.sizeDelta.x - strikethroughPadding * 2, strikeImage.rectTransform.sizeDelta.y);

        //If the timer is less than the strikethroughTime, continue looping
        while (t < strikethroughTime)
            {
            //Add the time between each frame to the timer
            t += Time.deltaTime;

            //Smoothly transition from the originalSize to the newSize in the strikethroughTime
            strikeImage.rectTransform.sizeDelta = Vector2.Lerp(originalSize, newSize, t / strikethroughTime);

            yield return null;
            }

        //Call the function in GameManager called CheckForWin and if it returns true
        if (GameManager.instance.CheckForWin() == true)
            {
            //The player has won
            //Deactivate the gameplayPanel
            gameplayPanel.gameObject.SetActive(false);
            //Activate the winPanel
            winPanel.gameObject.SetActive(true);
            }

        yield return null;
        }

    //A function that will be activated when the player clicks the Restart button
    public void OnClickRestartButton()
        {
        //Reload the currently active scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

    //A function that will be activated when the player clicks the Next Level button
    public void OnClickNextLevelButton()
        {
        //Load the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

    //A function that will be activated when the player clicks the Main Menu button
    public void OnClickMainMenuButton()
        {
        //Load the first scene (Main Menu)
        SceneManager.LoadScene(0);
        }
    }
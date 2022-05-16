using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
    {
    //Create a variable that is the same across all instances of GameManager
    public static GameManager instance;

    [Header("All SceneObjects the Player needs to find")]
    public List<SceneObject> sceneObjects = new List<SceneObject>();

    [Header("How much time (in seconds) the Player has")]
    public int secondsUntilGameEnds;

    [Header("Should objects be highlighted when the mouse hovers over them")]
    public bool highlightObjectsOnHover;

    [Header("The colors for the Highlight and Selection outlines")]
    public Color highlightColor = new Color(1, 1, 1, 1);
    public Color selectionColor = new Color(1, 1, 1, 1);

    [Header("How wide should the outline be")]
    //Displays a slider for numbers between 0 and 10 in the Inspector tab
    [SerializeField, Range(0f, 10f)]
    public float outlineWidth;

    //Awake is called before the scene has finished loading
    void Awake()
        {
        //Sets the static instance variable equal to this instance of the GameManager
        instance = this;
        }

    //This function is to try to select the SceneObject but only if it's on the checklist of objects
    public void TrySelect(SceneObject selectedObject)
        {
        //If the selected object has not been selected yet and it's on the list of objects
        if (selectedObject.selected == false && sceneObjects.Contains(selectedObject) == true)
            {
            //Call the Select function on the selected object to finalize the selection
            selectedObject.Select();

            //Strike through the text in the UIManager list
            UIManager.instance.StrikeThrough(selectedObject);
            }
        }

    //This function will return a true/false value. True for win. False for not win
    public bool CheckForWin()
        {
        //This will go through our checklist one SceneObject at a time
        //To determine if all of them have been found
        foreach (SceneObject sceneObject in sceneObjects)
            {
            //If even one object has not been found, the player has not won
            if (sceneObject.selected == false)
                {
                return false;
                }
            }

        return true;
        }
    }
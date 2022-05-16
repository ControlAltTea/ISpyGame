using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Forces the SceneObject script to be accompanied by the GameObjectOutline script
[RequireComponent(typeof(GameObjectOutline))]
public class SceneObject : MonoBehaviour
{
    [Header("Colors for Highlighting / Selecting")]
    public Color highlightColor = new Color(1,1,1,1);
    public Color selectColor = new Color(1, 1, 0, 1);

    [Header("Has this Object already been selected?")]
    public bool selected;

    //The GameObjectOutline script attached to this GameObject
    private GameObjectOutline outlineScript;

    // Start is called before the first frame update
    void Start()
    {
        //Set the outlineScript variable equal to the Outline script attached to this GameObject
        outlineScript = GetComponent<GameObjectOutline>();

        //Make the outline transparent for now
        outlineScript.OutlineColor = new Color(1, 1, 1, 0);
    }

    //This function is called whenever the Mouse enters the collider for the Mesh
    private void OnMouseEnter()
    {
        //If the Object has not already been selected and objects should have a highlight when the mouse is over them
        if (selected == false && GameManager.instance.highlightObjectsOnHover)
        {
            //Set the outline color to the highlight color
            outlineScript.OutlineColor = highlightColor;
        }
    }

    //This function is called whenever the Mouse is no longer over the collider for the Mesh
    private void OnMouseExit()
    {
        //If the Object has not already been selected
        if (selected == false)
        {
            //Make the outline transparent for now
            outlineScript.OutlineColor = new Color(1, 1, 1, 0);
        }
    }

    //This function is called whenever the Mouse is clicking while the mouse is over the collider for the Mesh
    private void OnMouseDown()
    {
        //Call the function from GameManager called TrySelect and pass in THIS instance of the script as a parameter
        GameManager.instance.TrySelect(this);
    }

    //This function is called from GameManager to select this object
    public void Select()
    {
        //Set the selected variable to true
        selected = true;

        //Set the outline color to the selected color
        outlineScript.OutlineColor = selectColor;
    }

}

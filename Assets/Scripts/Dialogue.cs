using System;
using System.Collections;
using System.Collections.Generic;
using SojaExiles;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[Serializable]
public struct DialogueElements
{
    [TextAreaAttribute]
    public string dialogueText;
    public string answerText1;
    public UnityEvent answerAction1;
    public string answerText2;
    public UnityEvent answerAction2;
}

public class Dialogue : MonoBehaviour
{
    [SerializeField] 
    private GameObject dialogueDisplay;
    [SerializeField] 
    private TextMeshProUGUI textElement;
    [SerializeField]
    private Button answer1Button;
    [SerializeField] 
    private Button answer2Button;
    [SerializeField] 
    private DialogueElements[] dialogueElements;
    
    private GameObject playerReference;
    private bool wasTriggered = false;


    public void OpenDialogue()
    {
        dialogueDisplay.SetActive(true);
        LoadDialogueElements(0);
    }
    
    public void CloseDialogue()
    {
        playerReference.GetComponent<PlayerInput>().Enable();
        Camera.main.GetComponent<PlayerInput>().Enable();
        dialogueDisplay.SetActive(false);
    }

    public void LoadDialogueElements(int index)
    {
        Debug.Log(index);
        DialogueElements currentDialogueElements = dialogueElements[index];
        textElement.text = currentDialogueElements.dialogueText;
        if (currentDialogueElements.answerAction1 != null)
        {
            answer1Button.gameObject.SetActive(true);
            answer1Button.GetComponentInChildren<TextMeshProUGUI>().text = dialogueElements[index].answerText1;
            answer1Button.onClick.RemoveAllListeners();
            answer1Button.onClick.AddListener(() => currentDialogueElements.answerAction1.Invoke());
        }
        else
            answer1Button.gameObject.SetActive(false);

        if (currentDialogueElements.answerAction2 != null)
        {
            answer2Button.gameObject.SetActive(true);
            answer2Button.GetComponentInChildren<TextMeshProUGUI>().text = dialogueElements[index].answerText2;
            answer2Button.onClick.RemoveAllListeners();
            answer2Button.onClick.AddListener(() => currentDialogueElements.answerAction2.Invoke());
        }
        else
            answer2Button.gameObject.SetActive(false);
    }

    void OnTriggerEnter(Collider other)
    {
        if (wasTriggered)
            return;
        if (other.gameObject.CompareTag("Player"))
        {
            wasTriggered = true;
            OpenDialogue();
            playerReference = other.gameObject;
            playerReference.GetComponent<PlayerInput>().Enable();
            Camera.main.GetComponent<PlayerInput>().Enable();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            wasTriggered = false;
        }
    }
}

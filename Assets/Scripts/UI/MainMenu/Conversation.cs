using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI.MainMenu
{
    public class Conversation : MonoBehaviour
    {
        [SerializeField] private GameObject messageDisplay;
        [SerializeField] private Transform messageContainer;
        [SerializeField] private TMP_InputField userMessageField;
        [SerializeField] private Button sendButton;

        [Header("Voice")]
        [SerializeField] private Button voiceButton;
        [SerializeField] private Image voiceDisplay;
        [SerializeField] private Sprite mikeIcon;
        [SerializeField] private Sprite voiceIcon;
        [SerializeField] private Sprite sendIcon;
        [SerializeField] private Sprite cancelIcon;

        [Header("Workout")] 
        [SerializeField] private GameObject workoutProposal;
        [SerializeField] private Button workoutButton;

        private TMP_Text userMessagePlaceholder;
        private Image sendDisplay;

        private string defaultMessageText;
        private string userMessage;

        private bool isRecordingMessage;


        void Start()
        {
            userMessagePlaceholder = userMessageField.placeholder.GetComponent<TMP_Text>();
            defaultMessageText = userMessagePlaceholder.text;
            sendDisplay = sendButton.GetComponent<Image>();

            userMessageField.onValueChanged.AddListener(SetMessage);
            sendButton.onClick.AddListener(TrySendMessage);
            voiceButton.onClick.AddListener(SetVoiceMessage);

            workoutButton.onClick.AddListener(SetWorkout);
        }

        private void SetMessage(string _message)
        {
            userMessage = _message;
        }

        private void TrySendMessage()
        {
            if (isRecordingMessage)
            {
                voiceDisplay.sprite = mikeIcon;
                sendDisplay.sprite = sendIcon;

                isRecordingMessage = false;
            }
            else if (userMessageField.text != "") SendMessage();
        }

        private void SendMessage()
        {
            GameObject message = Instantiate(messageDisplay, messageContainer);
            message.GetComponentInChildren<TMP_Text>().text = userMessage;

            userMessageField.text = "";
            userMessagePlaceholder.text = defaultMessageText;
        }

        private void SetVoiceMessage()
        {
            if (isRecordingMessage)
            {
                voiceDisplay.sprite = mikeIcon;
                sendDisplay.sprite = sendIcon;

                userMessage = "";
                SendMessage();
            }
            else
            {
                voiceDisplay.sprite = voiceIcon;
                sendDisplay.sprite = cancelIcon;
            }

            isRecordingMessage = !isRecordingMessage;
        }

        private void SetWorkout()
        {
            Instantiate(workoutProposal, transform.parent);
        }
    }
}


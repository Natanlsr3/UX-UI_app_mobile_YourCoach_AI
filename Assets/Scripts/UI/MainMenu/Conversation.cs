using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MVC.App.UI
{
    public class Conversation : MonoBehaviour
    {
        [SerializeField] private GameObject messageDisplay;
        [SerializeField] private Transform messageContainer;
        [SerializeField] private TMP_InputField userMessageField;
        [SerializeField] private Button sendButton;

        private TMP_Text userMessagePlaceholder;
        private string defaultMessageText;

        private string userMessage;

        void Start()
        {
            userMessagePlaceholder = userMessageField.placeholder.GetComponent<TMP_Text>();
            defaultMessageText = userMessagePlaceholder.text;

            userMessageField.onValueChanged.AddListener(SetMessage);
            sendButton.onClick.AddListener(SendMessage);
        }

        private void SetMessage(string _message)
        {
            userMessage = _message;
        }

        private void SendMessage()
        {
            GameObject message = Instantiate(messageDisplay, messageContainer);
            message.GetComponentInChildren<TMP_Text>().text = userMessage;

            userMessageField.text = "";
            userMessagePlaceholder.text = defaultMessageText;
        }
    }
}


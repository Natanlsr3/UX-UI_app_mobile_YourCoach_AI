using System;
using System.Collections;
using System.Collections.Generic;
using MVC.App.UI.Workout;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;
using UnityEngine.Video;

namespace MVC.App.UI.MainMenu
{
    public class Conversation : MonoBehaviour
    {
        [SerializeField] private GameObject userMessageDisplay;
        [SerializeField] private GameObject coachMessageDisplay;
        [SerializeField] private GameObject fillMessage;

        [SerializeField] private Transform userMessageContainer;
        [SerializeField] private Transform coachMessageContainer;

        [SerializeField] private Transform chat;
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
        [SerializeField] private GameObject workoutButton;

        [Header("Coach")]
        [SerializeField] private VideoPlayer coachVideo;
        [SerializeField] private VideoClip coachIdle;
        [SerializeField] private VideoClip coachTalking;
        [SerializeField] private VideoClip coachListening;
        [Serializable]
        private struct Talk
        {
            public string Message; // The message from the user
            public string Answer; // The answer from the coach
        }
        [SerializeField] private List<Talk> coachAnswersProfile = new List<Talk>();
        [SerializeField] private float answerDelay;

        private enum CoachState { Void, Idle, Talking, Listening }

        private CoachState currentCoachState = CoachState.Void;

        private PostProcessVolume ppVolume;

        private Image sendDisplay;

        private int conversationStep;

        private string userMessage;

        // To check if there is a mike recording
        private bool isRecordingMessage;

        void Start()
        {
            ppVolume = Camera.main.GetComponent<PostProcessVolume>();

            sendDisplay = sendButton.GetComponent<Image>();

            // Connect the different elements
            userMessageField.onValueChanged.AddListener(SetMessage);
            sendButton.onClick.AddListener(TrySendMessage);
            voiceButton.onClick.AddListener(SetVoiceMessage);

            MakeCoachIdle();

            if (PlayerPrefs.HasKey("TrainingSessionDone"))
            {
                conversationStep = 2;
            }
            SendCoachMessage(coachAnswersProfile[conversationStep].Answer);

            ChallengeTracker.Instance.Progress(ChallengeTracker.ChallengeType.Connexion, 1f);
        }

        /// <summary>
        /// Set the current user message and make the coach react to it
        /// </summary>
        /// <param name="_message"></param>
        private void SetMessage(string _message)
        {
            userMessage = _message;
            MakeCoachListen();
        }

        /// <summary>
        /// Check if there is a current message to send
        /// </summary>
        private void TrySendMessage()
        {
            if (isRecordingMessage)
            {
                voiceDisplay.sprite = mikeIcon;
                sendDisplay.sprite = sendIcon;

                isRecordingMessage = false;
            }
            else if (userMessageField.text != "") SendUserMessage();
        }

        /// <summary>
        /// Create the button to go to the proposed workout
        /// </summary>
        private void ProposeWorkout()
        {
            //Instantiate and configurate the workout button
            Button _workoutButton = Instantiate(workoutButton, coachMessageContainer).GetComponent<Button>();
            _workoutButton.onClick.AddListener(SetWorkout);
            FillMessage(userMessageContainer, "");
        }

        /// <summary>
        /// Send and display a message with the text entered by the user
        /// </summary>
        private void SendUserMessage()
        {
            string _messageText = userMessage;

            GameObject message = Instantiate(userMessageDisplay, userMessageContainer);
            message.GetComponentInChildren<TMP_Text>().text = _messageText;

            userMessageField.text = "";

            FillMessage(coachMessageContainer, _messageText);

            // Start a timer for the coach to answer
            StartCoroutine(AnswerCoroutine());
        }

        /// <summary>
        /// Make the coach answer the user message after a short delay
        /// </summary>
        /// <returns></returns>
        private IEnumerator AnswerCoroutine()
        {
            yield return new WaitForSeconds(answerDelay);

            SendCoachMessage(coachAnswersProfile[conversationStep].Answer);

            StopCoroutine(AnswerCoroutine());
        }

        /// <summary>
        /// Send and display the coach answer while making him talk
        /// </summary>
        /// <param name="_message"></param>
        private void SendCoachMessage(string _message)
        {
            string _messageText = _message;

            GameObject message = Instantiate(coachMessageDisplay, coachMessageContainer);
            message.GetComponentInChildren<TMP_Text>().text = _messageText;

            FillMessage(userMessageContainer, _messageText);
            SoundManager.instance.StopClip();
            SoundManager.instance.GoToClip(SoundManager.instance.conversationLines, conversationStep);
            SoundManager.instance.PlayClip();
            conversationStep++;
            
            MakeCoachTalk();

            if (conversationStep == 2)
                Invoke("ProposeWorkout", 15f);
        }

        /// <summary>
        /// Create a blank message in the given container
        /// </summary>
        /// <param name="_container"></param>
        /// <param name="_message"></param>
        private void FillMessage(Transform _container, string _message)
        {
            //Spawn an invisible message to create a gap in the chosen container, to recreate this effect of overlapping messages
            GameObject fill = Instantiate(fillMessage, _container);
            fill.GetComponentInChildren<TMP_Text>().text = _message;
        }

        /// <summary>
        /// Switch state of the voice message and update display according to it
        /// </summary>
        private void SetVoiceMessage()
        {
            isRecordingMessage = !isRecordingMessage;

            if (isRecordingMessage)
            {
                voiceDisplay.sprite = voiceIcon;
                sendDisplay.sprite = cancelIcon;
            }
            else
            {
                voiceDisplay.sprite = mikeIcon;
                sendDisplay.sprite = sendIcon;

                userMessage = "...";
                SendUserMessage();
            }
        }

        /// <summary>
        /// Create and display the workout proposal
        /// </summary>
        private void SetWorkout()
        {
            SetConversationDisplay(false);
            WorkoutProposal _proposal = Instantiate(workoutProposal, transform.parent).GetComponent<WorkoutProposal>();
            _proposal.Conversation = this;
        }

        /// <summary>
        /// Display or hide the conversation
        /// </summary>
        /// <param name="_isDisplayed"></param>
        public void SetConversationDisplay(bool _isDisplayed)
        {
            ppVolume.enabled = !_isDisplayed;
            chat.gameObject.SetActive(_isDisplayed);
        }

        /// <summary>
        /// Set the coach to idle state, with idle animation
        /// </summary>
        private void MakeCoachIdle()
        {
            if (currentCoachState == CoachState.Idle) return;

            coachVideo.Stop();
            coachVideo.clip = coachIdle;
            coachVideo.isLooping = true;

            coachVideo.time = 0f;
            coachVideo.Play();

            currentCoachState = CoachState.Idle;
        }

        /// <summary>
        /// Set the coach to talking state, with talking animation
        /// </summary>
        private void MakeCoachTalk()
        {
            if (currentCoachState == CoachState.Talking) return;

            coachVideo.Stop();
            coachVideo.clip = coachTalking;
            coachVideo.isLooping = true;

            coachVideo.time = 0f;
            coachVideo.Play();
            StartCoroutine(TalkCoroutine());

            currentCoachState = CoachState.Talking;
        }

        /// <summary>
        /// Set the coach to listening state, with listening animation
        /// </summary>
        private void MakeCoachListen()
        {
            if (currentCoachState == CoachState.Listening) return;

            coachVideo.Stop();
            coachVideo.clip = coachListening;
            coachVideo.isLooping = true;

            coachVideo.time = 0f;
            coachVideo.Play();

            currentCoachState = CoachState.Listening;
        }

        /// <summary>
        /// Make the coach talk until the end of his message
        /// </summary>
        /// <returns></returns>
        private IEnumerator TalkCoroutine()
        {
            yield return new WaitWhile(() => SoundManager.instance.soundSource.isPlaying);
            MakeCoachIdle();
            StopCoroutine(TalkCoroutine());
        }
    }
}


using System;
using System.Collections;
using System.Collections.Generic;
using MVC.App.UI.Workout;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Serialization;
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
            [SerializeField] public string message;
            [SerializeField] public string answer;
        }
        [SerializeField] private List<Talk> coachAnswersProfile = new List<Talk>();
        [SerializeField] private float answerDelay;

        private enum CoachState { Void, Idle, Talking, Listening }

        private CoachState currentCoachState = CoachState.Void;

        private PostProcessVolume ppVolume;

        private TMP_Text userMessagePlaceholder;
        private Image sendDisplay;

        private int conversationStep;

        private string defaultMessageText;
        private string userMessage;

        private bool isRecordingMessage;

        void Start()
        {
            ppVolume = Camera.main.GetComponent<PostProcessVolume>();

            userMessagePlaceholder = userMessageField.placeholder.GetComponent<TMP_Text>();
            defaultMessageText = userMessagePlaceholder.text;
            sendDisplay = sendButton.GetComponent<Image>();

            userMessageField.onValueChanged.AddListener(SetMessage);
            sendButton.onClick.AddListener(TrySendMessage);
            voiceButton.onClick.AddListener(SetVoiceMessage);

            MakeCoachIdle();

            if (PlayerPrefs.HasKey("TrainingSessionDone"))
            {
                conversationStep = 2;
            }
            SendCoachMessage(coachAnswersProfile[conversationStep].answer);

            ChallengeTracker.Instance.Progress(ChallengeTracker.ChallengeType.Connexion, 1f);
        }

        private void SetMessage(string _message)
        {
            userMessage = _message;
            MakeCoachListen();
        }

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

        private void ProposeWorkout()
        {
            //Instantiate and configurate the workout button
            Button _workoutButton = Instantiate(workoutButton, coachMessageContainer).GetComponent<Button>();
            _workoutButton.onClick.AddListener(SetWorkout);
            FillMessage(userMessageContainer, "");
        }

        private void SendUserMessage()
        {
            string _messageText = userMessage;

            GameObject message = Instantiate(userMessageDisplay, userMessageContainer);
            message.GetComponentInChildren<TMP_Text>().text = _messageText;

            userMessageField.text = "";
            //userMessagePlaceholder.text = defaultMessageText;

            FillMessage(coachMessageContainer, _messageText);

            StartCoroutine(AnswerCoroutine());
        }

        private IEnumerator AnswerCoroutine()
        {
            yield return new WaitForSeconds(answerDelay);

            SendCoachMessage(coachAnswersProfile[conversationStep].answer);

            StopCoroutine(AnswerCoroutine());
        }

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

        private void FillMessage(Transform _container, string _message)
        {
            //Spawn an invisible message to create a gap in the chosen container
            GameObject fill = Instantiate(fillMessage, _container);
            fill.GetComponentInChildren<TMP_Text>().text = _message;
        }

        private void SetVoiceMessage()
        {
            if (isRecordingMessage)
            {
                voiceDisplay.sprite = mikeIcon;
                sendDisplay.sprite = sendIcon;

                userMessage = "...";
                SendUserMessage();
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
            SetConversationDisplay(false);
            WorkoutProposal _proposal = Instantiate(workoutProposal, transform.parent).GetComponent<WorkoutProposal>();
            _proposal.Conversation = this;
        }

        public void SetConversationDisplay(bool _isDisplayed)
        {
            ppVolume.enabled = !_isDisplayed;
            chat.gameObject.SetActive(_isDisplayed);
        }

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

        private IEnumerator TalkCoroutine()
        {
            yield return new WaitWhile(() => SoundManager.instance.soundSource.isPlaying);
            MakeCoachIdle();
            StopCoroutine(TalkCoroutine());
        }
    }
}


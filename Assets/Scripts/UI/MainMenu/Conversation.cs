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
        [SerializeField] private Button triggerButton;

        [Header("Coach")]
        [SerializeField] private VideoPlayer coachVideo;
        [SerializeField] private VideoClip coachIdle;
        [SerializeField] private VideoClip coachTalking;
        [Serializable]
        private struct Talk
        {
            [SerializeField] public string message;
            [SerializeField] public string answer;
        }
        [SerializeField] private List<Talk> coachAnswersProfile1 = new List<Talk>();
        [SerializeField] private List<Talk> coachAnswersProfile2 = new List<Talk>();
        [SerializeField] private float answerDelay;

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

            triggerButton.onClick.AddListener(ProposeWorkout);

            MakeCoachIdle();

            if (LogSession.Instance.UserIndex == 0)
            {
                if (PlayerPrefs.HasKey("TrainingSessionDone"))
                {
                    conversationStep = 2;
                }
                
                SendCoachMessage(coachAnswersProfile1[conversationStep].answer);
            }
            else 
            {
                if (PlayerPrefs.HasKey("TrainingSessionDone"))
                {
                    conversationStep = 2;
                }
                SoundManager.instance.GoToClip(SoundManager.instance.conversationLines, conversationStep);
                SoundManager.instance.PlayClip();
                SendCoachMessage(coachAnswersProfile2[conversationStep].answer);
            }
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
            else if (userMessageField.text != "") SendUserMessage();
        }

        private void ProposeWorkout()
        {
            GameObject message = Instantiate(coachMessageDisplay, coachMessageContainer);
            message.GetComponentInChildren<TMP_Text>().text = "Get ready for your workout !";
            FillMessage(userMessageContainer);

            Button _workoutButton = Instantiate(workoutButton, coachMessageContainer).GetComponent<Button>();
            _workoutButton.onClick.AddListener(SetWorkout);
            FillMessage(userMessageContainer);

            MakeCoachTalk();
        }

        private void SendUserMessage()
        {
            GameObject message = Instantiate(userMessageDisplay, userMessageContainer);
            message.GetComponentInChildren<TMP_Text>().text = userMessage;

            userMessageField.text = "";
            userMessagePlaceholder.text = defaultMessageText;

            FillMessage(coachMessageContainer);

            StartCoroutine(AnswerCoroutine());
        }

        private IEnumerator AnswerCoroutine()
        {
            yield return new WaitForSeconds(answerDelay);

            if (LogSession.Instance.UserIndex == 0) SendCoachMessage(coachAnswersProfile1[conversationStep].answer);
            else SendCoachMessage(coachAnswersProfile2[conversationStep].answer);

            StopCoroutine(AnswerCoroutine());
        }

        private void SendCoachMessage(string _message)
        {
            GameObject message = Instantiate(coachMessageDisplay, coachMessageContainer);
            message.GetComponentInChildren<TMP_Text>().text = _message;

            FillMessage(userMessageContainer);
            SoundManager.instance.StopClip();
            SoundManager.instance.GoToClip(SoundManager.instance.conversationLines, conversationStep);
            //SoundManager.instance.NextClip(SoundManager.instance.conversationLines);
            SoundManager.instance.PlayClip();
            conversationStep++;
            
            MakeCoachTalk();

            if (conversationStep == 2)
                Invoke("ProposeWorkout", 15f);
        }

        private void FillMessage(Transform _container)
        {
            Instantiate(fillMessage, _container);
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
            coachVideo.Stop();
            coachVideo.clip = coachIdle;
            coachVideo.isLooping = true;

            coachVideo.time = 0f;
            coachVideo.Play();
        }

        private void MakeCoachTalk()
        {
            coachVideo.Stop();
            coachVideo.clip = coachTalking;
            coachVideo.isLooping = false;

            coachVideo.time = 0f;
            coachVideo.Play();
            StartCoroutine(TalkCoroutine());
        }

        private IEnumerator TalkCoroutine()
        {
            yield return new WaitForSeconds(7.5f);

            MakeCoachIdle();
            StopCoroutine(TalkCoroutine());
        }

        private void CheckMessage(string _userMessage)
        {
            List<Talk> _coachAnswers;
            if (LogSession.Instance.UserIndex == 0) _coachAnswers = coachAnswersProfile1;
            else _coachAnswers = coachAnswersProfile2;

            int _answersNum = _coachAnswers.Count;
            for (int i = 0; i < _answersNum; i++)
            {
                if (_userMessage == _coachAnswers[i].message)
                {
                    SendCoachMessage(_coachAnswers[i].answer);
                    break;
                }
            }
        }
    }
}


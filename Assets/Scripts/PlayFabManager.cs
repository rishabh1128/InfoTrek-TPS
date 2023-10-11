using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;
using System;

public class PlayFabManager : MonoBehaviour
{
    public static PlayFabManager instance;
    [Header("Login UI")]
    [SerializeField] private Text message;
    [SerializeField] private InputField userID;
    [SerializeField] private InputField password;
    [SerializeField] private GameObject loginScreen;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject nameInputScreen;

    public void LoginButton()
    {
        var request = new LoginWithEmailAddressRequest
        {
            Email = userID.text,
            Password = password.text,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };
        PlayFabClientAPI.LoginWithEmailAddress(request, OnLoginSuccess, OnError);
    }

    public void RegisterButton()
    {
        if(password.text.Length < 6)
        {
            message.text = "Password should be of atleast six characters!";
            return;
        }
        var request = new RegisterPlayFabUserRequest
        {
            Email = userID.text,
            Password = password.text,
            RequireBothUsernameAndEmail = false,
            InfoRequestParameters = new GetPlayerCombinedInfoRequestParams
            {
                GetPlayerProfile = true
            }
        };

        PlayFabClientAPI.RegisterPlayFabUser(request, OnRegisterSuccess, OnError);
    }

    private void OnRegisterSuccess(RegisterPlayFabUserResult res)
    {
        message.text = "Registered and logged in!";
        loginScreen.SetActive(false);
        nameInputScreen.SetActive(true);
    }

    public void ResetButton()
    {
        var request = new SendAccountRecoveryEmailRequest
        {
            Email = userID.text,
            TitleId = "2DDED"
        };
        PlayFabClientAPI.SendAccountRecoveryEmail(request, OnPasswordReset, OnError);
    }

    private void OnPasswordReset(SendAccountRecoveryEmailResult res)
    {
        message.text = "Password reset email sent!";
    }

    [Header("Leaderboard variables")]
    [SerializeField] private List<TextMeshProUGUI> pos;
    [SerializeField] private List<TextMeshProUGUI> names;
    [SerializeField] private List<TextMeshProUGUI> scores;

    [SerializeField] private Text hiScore;



    private void Start()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
   /* private void Login()
    {
        var request = new LoginWithCustomIDRequest {
            CustomId = SystemInfo.deviceUniqueIdentifier,
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSuccess, OnError);
    }*/

    private void OnError(PlayFabError playFabError)
    {
        message.text = playFabError.ErrorMessage;
        Debug.Log(playFabError.GenerateErrorReport());
    }

    private void OnLoginSuccess(LoginResult loginResult)
    {
        message.text = "Logged in!";
        Debug.Log("Successful login/account creation.");
        //Load menu
        GetMaxScore();
        loginScreen.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "PlayerScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    private void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successfully updated leaderboard.");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "PlayerScore",
            StartPosition = 0,
            MaxResultsCount = 10
        };

        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    private void OnLeaderboardGet(GetLeaderboardResult res)
    {
        int loopLength = (res.Leaderboard.Count < names.Count) ? res.Leaderboard.Count : names.Count;
        for(int i=0;i<loopLength;i++)
        {
            var item = res.Leaderboard[i];
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
            pos[i].text = (item.Position+1).ToString();
            names[i].text = item.DisplayName;
            scores[i].text = item.StatValue.ToString();
        }
    }
    public void GetMaxScore()
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnDataReceived, OnError);
    }

    private void OnDataReceived(GetUserDataResult res)
    {
        Debug.Log("User data fetched!");
        if(hiScore!=null && res.Data!=null && res.Data.ContainsKey("MaxScore"))
        {
            hiScore.text = res.Data["MaxScore"].Value;
        }
        else
        {
            Debug.Log("Player data not complete!");
        }
    }

    public void SaveMaxScore(int playerScore)
    {
        int score = Math.Max(int.Parse(hiScore.text),playerScore);
        var request = new UpdateUserDataRequest
        {
            
            Data = new Dictionary<string, string>
            {
                {"MaxScore",score.ToString()}
            }
        };
        PlayFabClientAPI.UpdateUserData(request,OnDataSend,OnError);
    }

    private void OnDataSend(UpdateUserDataResult result)
    {
        Debug.Log("Successfully sent user data!");
        GetMaxScore();
    }

    public void SubmitNameButton(string name)
    {
        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };
        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, (PlayFabError res)=> { Debug.Log(res.ErrorMessage); });
    }

    private void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult res)
    {
        Debug.Log("Successfully updated Display name!");
        mainMenu.SetActive(true);
        nameInputScreen.SetActive(false);
    }
}

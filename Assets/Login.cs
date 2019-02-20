using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Facebook.Unity;
using UnityEngine.UI;
using System.Collections.Generic;
public class Login : MonoBehaviour
{
    public Players ps = new Players();
    public GameObject DialoggedIn;
    public GameObject DialoggedOut;
    public GameObject DilageNamw;
    public GameObject DilageEmail;
    public GameObject DilageLocation;
    public GameObject DilagProfile_Pic;
    string name, email;
    void Awake()
    {
        FB.Init(SetInit, OnHideUnity);

    }
    void SetInit()
    {
        if (FB.IsLoggedIn)
        {
            Debug.Log("FB is logged in");
        }
        else
        {
            Debug.Log("FB is not logged in");

        }
        DealFBMeune(FB.IsLoggedIn);
    }
    void OnHideUnity(bool IsGameShown)
    {
        if (!IsGameShown)
        {
            Time.timeScale = 0;

        }
        else
        {
            Time.timeScale = 1;
        }
    }
    public void FBlogin()
    {
        List<string> permissions = new List<string>();
        permissions.Add("public_profile");
        permissions.Add("email");
        permissions.Add("user_location");
        FB.LogInWithReadPermissions(permissions, AuthCallBack);
    }
    void AuthCallBack(IResult result)
    {

        if (result.Error != null)
        {
            Debug.Log(result.Error);
        }
        else
        {
            if (FB.IsLoggedIn)
            {
                Debug.Log("Log  in");
            }
            else
            {
                Debug.Log(" not Log  in");
            }
            DealFBMeune(FB.IsLoggedIn);
        }
    }
    void DealFBMeune(bool isLoogedIn)
    {
        if (isLoogedIn)
        {
            DialoggedIn.SetActive(true);
            DialoggedOut.SetActive(true);

            FB.API("/me/picture?type=square&height=128&width=128", HttpMethod.GET, DisplayProfilePic);
            FB.API("/me?fields=id,name,email", HttpMethod.GET, GetFacebookInfo);
          
        }
        else
        {
            DialoggedIn.SetActive(false);
            DialoggedOut.SetActive(false);
        }
    }

    void DisplayProfilePic(IGraphResult result)
    {


        if (result.Texture != null)
        {
            Debug.Log("Eeeoe");
            Image ProfilePic = DilagProfile_Pic.GetComponent<Image>();
            ProfilePic.sprite = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());

        }

    }
   
public void GetFacebookInfo(IResult result)
    {
        Text PlayerNam = DilageNamw.GetComponent<Text>();
        Text Email = DilageEmail.GetComponent<Text>();

        if (result.Error == null)
        {
            Debug.Log(result.ResultDictionary["id"].ToString());
            Debug.Log(result.ResultDictionary["name"].ToString());
            Debug.Log(result.ResultDictionary["email"].ToString());
          
            PlayerNam.text = "Hi thera" + result.ResultDictionary["name"];
            Email.text = "Emaail" + result.ResultDictionary["email"];
           name =""+ result.ResultDictionary["name"];
            email =""+result.ResultDictionary["email"];
           
            Debug.Log("Email: "+email);
            AddPlayer();
        }
        else
        {
            Debug.Log(result.Error);
        }
    }

    public void AddPlayer()
    {
       
    StartCoroutine(Database.Instance.AddPlayer(new PlayerClass(name, email), isSuccessful =>
     {
    //  ShowPlayers();
        ps = Database.players;
      }));
        Debug.Log("Add Player");
     }


}

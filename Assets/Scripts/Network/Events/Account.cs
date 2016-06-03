using UnityEngine;
using System.Collections;
using SocketIO;

namespace Events
{
    public class Account : MonoBehaviour
    {

        // Use this for initialization
        void Start()
        {
            Network.AddListener("account:onLogin", OnLogin);
            Network.AddListener("account:onRequestCharacters", OnRequestCharacters);
            Network.AddListener("account:onSelectCharacter", OnSelectCharacter);
            Network.AddListener("account:onDeleteCharacter", OnDeleteCharacter);
            Network.AddListener("account:onEnterWorld", OnEnterWorld);
            Network.AddListener("account:onLogout", OnLogout);
        }

        #region OnLogin Handlers
        public static void OnLogin(JSONObject data = null)
        {
            string cmd = "account:onLogin";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnLogin(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            if (data["loggedIn"].b == true)
            {
                GuiManager.activeGUI = GuiManager.GUIs.CharacterSelectWindow;
                Destroy(FindObjectOfType<GuiLoginWindow>().gameObject);
            }
            else
            {
                string error = null;
                switch ((int)data["status"].n)
                {
                    case 1:
                        error = "Account Inactive";
                        break;
                    case 2:
                        error = "Username / Password Incorrect";
                        break;
                    case 3:
                        error = "Account Already Logged In";
                        break;
                }
                FindObjectOfType<GuiLoginWindow>().textStatus.text = "Status: " + error;
            }
        }
        #endregion

        #region OnRequestCharacters Handlers
        public static void OnRequestCharacters(JSONObject data = null)
        {
            string cmd = "account:onRequestCharacters";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnRequestCharacters(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            FindObjectOfType<GuiCharacterWindow>().SetupCharacters(data["characters"].list);
        }
        #endregion

        #region OnSelectCharacter Handlers
        public static void OnSelectCharacter(JSONObject data = null)
        {
            string cmd = "account:onSelectCharacter";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnSelectCharacter(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            FindObjectOfType<GuiCharacterWindow>().SelectCharacter();
        }
        #endregion

        #region OnDeleteCharacter Handlers
        public static void OnDeleteCharacter(JSONObject data = null)
        {
            string cmd = "account:onDeleteCharacter";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnDeleteCharacter(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);
            if (data["deleted"].b)
            {
                FindObjectOfType<GuiCharacterWindow>().DeleteCharacter();
            }
        }
        #endregion

        #region OnEnterWorld Handlers
        public static void OnEnterWorld(JSONObject data = null)
        {
            string cmd = "account:onEnterWorld";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnEnterWorld(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);

        }
        #endregion

        #region OnLogout Handlers
        public static void OnLogout(JSONObject data = null)
        {
            string cmd = "account:onLogout";
            if (data == null)
            {
                Network.Send(cmd);
            }
            else {
                Debug.Log("Out - " + data);
                Network.Send(cmd, data);
            }
        }
        private void OnLogout(SocketIOEvent e)
        {
            JSONObject data = e.data;
            Debug.Log("In - " + data);

            GuiCharacterWindow guiCharacterWindow = FindObjectOfType<GuiCharacterWindow>();
            guiCharacterWindow.buttonEnterWorld.gameObject.SetActive(false);
            GuiManager.activeGUI = GuiManager.GUIs.LoginWindow;

            Destroy(guiCharacterWindow.gameObject);
        }
        #endregion

    }
}

using UnityEngine;
using UnityEngine.UI;
using SocketIO;

public class GuiLoginWindow : MonoBehaviour
{

    #region Private
    private GuiManager guiManager;
    #endregion

    #region Public
    public Text textHeader;
    public Text textStatus;
    public InputField inputUser;
    public InputField inputPass;
    public Button buttonLogin;
    public Button buttonRegister;
    #endregion

    // Use this for initialization
    void Start()
    {
        print(this.name + ".Start");
        guiManager = GameObject.FindObjectOfType<GuiManager>();
        if (guiManager.tmpUser != null && guiManager.tmpPass != null)
        {
            inputUser.text = guiManager.tmpUser;
            inputPass.text = guiManager.tmpPass;
            guiManager.tmpUser = null;
            guiManager.tmpPass = null;
        }


        buttonLogin.onClick.AddListener(() => OnLoginClick());
        buttonRegister.onClick.AddListener(() => OnRegisterClick());
    }

    // Cleanup
    void OnDestroy()
    {
        buttonLogin.onClick.RemoveAllListeners();
        buttonRegister.onClick.RemoveAllListeners();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnLoginClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnLoginClick()
    {
        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("user", inputUser.text);
        data.AddField("pass", Utilities.CreateMD5(inputPass.text));
        Events.Account.OnLogin(data);
    }

    void OnRegisterClick()
    {
        GuiManager.activeGUI = GuiManager.GUIs.RegisterWindow;
        Destroy(gameObject);
    }
}
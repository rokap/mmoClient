using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;
using SocketIO;

public class GuiCharacterCreateWindow : MonoBehaviour {
    #region Private
    private GuiManager guiManager;
    //MyCharacter character;
    #endregion

    #region Public
    public Text textHeader;
    public Text textStatus;
    public InputField inputName;
    public ToggleGroup classes;
    public Button buttonCreate;
    public Button buttonCancel;
    #endregion

    // Use this for initialization
    void Start () {

        print(this.name + ".Start");
        guiManager = GameObject.FindObjectOfType<GuiManager>();

        buttonCreate.onClick.AddListener(() => OnCreateClick());
        buttonCancel.onClick.AddListener(() => OnCancelClick());

        Network.AddListener("client:createCharacter", CreateCharacter);

    }

    // Cleanup
    void OnDestroy()
    {
        buttonCreate.onClick.RemoveAllListeners();
        buttonCancel.onClick.RemoveAllListeners();
        Network.RemoveListener("client:createCharacter", CreateCharacter);
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnCreateClick();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnCancelClick();
        }

    }

    void OnCreateClick()
    {
        print(this.name + ".OnRegisterClick");

        List<string> missingFields = new List<string>();
        missingFields.ToArray();

        if (inputName.text == "")
            missingFields.Add("name");

        textStatus.text = "Please provide \n" + string.Join(", ", missingFields.ToArray());

        if (inputName.text != "")
        {
            JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
            data.AddField("name", inputName.text);
            data.AddField("class", classes.GetActive().transform.FindChild("Label").GetComponent<Text>().text);

            // Try it on server
            Network.Send("server:createCharacter", data);
        }
    }

    void OnCancelClick()
    {
        GuiManager.activeGUI = GuiManager.GUIs.CharacterSelectWindow;
        Destroy(gameObject);
    }

    void CreateCharacter(SocketIOEvent server)
    {
        Debug.Log(server);
        if (server.data["characterExists"].b == true)
        {
            textStatus.text = "Character name taken, try another";
        }
        else
        {
            textStatus.text = "Character Creation Successfull";
            GuiManager.activeGUI = GuiManager.GUIs.CharacterSelectWindow;
            Destroy(gameObject);
        }
    }
}

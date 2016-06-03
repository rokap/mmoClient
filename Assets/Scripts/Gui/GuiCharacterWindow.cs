using UnityEngine;
using UnityEngine.UI;
using SocketIO;
using System.Collections.Generic;
using Assets.Network.Structures;
using System;

public class GuiCharacterWindow : MonoBehaviour
{
    #region Private
    private GuiManager guiManager;
    //MyCharacter character;
    #endregion

    #region Public
    public Text textTotal;
    public Text textHeader;
    public Button buttonEnterWorld;
    public List<Button> buttons = new List<Button>();
    public int maxCharacters = 7;
    private Button buttonToRemove;
    #endregion

    // Use this for initialization
    void Start()
    {
        print(this.name + ".Start");
        //character = FindObjectOfType<MyCharacter>();
        guiManager = FindObjectOfType<GuiManager>();
        buttonEnterWorld.gameObject.SetActive(false);
        Events.Account.OnRequestCharacters();
    }

    // Cleanup
    void OnDestroy()
    {
        foreach (Button button in buttons)
        {
            button.onClick.RemoveAllListeners();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnLogout();
        }
    }

    public void SetupCharacters(List<JSONObject> list)
    {
        foreach (JSONObject character in list)
        {
            int id = (int)character["id"].n;
            string name = character["name"].str;
            int level = (int)character["level"].n;
            string _class = character["class"].str;

            GameObject buttonCharacter = Instantiate(Resources.Load<GameObject>("Gui/ButtonCharacter"));
            buttonCharacter.name = buttonCharacter.name.Replace("(Clone)", "").Trim();
            buttonCharacter.transform.SetParent(gameObject.transform.FindChild("Panel"));

            buttonCharacter.transform.FindChild("TextCharacter").GetComponent<Text>().text = name;
            buttonCharacter.transform.FindChild("TextInfo").GetComponent<Text>().text = string.Format("Level {0} {1}", level, _class);
            buttons.Add(buttonCharacter.GetComponent<Button>());

            buttonCharacter.GetComponent<Button>().onClick.AddListener(() => OnSelectCharacter(id, buttonCharacter.GetComponent<Button>()));
        }

        int totalCharacters = list.Count;
        textTotal.text = string.Format("Characters: {0} / {1}", totalCharacters, maxCharacters);

        if (totalCharacters < maxCharacters)
        {
            GameObject buttonCreateCharacter = Instantiate(Resources.Load<GameObject>("Gui/ButtonCreateCharacter"));
            buttonCreateCharacter.name = buttonCreateCharacter.name.Replace("(Clone)", "").Trim();
            buttonCreateCharacter.transform.SetParent(gameObject.transform.FindChild("Panel"));
            buttonCreateCharacter.GetComponent<Button>().onClick.AddListener(() => OnCharacterCreate());
            buttons.Add(buttonCreateCharacter.GetComponent<Button>());
        }
    }
    public void DeleteCharacter()
    {
        buttons.Remove(buttonToRemove);
        Destroy(buttonToRemove.transform.parent.gameObject);
        //character.Clear();
        buttonEnterWorld.gameObject.SetActive(false);
    }
    #region Local Button Listeners

    void OnCharacterCreate()
    {
        GuiManager.activeGUI = GuiManager.GUIs.CharacterCreateWindow;
        buttonEnterWorld.gameObject.SetActive(false);
        Destroy(gameObject);
    }

    void OnDeleteCharacter(int id, Button button)
    {
        buttonToRemove = button;
        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("id", id);
        Events.Account.OnDeleteCharacter(data);
        button.onClick.RemoveListener(() => OnDeleteCharacter(id, button));
    }

    void OnSelectCharacter(int id, Button button)
    {
        foreach (Button otherbutton in buttons)
        {
            if (otherbutton != null)
            {
                if (otherbutton.name != "ButtonCreateCharacter" && otherbutton.name != "ButtonEnterWorld")
                {
                    otherbutton.transform.FindChild("ButtonDelete").gameObject.SetActive(false);
                    otherbutton.transform.FindChild("ButtonDelete").GetComponent<Button>().onClick.RemoveAllListeners();
                }
            }
        }
        button.transform.FindChild("ButtonDelete").gameObject.SetActive(true);
        Button deleteButton = button.transform.FindChild("ButtonDelete").GetComponent<Button>();

        deleteButton.onClick.AddListener(() => OnDeleteCharacter(id, deleteButton));

        JSONObject data = new JSONObject(JSONObject.Type.OBJECT);
        data.AddField("id", id);
        Events.Account.OnSelectCharacter(data);
    }

    void OnEnterWorld()
    {
        Events.Account.OnEnterWorld();
    }

    void OnLogout()
    {
        Events.Account.OnLogout();
    }

    #endregion
    

    

    public void SelectCharacter()
    {
        buttonEnterWorld.onClick.RemoveAllListeners();
        /*
        character.netID = json["netID"].str;
        character._id = (int)json["id"].n;
        character._name = json["name"].str;
        character._class = json["class"].str;
        character._race = json["race"].str;
        character._gender = json["gender"].str;
        character._level = (int)json["level"].n;
        character._pos = new Vector3(json["posX"].f, json["posY"].f, json["posZ"].f);
        character._rot = json["rot"].f;
        */
        buttonEnterWorld.gameObject.SetActive(true);
        buttonEnterWorld.onClick.AddListener(() => OnEnterWorld());
    }

    void EnterWorld(SocketIOEvent server)
    {

        JSONObject json = server.data;
        Debug.Log(json);
        if (json["canEnter"].b)
        {
/*
            World world = GameObject.FindObjectOfType<World>();
            NetCharacter netCharacter = new NetCharacter(
                character.netID,
                character._id,
                character._name,
                character._class,
                character._race,
                character._gender,
                character._level,
                character._pos,
                character._rot
            );
            world.netCharacters.AddCharacter(netCharacter);

            */
            GuiManager.activeGUI = GuiManager.GUIs.Game;
            Destroy(gameObject);

        }
    }

    void Logout(SocketIOEvent obj)
    {
        Debug.Log(obj.data);
        //character.Clear();
    }

}

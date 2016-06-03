using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class guiMain : MonoBehaviour
{

    public Text versionText;
    private bool versionInitialized = false;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!versionInitialized)
        {
            string versionTextFileNameAndPath = "version.txt";
            string version = CommonUtils.ReadTextFile(versionTextFileNameAndPath);
            versionText.text = string.Format("Version: {0}", version.Replace("\n", string.Empty));
            versionInitialized = true;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class PlayerSelecter : MonoBehaviour {

    public GameObject logInPanel;
    public RawImage introductionImage;
    public GameObject playerList;
    public GameObject playerBlockPrefab;
    private InputField inputField;
    private Button confirmButton;
    private string mSelectedCharacterName = "";

	// Use this for initialization
	void Start () {
        inputField = logInPanel.GetComponentInChildren<InputField>();
        confirmButton = logInPanel.GetComponentInChildren<Button>();
        confirmButton.onClick.AddListener(delegate ()
        {
            if(!inputField.text.Equals(""))
            {
                AddPlayer(inputField.text);
            }
            inputField.text = "";
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public void SelecterCharacter(string characterName)
    {
        if (mSelectedCharacterName.Equals(""))
        {
            mSelectedCharacterName = characterName;
            logInPanel.SetActive(true);
            logInPanel.GetComponentInChildren<RawImage>().texture = Resources.Load(characterName) as Texture;
        }
    }

    public void AddPlayer(string playerName)
    {
        PlayerImformation.Instance.AddPlayer(new PlayerData(mSelectedCharacterName, playerName));
        logInPanel.SetActive(false);
        GameObject playerBlock = Instantiate(playerBlockPrefab) as GameObject;
        playerBlock.GetComponentInChildren<RawImage>().texture = Resources.Load(mSelectedCharacterName) as Texture;
        playerBlock.GetComponentInChildren<Text>().text = playerName;
        playerBlock.transform.SetParent(playerList.transform);
        mSelectedCharacterName = "";
    }

    public void ChangeIntroduction(string introductionName)
    {
        introductionImage.texture = Resources.Load(introductionName) as Texture;
    }

    public void StartGameScene()
    {
        if (PlayerImformation.Instance.PlayerList.Count > 0)
            SceneManager.LoadScene("UI");
    }

}

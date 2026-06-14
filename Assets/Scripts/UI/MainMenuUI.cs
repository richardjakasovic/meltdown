using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Threading.Tasks;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button hostButton;
    [SerializeField] private Button joinButton;
    [SerializeField] private TMP_InputField lobbyCodeInput;
    [SerializeField] private TextMeshProUGUI statusText;

    void Start()
    {
        hostButton.onClick.AddListener(OnHostClicked);
        joinButton.onClick.AddListener(OnJoinClicked);
    }

    async void OnHostClicked()
    {
        SetStatus("Creating lobby...");
        hostButton.interactable = false;
        joinButton.interactable = false;

        await GameNetworkManager.Instance.StartHost();

        SetStatus("Lobby code: " + GameNetworkManager.Instance.GetLobbyCode());
    }

    async void OnJoinClicked()
    {
        string code = lobbyCodeInput.text.Trim().ToUpper();

        if (string.IsNullOrEmpty(code))
        {
            SetStatus("Enter a lobby code first!");
            return;
        }

        SetStatus("Joining...");
        hostButton.interactable = false;
        joinButton.interactable = false;

        await GameNetworkManager.Instance.StartClient(code);

        SetStatus("Joined!");
    }

    void SetStatus(string message)
    {
        if (statusText != null)
            statusText.text = message;
    }
}
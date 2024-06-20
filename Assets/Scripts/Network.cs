using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;

public class Network : MonoBehaviour
{
    public TextMeshProUGUI statusText;
    public InputField ipInput;
    public Button connectButton;

    private void Start()
    {
        connectButton.onClick.AddListener(ConnectToHost);
    }

    private void ConnectToHost()
    {
        string ipAddress = "127.0.0.1";
        NetworkManager.Singleton.GetComponent<UnityTransport>().ConnectionData.Address = ipAddress;
        NetworkManager.Singleton.StartClient();
    }

    private void Update()
    {
        if (NetworkManager.Singleton.IsConnectedClient)
        {
            statusText.text = "Подключено к хосту";
        }
        else
        {
            statusText.text = "Не подключено";
        }
    }
}

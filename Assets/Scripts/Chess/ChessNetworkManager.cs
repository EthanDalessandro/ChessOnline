using System.Collections;
using System.Collections.Generic;
using PlayerIOClient;
using UnityEngine;

public class ChessNetworkManager : MonoBehaviour
{
    public static ChessNetworkManager Instance;
    public ChessBoard chessBoard;

    private Connection pioconnection;
    private List<Message> msgList = new List<Message>();

    public string localTeam = ""; // "White" or "Black"
    public bool isGameReady = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        if (chessBoard == null)
        {
            chessBoard = FindObjectOfType<ChessBoard>();
            if (chessBoard == null)
            {
                Debug.LogError("ChessBoard not found in scene!");
            }
        }

        Application.runInBackground = true;
        Connect();
    }

    void Connect()
    {
        System.Random random = new System.Random();
        string userid = "Player" + random.Next(0, 10000);

        PlayerIO.Authenticate(
            "chessonline-2xyyfrmdnuipfyqvw9idpg",
            "public",
            new Dictionary<string, string> { { "userId", userid } },
            null,
            delegate (Client client)
            {
                Debug.Log("Connected to PlayerIO");
                // Use local dev server
                client.Multiplayer.DevelopmentServer = new ServerEndpoint("localhost", 8184);

                client.Multiplayer.CreateJoinRoom(
                    "ChessRoom",
                    "MyGame",
                    true,
                    null,
                    null,
                    delegate (Connection connection)
                    {
                        Debug.Log("Joined Room");
                        pioconnection = connection;
                        pioconnection.OnMessage += handlemessage;
                    },
                    delegate (PlayerIOError error)
                    {
                        Debug.Log("Error Joining Room: " + error.ToString());
                    }
                );
            },
            delegate (PlayerIOError error)
            {
                Debug.Log("Error connecting: " + error.ToString());
            }
        );
    }

    void handlemessage(object sender, Message m)
    {
        msgList.Add(m);
    }

    public void SendMove(int ox, int oy, int nx, int ny)
    {
        if (pioconnection != null)
        {
            pioconnection.Send("Move", ox, oy, nx, ny);
        }
    }

    private bool isLocalReady = false;

    public void SendPlayerReady()
    {
        if (pioconnection != null)
        {
            pioconnection.Send("PlayerReady");
            isLocalReady = true;
        }
    }

    void OnGUI()
    {
        if (pioconnection != null)
        {
            if (!isGameReady)
            {
                if (!isLocalReady)
                {
                    if (GUI.Button(new Rect(10, 10, 150, 50), "Ready"))
                    {
                        SendPlayerReady();
                    }
                }
                else
                {
                    GUI.Label(new Rect(10, 10, 200, 20), "Waiting for other player...");
                }
            }
            else
            {
                GUI.Label(new Rect(10, 10, 200, 20), "Game Started! Team: " + localTeam);
            }
        }
        else
        {
            GUI.Label(new Rect(10, 10, 200, 20), "Connecting...");
        }
    }

    void FixedUpdate()
    {
        foreach (Message m in msgList)
        {
            switch (m.Type)
            {
                case "GameStart":
                    localTeam = m.GetString(0);
                    Debug.Log("You are team: " + localTeam);
                    break;

                case "GameReady":
                    isGameReady = true;
                    Debug.Log("Game Ready! Start playing.");
                    break;

                case "PlayerLeft":
                    Debug.Log("Player left the game.");
                    isGameReady = false;
                    localTeam = "";
                    pioconnection = null;
                    chessBoard = null;
                    msgList.Clear();
                    break;

                case "Move":
                    int ox = m.GetInt(0);
                    int oy = m.GetInt(1);
                    int nx = m.GetInt(2);
                    int ny = m.GetInt(3);

                    if (chessBoard != null)
                    {
                        chessBoard.MovePieceRemote(ox, oy, nx, ny);
                    }
                    break;
                case "Error":
                    Debug.LogError("Server Error: " + m.GetString(0));
                    break;
            }
        }
        msgList.Clear();
    }
}

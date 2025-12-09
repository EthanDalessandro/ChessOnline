using System;
using System.Collections.Generic;
using PlayerIO.GameLibrary;

namespace CleanPlayerIOServer {
	[RoomType("MyGame")]
	public class GameCode : Game<Player> 
	{
		private bool gameStarted = false;
		private string currentTurn = "White";

		public override void GameStarted() 
		{
			Console.WriteLine("Game is started: " + RoomId);
		}

		public override void GameClosed() 
		{
			Console.WriteLine("RoomId: " + RoomId);
		}

        public override void UserJoined(Player player) 
		{
			Console.WriteLine("UserJoined: " + player.Id);
            int playerCount = 0;
            foreach (Player p in Players) { playerCount++; }

            if (playerCount > 2)
            {
                player.Send("Error", "Room is full");
                player.Disconnect();
                return;
            }
            
		}

		public override void GotMessage(Player player, Message message) 
		{
			switch(message.Type) 
			{
                case "PlayerReady":
                    Console.WriteLine("DEBUG: Received PlayerReady from " + player.Id);
                    if (gameStarted) 
                    {
                        Console.WriteLine("DEBUG: Game already started, ignoring.");
                        return;
                    }
                    player.IsReady = true;
                    
                    int readyCount = 0;
                    foreach(Player p in Players) {
                        if (p.IsReady) readyCount++;
                    }
                    Console.WriteLine("DEBUG: Ready Count: " + readyCount);

                    if (readyCount == 2)
                    {
                        gameStarted = true;
                        bool assignWhite = true;
                        foreach(Player p in Players)
                        {
                            string team = assignWhite ? "White" : "Black";
                            p.Send("GameStart", team);
                            assignWhite = false;
                        }

                        Broadcast("GameReady");
                        Console.WriteLine("Game Ready! Two players ready.");
                    }
                    break;

				case "Move":
					if (!gameStarted) 
						return;

					bool isWhite = false;
					int index = 0;
					foreach(Player p in Players) {
						if (p.Id == player.Id) {
							if (index == 0) isWhite = true;
							break;
						}
						index++;
					}

					string playerTeam = isWhite ? "White" : "Black";

					if (playerTeam != currentTurn)
					{
						player.Send("Error", "Not your turn");
						return;
					}

					int originalX = message.GetInt(0);
					int originalY = message.GetInt(1);
					int targetX = message.GetInt(2);
					int targetY = message.GetInt(3);

					Broadcast("Move", originalX, originalY, targetX, targetY);

					currentTurn = (currentTurn == "White") ? "Black" : "White";
					break;
			}
		}
	}
}

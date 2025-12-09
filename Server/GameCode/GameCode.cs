using System;
using System.Collections.Generic;
using PlayerIO.GameLibrary;

namespace CleanPlayerIOServer {
	[RoomType("MyGame")]
	public class GameCode : Game<Player> 
	{
		// This method is called when an instance of your the game is created
		public override void GameStarted() 
		{
			// anything you write to the Console will show up in the 
			// output window of the development server
			Console.WriteLine("Game is started: " + RoomId);
		}

		// This method is called when the last user leaves the room 
		// and it's closed down.
		public override void GameClosed() 
		{
			Console.WriteLine("RoomId: " + RoomId);
		}

		// This method is called whenever a player joins the game
		public override void UserJoined(Player player) 
		{
			Console.WriteLine("UserJoined: " + player.Id);
            foreach(Player p in Players) 
            {
                if(p.Id != player.Id) 
                {
                    p.Send("UserJoined", player.Id);
                    player.Send("UserJoined", p.Id);
                }
            }
		}

		// This method is called when a player leaves the game
		public override void UserLeft(Player player) 
		{
			Console.WriteLine("UserLeft: " + player.Id);
            Broadcast("UserLeft", player.Id);
		}

		// This method is called when a player sends a message into the server code
		public override void GotMessage(Player player, Message message) 
		{
			switch(message.Type) 
			{
				// This is how you would set a player's name when they send in their name in a 
				// "Hello" message type.
				case "Hello":
					player.Send("Hello", "World");
					break;
			}
		}
	}
}

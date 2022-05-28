using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Desktop
{
    public static class UI
    {
        public static int GetPlayerResponse()
        {
            int responseCode = -1;

            ConsoleKey keyResponse;
            do
            {
                Console.WriteLine();
                Console.WriteLine("Please choose your next action");
                Console.Write("H - Hit / S - Stand / D - Double Down / Q - Surrender ");
                keyResponse = Console.ReadKey(false).Key;
                Console.WriteLine();

                responseCode = PlayerResponseCode(keyResponse);

            } while (responseCode < 0);



            return responseCode;
        }

        private static int PlayerResponseCode(ConsoleKey response) 
        {
            int responseCode = -1;

            switch (response)
            {
                case ConsoleKey.H:
                    responseCode = 0;
                    break;
                case ConsoleKey.S:
                    responseCode = 1;
                    break;
                case ConsoleKey.D:
                    responseCode = 2;
                    break;
                case ConsoleKey.Q:
                    responseCode = 3;
                    break;
                default:
                    responseCode = -1;
                    break;
            }

            return responseCode;
        }

        public static int GetPlayerBet(int position, string name, int money)
        {
            Console.WriteLine();
            Console.WriteLine("Player " + position + ", " + name + ", " +
                "please enter your wager. Your balance is " + money + " dollars");

            bool isValidAnswer = false;
            int bet = -1;
            while (!isValidAnswer)
            {
                try
                {
                    bet = int.Parse((Console.ReadLine() ?? ""));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

                if (bet > money || bet < 0)
                {
                    Console.WriteLine("You entered an invalid amount " + bet + " dollars. " +
                        "Your balance is " + money + " dollars. Enter a valid amount");
                }
                else
                {
                    isValidAnswer = true;
                }
            }
            return bet;
        }

        public static void PlayerResponseError()
        {
            Console.WriteLine("Your response was not valid. Please try again.");
        }

        public static void EndOfShoe()
        {
            Console.WriteLine("Warning: End of Shoe, forced Shuffle");
        }

        public static void TargetPenetrationWarning()
        {
            Console.WriteLine("Warning: Target Penetration is very high");
        }

        public static void YouWonMessage(string name, int winnings)
        {
            Console.WriteLine(name + ", You Won " + winnings + " dollars!");
        }

        public static void TieMessage(string name, int bet)
        {
            Console.WriteLine(name + ", You tied and got " + bet + " dollars back.");
        }

        public static void YouLostMessage(string name, int bet)
        {
            Console.WriteLine(name + ", You lost " + bet + " dollars...");
        }

        public static bool AskToEndGame()
        {
            Console.WriteLine();
            Console.WriteLine("Would you like to end the game?");
            return YesOrNo();
        }

        public static void EndingGame(List<Player> players)
        {
            Console.WriteLine("The final winnings are:");
            foreach (Player player in players)
            {
                Console.WriteLine(player.Name + " with a balance of " + player.Money + " dollars");
            }
            Console.WriteLine();
        }

        public static Player GetNewPlayer()
        {
            string name = GetNewPlayerName();
            int money = GetNewPlayerWallet(name);

            return new Player(name, money);
        }

        private static string GetNewPlayerName()
        {
            bool isConfirmName = false;
            string name = "";
            Console.WriteLine();
            do
            {
                Console.WriteLine("Please enter your name");
                name = (Console.ReadLine() ?? "");
                Console.WriteLine("Is this your name? : " + name);
                isConfirmName = YesOrNo();
            } while (!isConfirmName);

            return name;
        }

        private static int GetNewPlayerWallet(string name)
        {
            Console.WriteLine();
            Console.WriteLine(name + ", please enter your buy-in amount");

            bool isValidAnswer = false;
            int balance = -1;
            while (!isValidAnswer)
            {
                try
                {
                    balance = int.Parse((Console.ReadLine() ?? ""));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

                if (balance < 0 || balance > 100000)
                {
                    Console.WriteLine("You entered an invalid amount " + balance + " dollars. " +
                        "The maximum buy-in is 100000 dollars. Enter a valid amount");
                }
                else
                {
                    isValidAnswer = true;
                }
            }
            return balance;
        }

        public static void ShowCards(string name, int position, string cards, int score)
        {
            Console.WriteLine("Player " + position + ": " + name + " has the following card(s): " + cards + "with a score of: " + score);
        }

        public static void DisplayTakenPositions(List<int> positions)
        {
            string takenPositions = "";
            foreach (int position in positions)
            {
                takenPositions += position + ", ";
            }
            takenPositions = takenPositions.Substring(0, takenPositions.Length - 2);
            Console.WriteLine("Currently occupied position(s) are: " + takenPositions);
        }

        public static int GetNewPlayerPosition(List<int> positions)
        {
            Console.WriteLine("Please enter an empty seat number you would like to take");

            bool isValidAnswer = false;
            int position = -1;
            while (!isValidAnswer) 
            {
                try
                {
                    position = int.Parse((Console.ReadLine() ?? ""));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

                if (position < 0 || positions.Where(i => i == position).Count() > 0)
                {
                    Console.WriteLine("You entered an invalid seat number " + position + ", try again");
                }
                else
                {
                    isValidAnswer = true;
                }
            }
            return position;
        }

        public static int GetRemovePlayerPosition(List<int> positions)
        {
            Console.WriteLine("Please player number you would like to remove");

            bool isValidAnswer = false;
            int position = -1;
            while (!isValidAnswer)
            {
                try
                {
                    position = int.Parse((Console.ReadLine() ?? ""));
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error: " + e.Message);
                }

                if (positions.Where(i => i == position).Count() == 0 || position <= 0)
                {
                    Console.WriteLine("You entered an invalid seat number " + position + ", try again");
                }
                else
                {
                    isValidAnswer = true;
                }
            }
            return position;
        }

        public static void PositionAlreadyTaken()
        {
            Console.WriteLine("This seat is already taken, could not add new player");
        }

        public static bool YesOrNo()
        {
            ConsoleKey keyResponse;
            do
            {
                Console.Write("Y - Yes / N - No ");
                keyResponse = Console.ReadKey(false).Key;
                Console.WriteLine();
            } while (keyResponse != ConsoleKey.Y && keyResponse != ConsoleKey.N);
            return keyResponse == ConsoleKey.Y;
        }

        public static bool AddAnotherPlayer()
        {
            Console.WriteLine("Would you like to add another player?");
            return YesOrNo();
        }

        public static bool RemoveAnotherPlayer()
        {
            Console.WriteLine("Would you like to remove another player?");
            return YesOrNo();
        }

        public static bool AskToAddPlayer()
        {
            Console.WriteLine("Would you like to add players?");
            return YesOrNo();
        }

        public static bool AskToRemovePlayers()
        {
            Console.WriteLine("Would you like to remove players?");
            return YesOrNo();
        }

        public static Player? RemovedPlayer(Player? removedPlayer, int targetPosition)
        {
            Console.WriteLine();
            if (removedPlayer != null)
            {
                Console.WriteLine("Removed Player " + targetPosition + ", " + removedPlayer.Name);
            }
            else
            {
                Console.WriteLine("Could not remove player at seat " + targetPosition);
            }

            return removedPlayer;
        }

        public static void ListPlayers(List<Player> players)
        {
            foreach (Player player in players)
            {
                Console.WriteLine("The Players are:");
                Console.WriteLine(player.Name + " with " + player.Money + " dollars!");
                Console.WriteLine();
            }
        }
    }
}

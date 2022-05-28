using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack_Desktop
{
    class Table
    {
        List<Player> players;
        List<TablePosition> positions;
        Shoe shoe;
        double payoutMultiplier;

        public Table()
        {
            this.shoe = new Shoe(8, 0.3);
            this.players = new List<Player>();
            this.positions = new List<TablePosition>();
            this.payoutMultiplier = 1;

            fillTable();
        }

        public Table(List<Player> players)
        {
            this.shoe = new Shoe(8, 0.3);
            this.players = players;
            this.positions = new List<TablePosition>();
            this.payoutMultiplier = 1;

            fillTable();
        }

        public Table(List<Player> players, int decksInShoe, double targetPenetration, double payoutMultiplier)
        {
            this.shoe = new Shoe(decksInShoe, targetPenetration);
            this.players = players;
            this.positions = new List<TablePosition>();
            this.payoutMultiplier = payoutMultiplier;

            fillTable();
        }

        public void fillTable()
        {
            int position = 1;
            foreach (Player player in players)
            {
                positions.Add(new TablePosition(player, null, position, shoe));
                position++;
            }

            while (players.Count <= 0)
            {
                bool isAddingPlayers = true;
                while (isAddingPlayers)
                {
                    if (players.Count > 0)
                    {
                        UI.DisplayTakenPositions(TakenPositions());
                    }
                    int playerPosition = UI.GetNewPlayerPosition(TakenPositions());
                    int isPlayerAdded = AddPlayer(playerPosition, UI.GetNewPlayer());
                    if (isPlayerAdded < 0)
                    {
                        UI.PositionAlreadyTaken();
                    }
                    isAddingPlayers = UI.AddAnotherPlayer();
                }
            }

            positions.Add(new TablePosition(null, new Dealer(), 0, shoe));
            positions = positions.OrderByDescending(i => i.position).ToList();
        }

        public void playGame()
        {
            bool isEndGame = false;
            while (!isEndGame)
            {
                UI.ListPlayers(players);

                AddAndRemovePlayers();
                DealCards();
                PlayRound();
                isEndGame = UI.AskToEndGame();
                Console.WriteLine();
            }

            UI.EndingGame(positions.Where(i => i.player != null).Select(i => i.player ?? new Player("error", 0)).ToList());
        }

        public void AddAndRemovePlayers()
        {
            bool isAddingPlayers = UI.AskToAddPlayer();
            while (isAddingPlayers)
            {
                UI.DisplayTakenPositions(TakenPositions());
                int playerPosition = UI.GetNewPlayerPosition(TakenPositions());
                int isPlayerAdded = AddPlayer(playerPosition, UI.GetNewPlayer());
                if (isPlayerAdded < 0)
                {
                    UI.PositionAlreadyTaken();
                }
                Console.WriteLine();
                isAddingPlayers = UI.AddAnotherPlayer();
            }

            bool isRemovingPlayers = UI.AskToRemovePlayers();
            while (isRemovingPlayers)
            {
                UI.DisplayTakenPositions(TakenPositions());
                int playerPosition = UI.GetRemovePlayerPosition(TakenPositions());
                RemovePlayer(playerPosition);
                Console.WriteLine();
                isRemovingPlayers = UI.RemoveAnotherPlayer();
            }

            foreach(TablePosition position in positions)
            {
                if (position != null && (position?.player?.Money ?? 0) <= 0 && position?.position > 0)
                {
                    RemovePlayer(position.position);
                }
            }

            Console.WriteLine();
        }

        public void DealCards()
        {
            for (int i = 0; i < 2; i++) {
                foreach (TablePosition position in positions)
                {
                    position.AddCard(shoe.playCard());
                }
            }
            foreach(TablePosition position in positions)
            {
                position.ShowCards();
            }
        }

        public void PlayRound()
        {
            int dealerScore = 0;
            foreach (TablePosition position in positions)
            {
                if (position.player != null && position.position != 0)
                {
                    position.Bet();
                    while (!position.isTurnOver)
                    {
                        position.PlayerRound();
                    }
                }
                else
                {
                    dealerScore = position.DealerLoop();
                }
            }

            foreach (TablePosition position in positions)
            {
                if (position.player != null)
                {
                    position.NextRound(dealerScore, payoutMultiplier);
                }
            }

            foreach (TablePosition position in positions)
            {
                position.hand.ClearHand();
                position.isTurnOver = false;
            }
        }

        public List<int> TakenPositions()
        {
            List<int> takenPositions = new List<int>();
            foreach (TablePosition position in positions)
            {
                takenPositions.Add(position.position);
            }
            return takenPositions;
        }

        public int AddPlayer(int targetPosition, Player newPlayer)
        {
            Player? atPosition = positions.FirstOrDefault(i => i.position == targetPosition)?.player;

            if (atPosition != null)
            {
                return -1;
            }
            else
            {
                positions.Add(new TablePosition(newPlayer, null, targetPosition, shoe));
                positions = positions.OrderByDescending(i => i.position).ToList();
                return targetPosition;
            }
            
        }

        public Player? RemovePlayer(int targetPosition)
        {
            TablePosition? atPosition = positions.FirstOrDefault(i => i.position == targetPosition);

            if (atPosition != null && atPosition.position > 0)
            {
                positions.Remove(atPosition);
            }

            return UI.RemovedPlayer(atPosition?.player, targetPosition);
        }

        class Dealer
        {
            public string Name { get; set; }

            public Dealer()
            {
                Name = "Dealer";
            }
        }

        class Hand
        {
            private List<Card> hand { get; set; }
            public int total;
            public int cardCount;

            public Hand()
            {
                hand = new List<Card>();
            }

            public void AddCard(Card card)
            {
                hand.Add(card);
                sumCards();
            }

            public void ClearHand()
            {
                hand.Clear();
                total = 0;
            }

            public override string ToString()
            {
                string cardString = "";
                foreach (Card card in hand)
                {
                    cardString += card.name + " ";
                }
                return cardString;
            }

            private int sumCards()
            {
                total = 0;
                int highAces = findAces();

                foreach (Card card in hand)
                {
                    total += (int) card.rank;
                }
                while (total > 21 && highAces > 0)
                {
                    total -= 10;
                    highAces--;
                }

                return total;
            }

            private int findAces()
            {
                return hand.Where(i => i.rank == Card.Rank.Ace).Count();
            }
        }

        class TablePosition
        {
            public Player? player;
            public Dealer? dealer;
            public Hand hand;
            public Shoe shoe;
            public int bet;
            public int position;
            public bool isTurnOver;

            public enum PlayerResponse
            {
                Hit = 0,
                Stand = 1,
                DoubleDown = 2,
                Surrender = 3
            }

            public TablePosition(Player? player, Dealer? dealer, int position, Shoe shoe)
            {
                this.player = player;
                this.dealer = dealer;
                this.shoe = shoe;
                hand = new Hand();
                bet = 0;
                this.position = position;
                isTurnOver = false;
            }

            public void AddCard(Card card)
            {
                hand.AddCard(card);
            }

            public void Bet()
            {
                int responseError = 1;
                while (responseError > 0)
                {
                    responseError--;
                    int playerBet = UI.GetPlayerBet(position, player?.Name ?? "", player?.Money ?? 0);

                    if (playerBet > player?.Money)
                    {
                        responseError = 2;
                    }
                    else if (player != null)
                    {
                        player.Money -= playerBet;
                        bet = playerBet;
                    }
                    else
                    {
                        responseError = 3;
                    }
                }
            }

            public void PlayerRound()
            {
                int responseError = 1;
                while (responseError > 0) {
                    responseError--;
                    PlayerResponse response = (PlayerResponse)UI.GetPlayerResponse();

                    switch (response)
                    {
                        case PlayerResponse.Hit:
                            Hit();
                            break;
                        case PlayerResponse.Stand:
                            Stand();
                            break;
                        case PlayerResponse.DoubleDown:
                            responseError = DoubleDown();
                            break;
                        case PlayerResponse.Surrender:
                            responseError = Surrender();
                            break;
                    }

                    if (responseError > 0)
                    {
                        UI.PlayerResponseError();
                    }
                }
            }

            public void Hit()
            {
                hand.AddCard(shoe.playCard());
                if (hand.total >= 21)
                {
                    EndTurn();
                }
                ShowCards();
            }

            public void Stand()
            {
                EndTurn();
            }

            public int DoubleDown()
            {
                int responseCode = 1;
                if (bet > (player?.Money ?? 0))
                {
                    responseCode = 2;
                }
                else
                {
                    player.Money -= bet;
                    bet += bet;
                    responseCode = 0;
                    hand.AddCard(shoe.playCard());
                    EndTurn();
                    ShowCards();
                }

                return responseCode;
            }

            public int Surrender()
            {
                int responseCode = 0;
                if ((hand?.cardCount ?? 0) == 2)
                {
                    player.Money += bet;
                    bet = 0;
                    EndTurn();
                }
                else
                {
                    responseCode = 3;
                }
                return responseCode;
            }

            public void EndTurn()
            {
                isTurnOver = true;
            }

            public void NextRound(int dealerScore, double payoutMultiplier)
            {
                if ((dealerScore > hand.total && dealerScore <= 21) || hand.total > 21)
                {
                    UI.YouLostMessage(player.Name, bet);
                }
                else if (dealerScore == hand.total)
                {
                    player.Money += bet;
                    UI.TieMessage(player.Name, bet);
                }
                else if (dealerScore < hand.total)
                {
                    player.Money += (int)(bet * payoutMultiplier) + bet;
                    UI.YouWonMessage(player.Name, (int)(bet * payoutMultiplier));
                }
                bet = 0;
            }

            public void ShowCards()
            {
                UI.ShowCards((player?.Name ?? "The Dealer"), position, hand.ToString(), hand.total);
            }

            public int DealerLoop()
            {
                Console.WriteLine();
                while (hand.total < 17)
                {
                    hand.AddCard(shoe.playCard());
                }
                ShowCards();
                if (hand.total > 21)
                {
                    hand.total = 0;
                }
                return hand.total;
                Console.WriteLine();
            }
        }

    }
}

namespace BlackjackSimulator.Cli;

public class Game
{
    public void Run()
    {
        ConsoleKeyInfo? key = null;

        var table = ConfigureTable();
        
        do
        {
            var gameCount = key is { Key: ConsoleKey.Spacebar }
                ? 1000
                : 1;

            do
            {
                Console.Clear();

                UI.WriteTableStats(table);

                table.StartNewGame();

                UI.WriteGameResult(table);

                gameCount--;
            }
            while (gameCount > 0);

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.Write("Press space to run 1000 games, enter to run a single game, or escape to quit...");
            Console.ResetColor();

            key = Console.ReadKey();
        }
        while (key.Value.Key != ConsoleKey.Escape);
    }

    private Table ConfigureTable()
    {
        var players = new[]
        {
            new Player(name: "P1", money: 10000, isCardCounting: true),
            new Player(name: "P2", money: 10000, isCardCounting: true),
            new Player(name: "P3", money: 10000, isCardCounting: false),
            new Player(name: "P4", money: 10000, isCardCounting: false)
        };

        return new Table(
            deckCount: 6, 
            minimumBet: 10, 
            players);
    }
}
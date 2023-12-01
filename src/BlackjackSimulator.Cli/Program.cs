namespace BlackjackSimulator.Cli;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        var table = new Table(deckCount: 6, minimumBet: 10, playersWhoCount: 3, n00bsCount: 3);
        table.Start();
    }
}
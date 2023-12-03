namespace BlackjackSimulator.Cli;

internal abstract class Program
{
    private static void Main(string[] args)
    {
        var game = new Game();
        game.Run();
    }
    
    // total winnings not correct when lots of splits
}
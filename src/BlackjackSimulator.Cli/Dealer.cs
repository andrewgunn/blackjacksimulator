using System.Collections.Generic;
using System.Linq;

namespace BlackjackSimulator.Cli;

public class Dealer
{
    public bool CanTakeAnotherCard => !Hand.IsBust && Hand.Value < 17;
    public Hand Hand { get; private set; } = new(bet: 0);

    public Result CalculateResult(Hand hand)
    {
        if (hand.IsBlackjack && !Hand.IsBlackjack)
        {
            return Result.Blackjack;
        }

        if (hand.IsBust)
        {
            return Result.Loss;
        }

        if  (Hand.IsBust)
        {
            return Result.Win;
        }

        if (hand == Hand)
        {
            return Result.Push;
        }

        return hand > Hand
            ? Result.Win
            : Result.Loss;
    }

    public void ClearHands(Shoe shoe, 
        IEnumerable<Player> players)
    {
        shoe.DisposeHand(Hand);
        Hand = new Hand(0);

        foreach (var player in players)
        {
            player.ClearHands(shoe);
        }
    }

    public void Deal(Shoe shoe, 
        IReadOnlyCollection<Player> players)
    {
        foreach (var player in players)
        {
            foreach (var hand in player.Hands.Where(h => h.HasBet))
            {
                hand.AddCard(shoe.TakeNextCard());
            }
        }

        Hand.AddCard(shoe.TakeNextCard());

        foreach (var player in players)
        {
            foreach (var hand in player.Hands.Where(h => h.HasBet))
            {
                hand.AddCard(shoe.TakeNextCard());
            }
        }
    }

    public static void DealToHand(Shoe shoe, Hand hand)
    {
        if (!hand.CanTakeAnotherCard)
        {
            return;
        }

        hand.AddCard(shoe.TakeNextCard());
    }

    public void DealToSelf(Shoe shoe)
    {
        if (!CanTakeAnotherCard)
        {
            return;
        }

        Hand.AddCard(shoe.TakeNextCard());
    }
}
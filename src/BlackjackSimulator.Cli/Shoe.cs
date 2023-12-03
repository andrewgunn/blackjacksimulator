using System;
using System.Collections.Generic;
using System.Linq;

namespace BlackjackSimulator.Cli;

public class Shoe
{
    private readonly List<Card> _cards;
    private readonly List<Card> _disposedCards;

    public Shoe(IEnumerable<Deck> decks)
    {
        //var cutCard = (int)Math.Ceiling(decks.SelectMany(c => c.Cards).Count() * .75);

        //_cards = decks.SelectMany(d => d.Cards)
        //     .Take(cutCard)
        //     .ToList();
        //_disposedCards = decks.SelectMany(d => d.Cards)
        //     .Skip(cutCard)
        //     .ToList();

        _cards = decks.SelectMany(d => d.Cards)
            .ToList();
        _disposedCards = new List<Card>();
    }

    public IReadOnlyCollection<Card> Cards => _cards;
    public double DeckCount => _cards.Count < 52 ? 1 : Math.Round(_cards.Count / 52d * 2, MidpointRounding.AwayFromZero) / 2;
    public int RunningCount { get; private set; }
    public int TrueCount => RunningCount == 0 ? 0 : (int)Math.Ceiling(RunningCount / DeckCount);

    public void DisposeHand(Hand hand)
    {
        _disposedCards.AddRange(hand.Cards);
    }

    private void Reshuffle()
    {
        _cards.AddRange(_disposedCards);
        _cards.Shuffle();

        _disposedCards.Clear();

        RunningCount = 0;
    }

    public Card TakeNextCard()
    {
        if (!_cards.Any())
        {
            Reshuffle();
        }

        var card = _cards.First();

        switch (card.Value)
        {
            case <= 6:
                RunningCount++;
                break;
            case >= 10:
                RunningCount--;
                break;
        }

        _cards.Remove(card);

        return card;
    }
}

namespace RollBot.Models;


public class CardSystem
{
    // arrays of the choices for the card game
    private int[] cardNumbers = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13 };
    private string[] cardSuits = { "Hearts", "Diamonds", "Clubs", "Spades" };
    private string[] cardNames = { "Ace", "Two", "Three", "Four", "Five", "Six", "Seven", "Eight", "Nine", "Ten", "Jack", "Queen", "King" };

    // properties for the selected card
    public int SelectedNumber { get; set; }
    private string SelectedSuit { get; set; }
    private string SelectedName { get; set; }
    public string SelectedCard { get; set; }

    public CardSystem()
    {
        var random = new Random();
        int numberIndex = random.Next(0, cardNumbers.Length - 1);
        int suitIndex = random.Next(0, cardSuits.Length - 1);
        int nameIndex = numberIndex;

        this.SelectedNumber = cardNumbers[numberIndex];
        this.SelectedSuit = cardSuits[suitIndex];
        this.SelectedName = cardNames[nameIndex];
        this.SelectedCard = $"{this.SelectedName} of {this.SelectedSuit}";
    }
}
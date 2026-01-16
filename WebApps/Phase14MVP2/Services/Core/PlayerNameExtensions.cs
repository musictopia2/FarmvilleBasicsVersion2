namespace Phase14MVP2.Services.Core;

/// <summary>
/// UI-only aliasing for player names.
/// Keeps persisted/URL player keys stable (e.g. "Player1"/"Player2") while showing friendly names in the UI.
/// </summary>
public static class PlayerNameExtensions
{
    public static string ToDisplayPlayerName(this string playerName) => playerName switch
    {
        //used this so in future when i do my internal testing, i can use our names.
        // MVP tablet/testing aliases (keep internal keys the same)
        "Player1" => "Player1",
        "Player2" => "Player2",

        // common variations, just in case
        "Player 1" => "Player1",
        "Player 2" => "Player2",
        "player1" => "Player1",
        "player2" => "Player2",

        _ => playerName
    };
}
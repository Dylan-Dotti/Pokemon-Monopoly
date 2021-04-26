
public static class SpecialStrings
{
    public const string POKEMONEY_SYMBOL = "¥";
    public const string POKEMON_E_SYMBOL = "é";
    public const string POKE_STRING = "Poké";
    public const string POKEMON_STRING = "Pokémon";
    public const string POKEBALL_STRING = "Pokéball";

    public static string ToPokeMoneyString(this int money)
    {
        if (money >= 0) return POKEMONEY_SYMBOL + money;
        return "-" + POKEMONEY_SYMBOL + -money;
    }
}

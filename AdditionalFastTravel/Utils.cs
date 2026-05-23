namespace ATLYSS_AdditionalFastTravel;

public static class Utils
{
    public static List<T> FindClosestMatch<T>(string input, List<T> options, Func<T, string> keySelector)
    {
        foreach (var option in options)
        {
            if (keySelector(option).Equals(input, StringComparison.InvariantCultureIgnoreCase))
                return [option]; // Exact match
        }

        return options.Where(x => keySelector(x).Contains(input, StringComparison.InvariantCultureIgnoreCase)).ToList();
    }
    
    public static void ChatMsg(string message)
    {
        ChatBehaviour._current.New_ChatMessage(message);
    }
}
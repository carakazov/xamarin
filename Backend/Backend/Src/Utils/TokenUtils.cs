using Backend.Model;
using Microsoft.AspNetCore.Identity;

namespace Backend.Utils;

public class TokenUtils
{
    private const string TOKEN_CODE_WORD = "myToken";
    private const string TOKEN_DELIMITER = "|";

    public static string EncodeToToken(UserEntity user)
    {
        string tokenPlain = TOKEN_CODE_WORD + TOKEN_DELIMITER + user.Login + TOKEN_DELIMITER + user.Role.Title;
        return Base64Encoder.Encode(tokenPlain);
    }

    public static string CheckToken(string? token)
    {
        if (token == null)
        {
            return "Error";
        }

        string[] parts = token.Split(" ");
        if (parts.Length < 2)
        {
            return "Error";
        }
        string decodedToken = Base64Encoder.Base64Decode(parts[1]);
        string[] tokenParts = decodedToken.Split("|");
        if (tokenParts[0] != "myToken")
        {
            return "Error";
        }

        return tokenParts[1];
    }
}
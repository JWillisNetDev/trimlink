using shortid;
using shortid.Configuration;
using trimlink.core.Interfaces;

namespace trimlink.core;

public class TokenGenerator : ITokenGenerator
{
    private GenerationOptions GetOptions()
        => new(useNumbers: UseNumbers, useSpecialCharacters: UseSpecialCharacters, length: Length);

    public bool UseNumbers { get; set; }
    public bool UseSpecialCharacters { get; set; }
    public int Length { get; set; }

    public TokenGenerator(bool useNumbers = true, bool useSpecialCharacters = false, int length = 12)
    {
        UseNumbers = useNumbers;
        UseSpecialCharacters = useSpecialCharacters;
        Length = length;
    }

    public string GenerateToken()
    {
        var options = GetOptions();
        return ShortId.Generate(options);
    }
}


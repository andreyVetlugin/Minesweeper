namespace Minesweeper.ApplicationLayer.CustomExceptions;

public class InvalidMapParametersException : Exception
{
    public InvalidMapParametersException()
    {
    }

    public InvalidMapParametersException(string exceptionMessage) : base(exceptionMessage)
    {
    }
}
namespace Minesweeper.ApplicationLayer.CustomExceptions;

public class InvalidTurnParameterException: Exception
{
    public InvalidTurnParameterException()
    {
    }

    public InvalidTurnParameterException(string exceptionMessage) : base(exceptionMessage)
    {
    }
}
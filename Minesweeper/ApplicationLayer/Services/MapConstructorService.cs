using Minesweeper.ApplicationLayer.CustomExceptions;
using Minesweeper.ApplicationLayer.Models;

namespace Minesweeper.ApplicationLayer.Services;

public class MapConstructorService(int maxPossibleWidth, int maxPossibleHeight)
{
    public Map CreateMap(int width, int height, int minesCount)
    {
        ThrowsOnInvalidParams(width, height, minesCount);
        
        var map = new Map { Width = width, Height = height, MinesCount = minesCount };
        var fieldConstructor = new MapFieldConstructor(map);
        fieldConstructor.FillMapWithUnknownField();
        return map;
    }

    private void ThrowsOnInvalidParams(int width, int height, int minesCount)
    {
        if (width > maxPossibleWidth || width < 1)
            throw new InvalidMapParametersException($"Ширина поля может быть от 1 до {maxPossibleWidth}");
        if (height > maxPossibleHeight || height < 1)
            throw new InvalidMapParametersException($"Высота поля может быть от 1 до {maxPossibleHeight}");
        if (minesCount > width * height - 1 || minesCount < 1)
            throw new InvalidMapParametersException($"Количество мин может быть от 1 до {width * height - 1}");
    }
}
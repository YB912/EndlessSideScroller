using Mechanics.CourseGeneration;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileSetter
{
    GenerationParameters _parameters;

    Tilemap _attachedMap;

    public TileSetter(GenerationParameters parameters)
    {
        _parameters = parameters;
    }
}

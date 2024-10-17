using System;

namespace MusicApp.Domain.ValueObject;

public class Duration
{
    public TimeSpan Value { get; private set; }

    public Duration(TimeSpan value)
    {
        if (value.TotalSeconds <= 0)
        {
            throw new ArgumentException("Duration must be greater than zero.");
        }
        Value = value;
    }

    public static implicit operator TimeSpan(Duration duration) => duration.Value;
}

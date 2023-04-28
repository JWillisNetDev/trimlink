﻿public interface IEntity<out TKey>
    where TKey : IEquatable<TKey>
{
    TKey Id { get; }
}

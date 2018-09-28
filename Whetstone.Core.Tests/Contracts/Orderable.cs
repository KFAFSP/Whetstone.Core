using System;
using System.Collections.Generic;

using JetBrains.Annotations;

namespace Whetstone.Core.Contracts
{
    public sealed class Orderable : IComparable<Orderable>
    {
        static readonly Dictionary<int, Orderable> _FMapped = new Dictionary<int, Orderable>();

        [MustUseReturnValue]
        [NotNull]
        public static Orderable Map(int AValue)
        {
            if (!_FMapped.TryGetValue(AValue, out var replace))
            {
                replace = new Orderable(AValue);
                _FMapped.Add(AValue, replace);
            }

            return replace;
        }

        public Orderable(int AValue)
        {
            Value = AValue;
        }

        #region System.Object overrides
        public override string ToString()
        {
            return $"'{Value}'";
        }
        #endregion

        #region IComparable<CompareExemplar>
        public int CompareTo(Orderable AOrderable)
        {
            return AOrderable is null
                ? 1
                : Value.CompareTo(AOrderable.Value);
        }
        #endregion

        int Value { get; }
    }
}
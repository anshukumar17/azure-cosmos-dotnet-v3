﻿// ------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
// ------------------------------------------------------------

namespace Microsoft.Azure.Cosmos.Routing
{
    using System;

    internal readonly struct PartitionKeyHashRange : IComparable<PartitionKeyHashRange>, IEquatable<PartitionKeyHashRange>
    {
        /// <summary>
        /// Range of PartitionKeyHash where both start and end can be nullable, which signals an open range.
        /// </summary>
        /// <param name="startInclusive">The inclusive start hash. If null this means that the range starts from negative infinity.</param>
        /// <param name="endExclusive">The exclusive end hash. If null this means that the range goes until positive infinity.</param>
        public PartitionKeyHashRange(PartitionKeyHash? startInclusive, PartitionKeyHash? endExclusive)
        {
            if (endExclusive.HasValue && startInclusive.HasValue && (endExclusive.Value.CompareTo(startInclusive.Value) < 0))
            {
                throw new ArgumentOutOfRangeException($"{nameof(startInclusive)} must be less than {nameof(endExclusive)}.");
            }

            this.StartInclusive = startInclusive;
            this.EndExclusive = endExclusive;
        }

        public PartitionKeyHash? StartInclusive { get; }

        public PartitionKeyHash? EndExclusive { get; }

        public int CompareTo(PartitionKeyHashRange other)
        {
            // Provide a total sort order by first comparing on the start and then going to the end.
            int cmp;
            if (this.StartInclusive.HasValue && other.StartInclusive.HasValue)
            {
                cmp = this.StartInclusive.Value.CompareTo(other.StartInclusive.Value);
            }
            else if (this.StartInclusive.HasValue && !other.StartInclusive.HasValue)
            {
                cmp = -1;
            }
            else if (!this.StartInclusive.HasValue && other.StartInclusive.HasValue)
            {
                cmp = 1;
            }
            else
            {
                cmp = 0;
            }

            if (cmp != 0)
            {
                return cmp;
            }

            if (this.EndExclusive.HasValue && other.EndExclusive.HasValue)
            {
                cmp = this.EndExclusive.Value.CompareTo(other.EndExclusive.Value);
            }
            else if (this.EndExclusive.HasValue && !other.EndExclusive.HasValue)
            {
                cmp = -1;
            }
            else if (!this.EndExclusive.HasValue && other.EndExclusive.HasValue)
            {
                cmp = 1;
            }
            else
            {
                cmp = 0;
            }

            return cmp;
        }

        public override bool Equals(object obj)
        {
            if (!(obj is PartitionKeyHashRange partitionKeyHashRange))
            {
                return false;
            }

            return this.Equals(partitionKeyHashRange);
        }

        public bool Equals(PartitionKeyHashRange other)
        {
            return this.StartInclusive.Equals(other.StartInclusive) && this.EndExclusive.Equals(other.EndExclusive);
        }

        public override int GetHashCode()
        {
            int startHashCode = this.StartInclusive.GetHashCode();
            int endHashCode = this.EndExclusive.GetHashCode();
            return startHashCode ^ endHashCode;
        }
    }
}

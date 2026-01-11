// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System.Diagnostics.CodeAnalysis;

namespace System.Buffers
{
    internal sealed partial class ArrayPoolEventSource 
    {
        private const string EventSourceSuppressMessage = "Parameters to this method are primitive and are trimmer safe";
        internal static readonly ArrayPoolEventSource Log = new ArrayPoolEventSource();

        /// <summary>Bucket ID used when renting/returning an array that's too large for a pool.</summary>
        internal const int NoBucketId = -1;

        public ArrayPoolEventSource() {}

        public bool IsEnabled() => false;

        /// <summary>The reason for a BufferAllocated event.</summary>
        internal enum BufferAllocatedReason : int
        {
            /// <summary>The pool is allocating a buffer to be pooled in a bucket.</summary>
            Pooled,
            /// <summary>The requested buffer size was too large to be pooled.</summary>
            OverMaximumSize,
            /// <summary>The pool has already allocated for pooling as many buffers of a particular size as it's allowed.</summary>
            PoolExhausted
        }

        /// <summary>The reason for a BufferDropped event.</summary>
        internal enum BufferDroppedReason : int
        {
            /// <summary>The pool is full for buffers of the specified size.</summary>
            Full,
            /// <summary>The buffer size was too large to be pooled.</summary>
            OverMaximumSize,
        }

        // Parameterized constructor to block initialization and ensure the EventSourceGenerator is creating the default constructor
        // as you can't make a constructor partial.
        private ArrayPoolEventSource(int _) { }

        /// <summary>
        /// Event for when a buffer is rented.  This is invoked once for every successful call to Rent,
        /// regardless of whether a buffer is allocated or a buffer is taken from the pool.  In a
        /// perfect situation where all rented buffers are returned, we expect to see the number
        /// of BufferRented events exactly match the number of BuferReturned events, with the number
        /// of BufferAllocated events being less than or equal to those numbers (ideally significantly
        /// less than).
        /// </summary>
        internal unsafe void BufferRented(int bufferId, int bufferSize, int poolId, int bucketId)
        {
        }

        /// <summary>
        /// Event for when a buffer is allocated by the pool.  In an ideal situation, the number
        /// of BufferAllocated events is significantly smaller than the number of BufferRented and
        /// BufferReturned events.
        /// </summary>
        internal unsafe void BufferAllocated(int bufferId, int bufferSize, int poolId, int bucketId, BufferAllocatedReason reason)
        {
        }

        /// <summary>
        /// Event raised when a buffer is returned to the pool.  This event is raised regardless of whether
        /// the returned buffer is stored or dropped.  In an ideal situation, the number of BufferReturned
        /// events exactly matches the number of BufferRented events.
        /// </summary>
        internal void BufferReturned(int bufferId, int bufferSize, int poolId) => WriteEvent(3, bufferId, bufferSize, poolId);

        /// <summary>
        /// Event raised when we attempt to free a buffer due to inactivity or memory pressure (by no longer
        /// referencing it). It is possible (although not common) this buffer could be rented as we attempt
        /// to free it. A rent event before or after this event for the same ID, is a rare, but expected case.
        /// </summary>
        internal void BufferTrimmed(int bufferId, int bufferSize, int poolId) => WriteEvent(4, bufferId, bufferSize, poolId);

        /// <summary>
        /// Event raised when we check to trim buffers.
        /// </summary>
        internal void BufferTrimPoll(int milliseconds, int pressure) => WriteEvent(5, milliseconds, pressure);

        private void WriteEvent(int a, int b, int c)
        {}

        private void WriteEvent(int a, int b, int c, int d)
        {}

        /// <summary>
        /// Event raised when a buffer returned to the pool is dropped.
        /// </summary>
        internal unsafe void BufferDropped(int bufferId, int bufferSize, int poolId, int bucketId, BufferDroppedReason reason)
        {
        }
    }
}
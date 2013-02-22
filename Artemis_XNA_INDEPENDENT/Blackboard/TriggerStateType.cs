namespace Artemis.Blackboard
{
    /// <summary>Enum TriggerStateType.</summary>
    public enum TriggerStateType : long
    {
        /// <summary>The value added.</summary>
        ValueAdded   = 0x00001,

        /// <summary>The value removed.</summary>
        ValueRemoved = 0x00010,

        /// <summary>The value changed.</summary>
        ValueChanged = 0x00100,

        /// <summary>The trigger added.</summary>
        TriggerAdded = 0x01000
    }
}
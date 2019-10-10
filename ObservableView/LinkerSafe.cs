using System;
using ObservableView;

//[assembly: LinkerSafe]
[assembly: Preserve (AllMembers = true)]
namespace ObservableView
{
    [AttributeUsage(AttributeTargets.Assembly)]
    internal class LinkerSafeAttribute : Attribute
    {
    }

    [AttributeUsage(AttributeTargets.All)]
    internal sealed class PreserveAttribute : Attribute
    {
        /// <summary>
        /// Ensures that all members of this type are preserved.
        /// </summary>
        public bool AllMembers;

        /// <summary>
        /// Flags the method as a method to preserve during linking if the container class is pulled in.
        /// </summary>
        public bool Conditional;

        public PreserveAttribute(bool allMembers, bool conditional)
        {
            this.AllMembers = allMembers;
            this.Conditional = conditional;
        }

        public PreserveAttribute()
        {
        }
    }
}
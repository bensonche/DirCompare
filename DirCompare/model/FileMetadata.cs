using System;

namespace DirCompare.model
{
    public class FileMetadata
    {
        public static bool checkDate;
        public static bool checkContent;

        public string Name { get; set; }

        private DateTime? _modifyDate;
        public DateTime? ModifyDate
        {
            get
            {
                return checkDate ? _modifyDate : null;
            }
            set
            {
                _modifyDate = value;
            }
        }
        public long? Size { get; set; }

        public override int GetHashCode()
        {
            int hash = 17;

            int nameHash = Name?.GetHashCode() ?? 17;
            int dateHash = ModifyDate?.GetHashCode() ?? 17;
            int sizeHash = Size?.GetHashCode() ?? 17;

            unchecked
            {
                hash = hash * 17 + nameHash;
                hash = hash * 17 + dateHash;
                hash = hash * 17 + sizeHash;
            }

            return hash;
        }

        public override bool Equals(object obj)
        {
            var a = obj as FileMetadata;

            if (a == null )
                return false;

            return Name == a.Name && Size == a.Size && ModifyDate == a.ModifyDate;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ElvuiUpdater.SoftwareUpdate
{
    public class Version : IComparable
    {
        public int Major { get; set; }
        public int Minor { get; set; }
        public int Patch { get; set; }
        public int Build { get; set; }
        public string MajorRow { get; set; }
        public string MinorRow { get; set; }
        public string PatchRow { get; set; }
        public string BuildRow { get; set; }
        public string Row { get; set; }
        public int Priority { get; set; } = 0;
        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}.{Build} (row:{MajorRow}.{MinorRow}.{PatchRow}.{BuildRow}) - priority {Priority}";
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            Version target = obj as Version;

            return target.Major == this.Major &&
                    target.Minor == this.Minor &&
                    target.Build == this.Build &&
                    target.Patch == this.Patch;
        }

        public static bool operator >(Version a, Version b)
        {
            if (a.Major > b.Major) return true;
            if (a.Major == b.Major && a.Minor > b.Minor) return true;
            if (a.Major == b.Major && a.Minor == b.Minor && a.Patch > b.Patch) return true;
            if (a.Major == b.Major && a.Minor == b.Minor && a.Patch == b.Patch && a.Build > b.Build) return true;
            return false;
        }

        public static bool operator >=(Version a, Version b)
        {
            return a > b || a.Equals(b);
        }

        public static bool operator <(Version a, Version b)
        {
            if (b.Major > a.Major) return true;
            if (b.Major == a.Major && b.Minor > a.Minor) return true;
            if (b.Major == a.Major && b.Minor == a.Minor && b.Patch > a.Patch) return true;
            if (b.Major == a.Major && b.Minor == a.Minor && b.Patch == a.Patch && b.Build > a.Build) return true;
            return false;
        }
        public static bool operator <=(Version a, Version b)
        {
            return a < b || a.Equals(b);
        }
        public static bool operator ==(Version a, Version b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(Version a, Version b)
        {
            return !a.Equals(b);
        }

        public static Version Parse(string version)
        {
            var result = new Version();
            result.Row = version;
            version = version.Replace("release/", "").Replace("hotfix/", "");
            version = Regex.Replace(version, @"[a-z \/-]", "", RegexOptions.IgnoreCase);

            var digits = version.Split(".");


            if (digits.Length > 0 && int.TryParse(digits[0], out int major))
                result.Major = major;
            else
                result.Priority--;

            if (digits.Length > 1 && int.TryParse(digits[1], out int minor))
                result.Minor = minor;
            else
                result.Priority--;

            if (digits.Length > 2 && int.TryParse(digits[2], out int patch))
                result.Patch = patch;
            else
                result.Priority--;

            if (digits.Length > 3 && int.TryParse(digits[3], out int build))
                result.Build = build;
            else
                result.Priority--;

            result.MajorRow = digits.Length > 0 ? digits[0] : null;
            result.MinorRow = digits.Length > 1 ? digits[1] : null;
            result.PatchRow = digits.Length > 2 ? digits[2] : null;
            result.BuildRow = digits.Length > 3 ? digits[3] : null;

            return result;
        }

        public int CompareTo(object obj)
        {
            Version target = obj as Version;

            if (target > this) return -1;
            if (target < this) return 1;
            if (target == this && target.Priority > this.Priority) return 1;
            if (target == this && target.Priority < this.Priority) return -1;
            if (target == this && target.Priority == this.Priority) return 0;
            return -1;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }


}

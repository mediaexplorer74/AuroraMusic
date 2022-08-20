//
// Copyright SmartFormat Project maintainers and contributors.
// Licensed under the MIT license.
//

namespace SmartFormat.Extensions
{
    internal class BindingFlags
    {
        public static BindingFlags? Instance { get; internal set; }
        public static BindingFlags? Static { get; internal set; }
        public static BindingFlags? Public { get; internal set; }
    }
}
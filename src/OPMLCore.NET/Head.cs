using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace OPMLCore.NET {
    using static CommonUtils;

    public class Head
    {
        ///<summary>
        /// title
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// Created date
        ///</summary>
        public DateTime? DateCreated { get; set; }

        ///<summary>
        /// Modified date
        ///</summary>
        public DateTime? DateModified { get; set; }

        ///<summary>
        /// ownerName
        ///</summary>
        public string OwnerName { get; set; }

        ///<summary>
        /// ownerEmail
        ///</summary>
        public string OwnerEmail { get; set; }

        ///<summary>
        /// ownerId
        ///</summary>
        public string OwnerId { get; set;}

        ///<summary>
        /// docs
        ///</summary>
        public string Docs { get; set;}

        ///<summary>
        /// expansionState
        ///</summary>
        public List<string> ExpansionState { get; set; } = new();

        ///<summary>
        /// vertScrollState
        ///</summary>
        public string VertScrollState { get; set; }

        ///<summary>
        /// windowTop
        ///</summary>
        public string WindowTop { get; set; }

        ///<summary>
        /// windowLeft
        ///</summary>
        public string WindowLeft { get; set; }

        ///<summary>
        /// windowBottom
        ///</summary>
        public string WindowBottom { get; set; }

        ///<summary>
        /// windowRight
        ///</summary>
        public string WindowRight { get; set; }

        ///<summary>
        /// Constructor
        ///</summary>
        public Head()
        {

        }

        ///<summary>
        /// Constructor
        ///</summary>
        /// <param name="element">element of Head</param>
        public Head(XmlElement element)
        {
            if (element.Name == "head")
            {
                foreach (XmlNode node in element.ChildNodes)
                {
                    if (node.NodeType != XmlNodeType.Element) continue;
                    switch (node.Name)
                    {
                        case "title":
                            Title = node.InnerText;
                            break;
                        case "dateCreated":
                            DateCreated = ParseDateTime(node.InnerText);
                            break;
                        case "dateModified":
                            DateModified = ParseDateTime(node.InnerText);
                            break;
                        case "ownerName":
                            OwnerName = node.InnerText;
                            break;
                        case "ownerEmail":
                            OwnerEmail = node.InnerText;
                            break;
                        case "ownerId":
                            OwnerId = node.InnerText;
                            break;
                        case "docs":
                            Docs = node.InnerText;
                            break;
                        case "expansionState":
                            ExpansionState = ParseExpansionState(node.InnerText);
                            break;
                        case "vertScrollState":
                            VertScrollState = node.InnerText;
                            break;
                        case "windowTop":
                            WindowTop = node.InnerText;
                            break;
                        case "windowLeft":
                            WindowLeft = node.InnerText;
                            break;
                        case "windowBottom":
                            WindowBottom = node.InnerText;
                            break;
                        case "windowRight":
                            WindowRight = node.InnerText;
                            break;
                    }
                }
            }
        }

        private static DateTime? ParseDateTime(string value)
        {
            if (string.IsNullOrEmpty(value)) return null;
            try { return DateTime.Parse(value, Opml.MyCultureInfo); }
            catch { return null; }
        }
        private static List<string> ParseExpansionState(string value)
        {
            if (string.IsNullOrEmpty(value)) return new List<string>();
            var items = value.Split(',');
            var list = new List<string>();
            foreach (var item in items)
                if (!string.IsNullOrWhiteSpace(item))
                    list.Add(item.Trim());
            return list;
        }



        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append($"<head>{NewLine}");
            buf.Append(GetNodeString("title", Title));
            buf.Append(GetNodeString("dateCreated", DateCreated));
            buf.Append(GetNodeString("dateModified", DateModified));
            buf.Append(GetNodeString("ownerName", OwnerName));
            buf.Append(GetNodeString("ownerEmail", OwnerEmail));
            buf.Append(GetNodeString("ownerId", OwnerId));
            buf.Append(GetNodeString("docs", Docs));
            buf.Append(GetNodeString("expansionState", ExpansionState));
            buf.Append(GetNodeString("vertScrollState", VertScrollState));
            buf.Append(GetNodeString("windowTop", WindowTop));
            buf.Append(GetNodeString("windowLeft", WindowLeft));
            buf.Append(GetNodeString("windowBottom", WindowBottom));
            buf.Append(GetNodeString("windowRight", WindowRight));
            buf.Append($"</head>{NewLine}");
            return buf.ToString();
        }



        private static string GetNodeString(string name, string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : $"<{name}>{value}</{name}>{NewLine}";
        }
        private static string GetNodeString(string name, DateTime? value)
        {
            return value == null ? string.Empty : $"<{name}>{value?.ToString("R")}</{name}>{NewLine}";
        }

        private static string GetNodeString(string name, List<string> value)
        {
            if (value.Count == 0) {
                return string.Empty;
            }

            var buf = new StringBuilder();
            foreach (var item in value)
            {
                buf.Append(item);
                buf.Append(",");
            }

            return $"<{name}>{buf.Remove(buf.Length - 1, 1).ToString()}</{name}>{NewLine}";
        }
    }
}
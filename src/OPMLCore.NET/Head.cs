using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;

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


        public string Source { get; set; }

        public string Flavor { get; set; }

        public IDictionary<string, string> OtherElements { get; } = new Dictionary<string, string>();

        ///<summary>
        /// Constructor
        ///</summary>
        public Head()
        {
        }

        /// <summary>
        /// Constructor for LINQ to XML (XElement)
        /// </summary>
        public Head(System.Xml.Linq.XElement element)
        {
            if (element.Name.LocalName == "head")
            {
                foreach (var node in element.Elements())
                {
                    switch (node.Name.LocalName)
                    {
                        case "title":
                            Title = node.Value;
                            break;
                        case "dateCreated":
                            DateCreated = ParseDateTime(node.Value);
                            break;
                        case "dateModified":
                            DateModified = ParseDateTime(node.Value);
                            break;
                        case "ownerName":
                            OwnerName = node.Value;
                            break;
                        case "ownerEmail":
                            OwnerEmail = node.Value;
                            break;
                        case "ownerId":
                            OwnerId = node.Value;
                            break;
                        case "docs":
                            Docs = node.Value;
                            break;
                        case "expansionState":
                            ExpansionState = ParseExpansionState(node.Value);
                            break;
                        case "vertScrollState":
                            VertScrollState = node.Value;
                            break;
                        case "windowTop":
                            WindowTop = node.Value;
                            break;
                        case "windowLeft":
                            WindowLeft = node.Value;
                            break;
                        case "windowBottom":
                            WindowBottom = node.Value;
                            break;
                        case "windowRight":
                            WindowRight = node.Value;
                            break;
                        case "flavor":
                            Flavor = node.Value;
                            break;
                        case "source":
                            Source = node.Value;
                            break;
                        default:
                            OtherElements.TryAdd(node.Name.LocalName, node.Value);
                            break;
                    }
                }
            }
        }

        private static List<string> ParseExpansionState(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return new List<string>();
            // Use StringSplitOptions.RemoveEmptyEntries to avoid empty results
            return [..value.Split([','], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)];
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
            foreach (var element in OtherElements)
            {
                buf.Append(GetNodeString(element.Key, element.Value));
            }
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

        /// <summary>
        /// Returns an XElement representing this Head and its children, using LINQ to XML.
        /// </summary>
        public System.Xml.Linq.XElement ToXml()
        {
            var element = new System.Xml.Linq.XElement("head");
            void AddElemString(string name, string value)
            {
                if (!string.IsNullOrEmpty(value))
                    element.Add(new System.Xml.Linq.XElement(name, value));
            }
            void AddElemDate(string name, DateTime? value)
            {
                if (value != null)
                    element.Add(new System.Xml.Linq.XElement(name, value?.ToString("R")));
            }
            void AddElemList(string name, List<string> value)
            {
                if (value != null && value.Count > 0)
                    element.Add(new System.Xml.Linq.XElement(name, string.Join(",", value)));
            }

            AddElemString("title", Title);
            AddElemDate("dateCreated", DateCreated);
            AddElemDate("dateModified", DateModified);
            AddElemString("ownerName", OwnerName);
            AddElemString("ownerEmail", OwnerEmail);
            AddElemString("ownerId", OwnerId);
            AddElemString("docs", Docs);
            AddElemList("expansionState", ExpansionState);
            AddElemString("vertScrollState", VertScrollState);
            AddElemString("windowTop", WindowTop);
            AddElemString("windowLeft", WindowLeft);
            AddElemString("windowBottom", WindowBottom);
            AddElemString("windowRight", WindowRight);
            AddElemString("flavor", Flavor);
            AddElemString("source", Source);
            foreach (var elementPair in OtherElements)
            {
                AddElemString(elementPair.Key, elementPair.Value);
            }
            return element;
        }

        /// <summary>
        /// Returns the XML string representation of this Head and its children, using LINQ to XML.
        /// </summary>
        public string ToXmlString(SaveOptions saveOptions = SaveOptions.DisableFormatting)
        {
            return ToXml().ToString(saveOptions);
        }
    }
}
using System;
using System.Collections.Generic;
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
        public Head(XElement element)
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
            return ToXmlString(SaveOptions.None);
        }

        /// <summary>
        /// Returns an XElement representing this Head and its children, using LINQ to XML.
        /// </summary>
        public XElement ToXml()
        {
            var element = new XElement("head");

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

            void AddElemString(string name, string value)
            {
                if (!string.IsNullOrEmpty(value))
                    element.Add(new XElement(name, value));
            }

            void AddElemDate(string name, DateTime? value)
            {
                if (value != null)
                    element.Add(new XElement(name, value.Value.ToString("R")));
            }

            void AddElemList(string name, List<string> value)
            {
                if (value != null && value.Count > 0)
                    element.Add(new XElement(name, string.Join(",", value)));
            }
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
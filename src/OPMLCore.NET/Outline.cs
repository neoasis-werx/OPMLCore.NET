using System;
using System.Text;
using System.Xml;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OPMLCore.NET {
    public class Outline
    {
        ///<summary>
        /// Text of the XML file (required)
        ///</summary>
        public string Text { get; set; }

        ///<summary>
        /// true / false
        ///</summary>
        public string IsComment { get; set; }

        ///<summary>
        /// true / false
        ///</summary>
        public string IsBreakpoint { get; set; }

        ///<summary>
        /// outline node was created
        ///</summary>
        public DateTime? Created { get; set; }

        ///<summary>
        /// Categories
        ///</summary>
        public List<string> Category { get; set; } = new List<string>();

        ///<summary>
        /// Description
        ///</summary>
        public string Description { get; set; }

        ///<summary>
        /// HTML URL
        ///</summary>
        public string HtmlUrl { get; set; }

        ///<summary>
        /// Language
        ///</summary>
        public string Language { get; set; }

        ///<summary>
        /// Title
        ///</summary>
        public string Title { get; set; }

        ///<summary>
        /// Type (rss/atom)
        ///</summary>
        public string Type { get; set; }

        ///<summary>
        /// Version of RSS.
        /// RSS1 for RSS1.0. RSS for 0.91, 0.92 or 2.0.
        ///</summary>
        public string Version { get; set; }

        ///<summary>
        /// URL of the XML file
        ///</summary>
        public string XmlUrl { get; set; }


        /// <summary>
        /// DynaList flavor Attribute
        /// </summary>
        public string Note { get; set; }

        ///<summary>
        /// Outline list
        ///</summary>
        public List<Outline> Outlines { get; set; }  = new List<Outline>();


        public IDictionary<string, string> OtherAttributes { get; } = new Dictionary<string, string>();

        ///<summary>
        /// Constructor
        ///</summary>
        public Outline()
        {

        }


        ///<summary>
        /// Constructor for LINQ to XML (XElement)
        ///</summary>
        /// <param name="element">XElement representing an <outline> node</param>
        public Outline(System.Xml.Linq.XElement element)
        {
            if (element == null) return;
            foreach (var attr in element.Attributes())
            {
                switch (attr.Name.LocalName)
                {
                    case "text":
                        Text = attr.Value;
                        break;
                    case "isComment":
                        IsComment = attr.Value;
                        break;
                    case "isBreakpoint":
                        IsBreakpoint = attr.Value;
                        break;
                    case "created":
                        Created = CommonUtils.ParseDateTime(attr.Value);
                        break;
                    case "category":
                        Category = GetCategoriesAttribute(attr.Value);
                        break;
                    case "description":
                        Description = attr.Value;
                        break;
                    case "htmlUrl":
                        HtmlUrl = attr.Value;
                        break;
                    case "language":
                        Language = attr.Value;
                        break;
                    case "title":
                        Title = attr.Value;
                        break;
                    case "type":
                        Type = attr.Value;
                        break;
                    case "version":
                        Version = attr.Value;
                        break;
                    case "xmlUrl":
                        XmlUrl = attr.Value;
                        break;
                    case "_note":
                        Note = attr.Value;
                        break;
                    default:
                        OtherAttributes.TryAdd(attr.Name.LocalName, attr.Value);
                        break;
                }
            }

            foreach (var child in element.Elements("outline"))
            {
                Outlines.Add(new Outline(child));
            }
        }

        private static List<string> GetCategoriesAttribute(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return new List<string>();
            return new List<string>(value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
        }

        public override string ToString()
        {
            StringBuilder buf = new StringBuilder();
            buf.Append("<outline");
            buf.Append(GetAttributeString("text", Text));
            buf.Append(GetAttributeString("isComment", IsComment));
            buf.Append(GetAttributeString("isBreakpoint", IsBreakpoint));
            buf.Append(GetAttributeString("created", Created));
            buf.Append(GetAttributeString("category", Category));
            buf.Append(GetAttributeString("description", Description));
            buf.Append(GetAttributeString("htmlUrl", HtmlUrl));
            buf.Append(GetAttributeString("language", Language));
            buf.Append(GetAttributeString("title", Title));
            buf.Append(GetAttributeString("type", Type));
            buf.Append(GetAttributeString("version", Version));
            buf.Append(GetAttributeString("xmlUrl", XmlUrl));
            foreach (var attribute in OtherAttributes)
            {
                buf.Append(GetAttributeString(attribute.Key, attribute.Value));
            }

            if (Outlines.Count > 0)
            {
                buf.Append(">\r\n");
                foreach (Outline outline in Outlines)
                {
                    buf.Append(outline);
                }
                buf.Append("</outline>\r\n");
            } else {
                buf.Append(" />\r\n");
            }
            return buf.ToString();
        }

        private static string GetAttributeString(string name, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            } else {
                return $" {name}=\"{value}\"";
            }
        }

        private static string GetAttributeString(string name, DateTime? value)
        {
            if (value == null)
            {
                return string.Empty;
            } else {
                return $" {name}=\"{value?.ToString("R")}\"";
            }
        }

        private static string GetAttributeString(string name, List<string> value)
        {
            if (value.Count == 0) {
                return string.Empty;
            }

            StringBuilder buf = new StringBuilder();
            foreach (var item in value)
            {
                buf.Append(item);
                buf.Append(",");
            }

            return $" {name}=\"{buf.Remove(buf.Length - 1, 1)}\"";
        }
    }
}
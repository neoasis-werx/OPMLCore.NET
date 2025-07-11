using System.Text;
using System.Xml.Linq;
using System.Collections.Generic;

namespace OPMLCore.NET {
    using System;
    using static CommonUtils;

    public class Opml {
        ///<summary>
        /// Version of OPML
        ///</summary>
        public string Version { get; set;}

        ///<summary>
        /// Encoding of OPML
        ///</summary>
        public string Encoding { get; set;}

        ///<summary>
        /// Head of OPML
        ///</summary>
        public Head Head { get; set;} = new Head();

        ///<summary>
        /// Body of OPML
        ///</summary>
        public Body Body { get; set;} = new Body();

        ///<summary>
        /// Other OPML attributes not explicitly handled
        ///</summary>
        public IDictionary<string, string> OtherAttributes { get; set; } = new Dictionary<string, string>();

        ///<summary>
        /// Constructor
        ///</summary>
        public Opml() { }

        ///<summary>
        /// Constructor
        ///</summary>
        /// <param name="location">Location of the OPML file</param>
        public Opml(string location)
        {
            var doc = XDocument.Load(location);
            ReadOpmlNodes(doc);
        }

        ///<summary>
        /// Constructor
        ///</summary>
        /// <param name="doc">XDocument of the OPML</param>
        public Opml(XDocument doc)
        {
            ReadOpmlNodes(doc);
        }

        private void ReadOpmlNodes(XDocument doc) {
            // Get encoding from XDeclaration if available
            if (doc.Declaration != null && !string.IsNullOrEmpty(doc.Declaration.Encoding))
            {
                Encoding = doc.Declaration.Encoding.Equals("iso-8859-1", StringComparison.InvariantCultureIgnoreCase) ? "UTF-8" : doc.Declaration.Encoding;
            }

            // Find the first 'opml' element
            var opmlElement = doc.Root;
            if (opmlElement == null || opmlElement.Name != "opml") return;

            // Read attributes using a switch statement
            foreach (var attr in opmlElement.Attributes())
            {
                switch (attr.Name.LocalName)
                {
                    case "version":
                        Version = attr.Value;
                        break;
                    default:
                        OtherAttributes.TryAdd(attr.Name.LocalName, attr.Value);
                        break;
                }
            }

            // Loop through children of 'opml' element only once
            foreach (var child in opmlElement.Elements())
            {
                switch (child.Name.LocalName)
                {
                    case "head":
                        Head = new Head(child);
                        break;
                    case "body":
                        Body = new Body(child);
                        break;
                }
            }
        }

        public override string ToString()
        {
            var buf = new StringBuilder();
            var encoding = string.IsNullOrEmpty(Encoding)?"UTF-8":Encoding;
            buf.Append($"<?xml version=\"1.0\" encoding=\"{encoding}\" ?>{NewLine}");
            var version = string.IsNullOrEmpty(Version)?"2.0":Version;
            buf.Append($"<opml version=\"{version}\">{NewLine}");
            buf.Append(Head.ToString());
            buf.Append(Body.ToString());
            buf.Append("</opml>");

            return buf.ToString();
        }

        /// <summary>
        /// Returns an XDocument representing this OPML document and its children, using LINQ to XML.
        /// </summary>
        public XDocument ToXml()
        {
            var encoding = string.IsNullOrEmpty(Encoding) ? "UTF-8" : Encoding;
            var version = string.IsNullOrEmpty(Version) ? "2.0" : Version;
            var declaration = new XDeclaration("1.0", encoding, null);

            var opmlElem = new XElement("opml");
            opmlElem.SetAttributeValue("version", version);
            foreach (var attr in OtherAttributes)
            {
                opmlElem.SetAttributeValue(attr.Key, attr.Value);
            }
            opmlElem.Add(Head.ToXml());
            opmlElem.Add(Body.ToXml());

            var doc = new XDocument(declaration, opmlElem);
            return doc;
        }

        /// <summary>
        /// Returns the XML string representation of this OPML document and its children, using LINQ to XML.
        /// </summary>
        public string ToXmlString(SaveOptions saveOptions = SaveOptions.DisableFormatting)
        {
            return ToXml().ToString(saveOptions);
        }

        /// <summary>
        /// Loads an OPML document from a file path.
        /// </summary>
        public static Opml Load(string path)
        {
            var doc = XDocument.Load(path);
            return new Opml(doc);
        }

        /// <summary>
        /// Saves this OPML document to the specified file path using LINQ to XML.
        /// </summary>
        public void Save(string path, SaveOptions saveOptions = SaveOptions.DisableFormatting)
        {
            ToXml().Save(path, saveOptions);
        }
    }
}
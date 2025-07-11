using System.Xml.Linq;

namespace OPMLCore.NET.Tests
{
    public static class OpmlTestData
    {
        public static string GetNormalOpmlXmlString()
        {
            var opml = new XElement("opml",
                new XAttribute("version", "2.0"),
                new XElement("head",
                    new XElement("title", "mySubscriptions.opml"),
                    new XElement("dateCreated", "Sat, 18 Jun 2005 12:11:52 GMT"),
                    new XElement("dateModified", "Tue, 02 Aug 2005 21:42:48 GMT"),
                    new XElement("ownerName", "fnya"),
                    new XElement("ownerEmail", "fnya@example.com"),
                    new XElement("ownerId", "http://news.com.com/"),
                    new XElement("docs", "http://news.com.com/"),
                    new XElement("expansionState", "1, 6, 13, 16, 18, 20"),
                    new XElement("vertScrollState", "1"),
                    new XElement("windowTop", "106"),
                    new XElement("windowLeft", "106"),
                    new XElement("windowBottom", "558"),
                    new XElement("windowRight", "479")
                ),
                new XElement("body",
                    new XElement("outline",
                        new XAttribute("text", "CNET News.com"),
                        new XAttribute("isComment", "true"),
                        new XAttribute("isBreakpoint", "true"),
                        new XAttribute("created", "Tue, 02 Aug 2005 21:42:48 GMT"),
                        new XAttribute("category", "/Harvard/Berkman,/Politics"),
                        new XAttribute("description", "Tech news and business reports by CNET News.com."),
                        new XAttribute("htmlUrl", "http://news.com.com/"),
                        new XAttribute("language", "unknown"),
                        new XAttribute("title", "CNET News.com"),
                        new XAttribute("type", "rss"),
                        new XAttribute("version", "RSS2"),
                        new XAttribute("xmlUrl", "http://news.com.com/2547-1_3-0-5.xml")
                    )
                )
            );
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                opml
            );
            return xdoc.ToString(SaveOptions.DisableFormatting);
        }

        public static string GetChildOpmlXmlString()
        {
            var opml = new XElement("opml",
                new XAttribute("version", "2.0"),
                new XElement("head",
                    new XElement("title", "mySubscriptions.opml")
                ),
                new XElement("body",
                    new XElement("outline",
                        new XAttribute("text", "IT"),
                        new XElement("outline",
                            new XAttribute("text", "CNET News.com"),
                            new XAttribute("htmlUrl", "http://news.com.com/"),
                            new XAttribute("xmlUrl", "http://news.com.com/2547-1_3-0-5.xml")
                        )
                    )
                )
            );
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                opml
            );
            return xdoc.ToString(SaveOptions.DisableFormatting);
        }

        public static string GetChildNodeOpmlXmlString()
        {
            var opml = new XElement("opml",
                new XAttribute("version", "2.0"),
                new XElement("head",
                    new XElement("title", "mySubscriptions.opml")
                ),
                new XElement("body",
                    new XElement("outline",
                        new XAttribute("text", "IT"),
                        new XElement("outline",
                            new XAttribute("text", "washingtonpost.com"),
                            new XAttribute("htmlUrl", "http://www.washingtonpost.com"),
                            new XAttribute("xmlUrl", "http://www.washingtonpost.com/rss.xml")
                        )
                    )
                )
            );
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                opml
            );
            return xdoc.ToString(SaveOptions.DisableFormatting);
        }

        public static string GetCreateNormalOpmlXmlString()
        {
            var opml = new XElement("opml",
                new XAttribute("version", "2.0"),
                new XElement("head",
                    new XElement("title", "mySubscriptions.opml"),
                    new XElement("dateCreated", "Sat, 18 Jun 2005 12:11:52 GMT"),
                    new XElement("dateModified", "Tue, 02 Aug 2005 21:42:48 GMT"),
                    new XElement("ownerName", "fnya"),
                    new XElement("ownerEmail", "fnya@example.com"),
                    new XElement("ownerId", "http://news.com.com/"),
                    new XElement("docs", "http://news.com.com/"),
                    new XElement("expansionState", "1,6,13,16,18,20"),
                    new XElement("vertScrollState", "1"),
                    new XElement("windowTop", "106"),
                    new XElement("windowLeft", "106"),
                    new XElement("windowBottom", "558"),
                    new XElement("windowRight", "479")
                ),
                new XElement("body",
                    new XElement("outline",
                        new XAttribute("text", "CNET News.com"),
                        new XAttribute("isComment", "true"),
                        new XAttribute("isBreakpoint", "true"),
                        new XAttribute("created", "Tue, 02 Aug 2005 21:42:48 GMT"),
                        new XAttribute("category", "/Harvard/Berkman,/Politics"),
                        new XAttribute("description", "Tech news and business reports by CNET News.com."),
                        new XAttribute("htmlUrl", "http://news.com.com/"),
                        new XAttribute("language", "unknown"),
                        new XAttribute("title", "CNET News.com"),
                        new XAttribute("type", "rss"),
                        new XAttribute("version", "RSS2"),
                        new XAttribute("xmlUrl", "http://news.com.com/2547-1_3-0-5.xml")
                    )
                )
            );
            var xdoc = new XDocument(
                new XDeclaration("1.0", "UTF-8", null),
                opml
            );
            return xdoc.ToString(SaveOptions.DisableFormatting);
        }
    }
}

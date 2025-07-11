function outlineToJson (adrx, nameOutlineElement) {
	var theOutline = new Object ();
	if (nameOutlineElement == undefined) {
		nameOutlineElement = "source\\:outline";
		}
	xmlGatherAttributes (adrx, theOutline);
	if (xmlHasSubs (adrx)) {
		theOutline.subs = [];
		$(adrx).children (nameOutlineElement).each (function () {
			theOutline.subs [theOutline.subs.length] = outlineToJson (this, nameOutlineElement);
			});
		}
	return (theOutline);
	}
function getPubDate (item) {
	var d = new Date ();
	$(item).children ("pubDate").each (function () {
		d = new Date ($(this).text ());
		});
	return (d);
	}
function getSub (item, name) {
	var url;
	$(item).children (name).each (function () {
		url = $(this).text ();
		});
	return (url);
	}
function readFlickrFeed (url, idFlickrPics, callback) {
	var htmltext = "", indentlevel = 0, whenstart = new Date ();
	function add (s) {
		htmltext += s + "\r\n";
		}
	readHttpFileThruProxy (url, undefined, function (xmltext) {
		var xstruct = $($.parseXML (xmltext));
		var adrchannel = xmlGetAddress (xstruct, "channel");
		$(adrchannel).children ("item").each (function () {
			var title = getSub (this, "title"), pubDate = getPubDate (this), link = getSub (this, "link");
			var permalinkString = formatDate (pubDate, "%d-%b-%Y"), urlImage; 
			
			$(this).children ("media\\:content").each (function () {
				urlImage = $(this).attr ("url");
				});
			
			var theOutline = {
				text: title,
				type: "image",
				url: urlImage
				};
			add ("<div class=\"divBlogPost\">"); indentlevel++;
			add (riverRenderTypedOutline (theOutline, link, permalinkString.toUpperCase (), true));
			add ("</div>"); indentlevel--;
			});
		$("#" + idFlickrPics).append (htmltext);
		if (callback != undefined) {
			callback ();
			}
		console.log ("readFlickrFeed: " + secondsSince (whenstart) + " secs.");
		});
	}
function readOpmlFile (url, idOpml, outlineTitle, callback) {
	readHttpFile (url, function (opmltext) {
		var xstruct = $($.parseXML (opmltext));
		var adrbody = getXstuctBody (xstruct);
		var theOutline = outlineToJson (adrbody, "outline");
		theOutline.text = outlineTitle;
		$("#" + idOpml).html (riverRenderOutline (theOutline, false, undefined, undefined, true)); //10/23/16 by DW -- changed last param from false to true
		if (callback !== undefined) { //2/13/16 by DW
			callback ();
			}
		});
	}
function readOutlinesFromRss (url, idBlogPosts, flDayHeaders, flTypedRender, callback) {
	var theDays = [], htmltext = "", indentlevel = 0, whenstart = new Date ();
	
	if (flTypedRender === undefined) {
		flTypedRender = false;
		}
	
	function add (s) {
		htmltext += s + "\r\n";
		}
	$("#" + idBlogPosts).empty ();
	readHttpFile (url, function (xmltext) {
		var xstruct = $($.parseXML (xmltext));
		var adrchannel = xmlGetAddress (xstruct, "channel");
		
		var lastBuildDate = new Date (getSub (adrchannel, "lastBuildDate"));
		if (lastBuildDate > whenLastUpdate) {
			whenLastUpdate = lastBuildDate;
			}
		
		$(adrchannel).children ("item").each (function () {
			var pubDate = getPubDate (this), urlPermalink = getSub (this, "guid");
			$(this).children ("source\\:outline").each (function () {
				var theOutline = outlineToJson (this), flfound = false, flMarkdown = theOutline.type == "markdown", htmltext;
				var permalinkString = formatDate (pubDate, "%d-%b-%Y"); //"###";
				if (flTypedRender) {
					theOutline ["margin-left"] = 0;
					htmltext = riverRenderTypedOutline (theOutline, urlPermalink, permalinkString.toUpperCase (), true);
					}
				else {
					htmltext = riverRenderOutline (theOutline, flMarkdownDefault, urlPermalink, permalinkString.toUpperCase (), true);
					}
				
				for (var i = 0; i < theDays.length; i++) {
					if (sameDay (theDays [i].when, pubDate)) {
						var theOutlines = theDays [i].outlines;
						theOutlines [theOutlines.length] = htmltext;
						flfound = true;
						break;
						}
					}
				if (!flfound) {
					theDays [theDays.length] = {
						when: pubDate,
						outlines: [htmltext]
						};
					}
				});
			});
		for (var i = 0; i < theDays.length; i++) {
			var theDay = theDays [i];
			if (flDayHeaders) {
				var datestring = formatDate (theDay.when, pagetable.homePageDateFormat);
				add ("<div class=\"divLinkblogDayTitle\">" + datestring + "</div>");
				add ("<div class=\"divBlogDay\">"); indentlevel++;
				}
			for (var j = 0; j < theDay.outlines.length; j++) {
				var theOutline = theDay.outlines [j];
				add ("<div class=\"divBlogPost\">"); indentlevel++;
				add (theOutline);
				add ("</div>"); indentlevel--;
				}
			if (flDayHeaders) {
				add ("</div>"); indentlevel--;
				}
			}
		$("#" + idBlogPosts).append (htmltext);
		console.log ("readOutlinesFromRss: url == " + url + ", " + secondsSince (whenstart) + " secs.");
		if (callback != undefined) {
			callback ();
			}
		});
	}
function readLinksFromRss (url, idBlogPosts, callback) { //3/24/15 by DW
	var htmltext = "", indentlevel = 0, whenstart = new Date ();
	function add (s) {
		htmltext += s + "\r\n";
		}
	$("#" + idBlogPosts).empty ();
	readHttpFile (url, function (xmltext) {
		var xstruct = $($.parseXML (xmltext));
		var adrchannel = xmlGetAddress (xstruct, "channel");
		add ("<ul>"); indentlevel++;
		$(adrchannel).children ("item").each (function () {
			var pubDate = getSub (this, "pubDate"), urlPermalink = getSub (this, "guid"), title = getSub (this, "title"), link = getSub (this, "link");
			var theLink = "<a href=\"" + link + "\">" + title + "</a>";
			add ("<li>" + theLink + "</i>");
			console.log ("readLinksFromRss: item == " + theLink);
			
			});
		add ("</ul>"); indentlevel--;
		$("#" + idBlogPosts).append (htmltext);
		if (callback != undefined) {
			callback ();
			}
		});
	}
function readPostsFromRss (url, idBlogPosts, callback) { //12/20/16 by DW
	var htmltext = "", indentlevel = 0, whenstart = new Date ();
	function add (s) {
		htmltext += s + "\r\n";
		}
	function formatDateTime (d) {
		d = new Date (d);
		return (dateFormat (d, "m/d/yyyy; h:MM TT"));
		}
	$("#" + idBlogPosts).empty ();
	readHttpFile (url, function (xmltext) {
		var xstruct = $($.parseXML (xmltext));
		var adrchannel = xmlGetAddress (xstruct, "channel");
		add ("<div class=\"divBlogPostList\">"); indentlevel++;
		$(adrchannel).children ("item").each (function () {
			var pubDate = getSub (this, "pubDate"), urlPermalink = getSub (this, "guid"), title = getSub (this, "title"), link = getSub (this, "link"), description = getSub (this, "description");
			add ("<div class=\"divBlogPost\">"); indentlevel++;
			add ("<div class=\"divBlogPostTitle\"><a href=\"" + urlPermalink + "\">" + title + "</a></div>");
			add ("<div class=\"divBlogPostBody\">" + description + "</div></li>");
			add ("<div class=\"divBlogPostWhen\"><a href=\"" + urlPermalink + "\">" + formatDateTime (pubDate) + "</a></div></li>");
			add ("</div>"); indentlevel--;
			console.log ("readPostsFromRss: item == " + title);
			});
		add ("</div>"); indentlevel--;
		$("#" + idBlogPosts).append (htmltext);
		if (callback != undefined) {
			callback ();
			}
		});
	}
function freshBlogDisplay (theRiver, idBlogPosts) {
	var feeds = theRiver.updatedFeeds.updatedFeed, htmltext = "", indentlevel = 0, theDays = [];
	function add (s) {
		htmltext += filledString ("\t", indentlevel) + s + "\r\n";
		}
	for (var i = 0; i < feeds.length; i++) {
		var feed = feeds [i];
		for (var j = 0; j < feed.item.length; j++) {
			var item = feed.item [j];
			if (item.outline != undefined) {
				var flfound = false, pubDate = new Date (item.pubDate);
				var itemhtml = riverRenderTypedOutline (item.outline);
				console.log (item.outline.text);
				for (var k = 0; k < theDays.length; k++) {
					var theDay = theDays [k];
					if (sameDay (theDay.when, pubDate)) {
						theDay.outlines [theDay.outlines.length] = itemhtml;
						flfound = true;
						}
					}
				if (!flfound) {
					theDays [theDays.length] = {
						when: pubDate,
						outlines: [itemhtml]
						};
					}
				}
			}
		}
	
	for (var i = 0; i < theDays.length; i++) {
		var theDay = theDays [i], datestring = formatDate (theDay.when, pagetable.homePageDateFormat);
		add ("<div class=\"divLinkblogDayTitle\">" + datestring + "</div>");
		add ("<div class=\"divBlogDay\">"); indentlevel++;
		for (var j = 0; j < theDay.outlines.length; j++) {
			var theOutline = theDay.outlines [j];
			add (theOutline);
			}
		add ("</div>"); indentlevel--;
		}
	
	
	$("#" + idBlogPosts).html (htmltext);
	}
function readRiverIntoBlogTab (urlRiver, idBlogPosts, callback) {
	var whenstart = new Date ();
	function onGetRiverStream (updatedFeeds) {
		console.log ("onGetRiverStream: updatedFeeds == " + updatedFeeds);
		}
	$.ajax ({ 
		url: urlRiver,  
		jsonpCallback : "onGetRiverStream",
		success: function (data) {
			console.log ("readRiverIntoBlogTab: url == " + urlRiver);
			console.log ("readRiverIntoBlogTab: read took " + secondsSince (whenstart) + " secs.");
			freshBlogDisplay (data, idBlogPosts);
			if (callback != undefined) {
				callback ();
				}
			},
		error: function (status) {
			console.log ("readRiverIntoBlogTab: error status == " + jsonStringify (status));
			if (callback != undefined) {
				callback ();
				}
			},
		dataType: "jsonp"
		});
	}

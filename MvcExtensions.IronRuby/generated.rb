def render_page
response.write("<h2>")
response.write(view_data.message)
response.write("</h2>\r\n<p>\r\n    Say hello for 5 times\r\n    <br/>\r\n    ")
5.times do
response.write("\r\n        Hello\r\n        <br/>\r\n    ")
end
response.write("\r\n</p>\r\n<p>\r\n    To learn more about ASP.NET MVC Iron Ruby View Engine visit <a href=\"http://weblogs.asp.net/rashid\">http://weblogs.asp.net/rashid</a>.\r\n</p>")
end
def render_layout
response.write("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">\r\n<html xmlns=\"http://www.w3.org/1999/xhtml\">\r\n<head>\r\n    <title>ASP.NET MVC Iron Ruby View Engine</title>\r\n    <link href=\"")
response.write(url.content("~/Content/Site.css"))
response.write("\" rel=\"stylesheet\" type=\"text/css\"/>\r\n</head>\r\n<body>\r\n    <div class=\"page\">\r\n        <div id=\"header\">\r\n            <div id=\"title\">\r\n                <h1>My MVC Application</h1>\r\n            </div>\r\n            <div id=\"logindisplay\">\r\n                ")
html.render_partial("LogOnUserControl");
response.write("\r\n            </div>\r\n            <div id=\"menucontainer\">\r\n                <ul id=\"menu\">\r\n                    <li>")
response.write(html.action_link("Home", { :action => "index", :controller => "home" }))
response.write("</li>\r\n                    <li>")
response.write(html.action_link("About", { :action => "about", :controller => "home" }))
response.write("</li>\r\n                </ul>\r\n            </div>\r\n        </div>\r\n        <div id=\"main\">\r\n            ")
response.write(yield)
response.write("\r\n            <div id=\"footer\"></div>\r\n        </div>\r\n    </div>\r\n</body>\r\n</html>")
end

def view_data.method_missing(methodname)
	 get_Item(methodname.to_s)
end
def temp_data.method_missing(methodname)
	 get_Item(methodname.to_s)
end
def route_data.method_missing(methodname)
	 get_Item(methodname.to_s)
end
def session.method_missing(methodname)
	 get_Item(methodname.to_s)
end
def cache.method_missing(methodname)
	 get_Item(methodname.to_s)
end
def application.method_missing(methodname)
	 get_Item(methodname.to_s)
end
render_layout { |content| render_page }

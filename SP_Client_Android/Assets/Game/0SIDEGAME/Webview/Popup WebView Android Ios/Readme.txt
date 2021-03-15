This is simple plugin to call access native function webview android and ios 
and display popup screen in Unity.

For Android
on Menu Player Settings > Android Tab > Other Settings > Configuration 
set Internet Access to Require

How to use :

There are 2 function to call from PopupWebView Class

- To Display FullScreen 
PopupWebView.FullWebView(url);

- To Display Custom Screen
PopupWebView.CustomWebView(url,false,width,height);

url = your http website link
width = screen width
height = screen height

Reference:
Android
http://developer.android.com/reference/android/webkit/WebView.html
Ios
https://developer.apple.com/library/ios/documentation/UIKit/Reference/UIWebView_Class/
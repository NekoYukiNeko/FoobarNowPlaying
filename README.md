This program was originally created as a method of displaying the current track artist and title from Foobar2000 by parsing a text file using the foo_nowplaying2 component. Rather than rely on file IO operations, this uses the Windows SDK with Windows.Media.Control.GlobalSystemMediaTransportControlsSessionManager (GSMTC) to read the information from the application directly. One neat, but untested, byproduct is that this allows it to (potentially) interface with other applications agnostically.

Because this was designed to be used as an OBS overlay via window capture, the application is very simple - to the point it's designed to not be interacted with. I move it between monitors using the key sequence Windows+Shift+LeftArrow or RightArrow. It is transparent so you can still see behind it, both in the window capture and in actual use.

Customizing the window relies on modifying a build time asset, MainWindow.xaml. If changes are made to this file, you will need to rebuild the solution.

This application uses net9.0-windows10.0.19041.0 .

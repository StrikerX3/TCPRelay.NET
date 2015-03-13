# TCPRelay v0.4 beta 2

## Introduction

TCPRelay's main purpose is to serve as an intermediator between streaming programs and the actual streaming servers. For some reason certain streaming programs have poor network handling code and are unable to push enough data to the RTMP servers even though users have more than enough upload speed.

TCPRelay started out as a quick'n'dirty program just to test XSplit's upload bandwidth issues. I was surprised to see it worked. It works by relaying TCP streams between an application and the destination server, allowing users to tune the socket parameters for improved performance.

Streaming directly to Twitch.tv with XSplit:
![Streaming directly to Twitch.tv with XSplit](http://i.imgur.com/Qlgv7.png)

Streaming to Twitch.tv through the relay:
![Streaming to Twitch.tv through the relay](http://i.imgur.com/lNh3Z.png)


## Requirements

Windows
- [.NET Framework 4](http://www.microsoft.com/en-us/download/details.aspx?id=17851)

Linux
- Coming soon... hopefully! :)


## How to build

You will need Visual Studio 2010 or later. I'm not sure the Express versions will work right out of the box as the project depends on the [Newtonsoft.JSON NuGet package](https://www.nuget.org/packages/Newtonsoft.Json/).
The project can be loaded and built on [Visual Studio 2013 Community Edition](http://www.visualstudio.com/en-us/news/vs2013-community-vs.aspx).

Open the solution, update the NuGet packages, select a build target (either x86 or x64) and build it. Run the `prepare-build.cmd` script and you should be good to go.

If you wish to improve this build process, feel free to do so and submit a pull request. I would appreciate it very much.


## TCPRelay GUI Instructions

Run TCPRelay.exe to open the GUI. The Target RTMP URL comes with the default target server and is populated with a list of all Twitch.tv servers as soon as possible. Click "Load Twitch.tv servers" if you wish to refresh the list.
The listen port is the port XSplit will have to connect to. Most people won't need to change this.
Click the Start button to start the relay. As soon XSplit connects to TCPRelay, a new connection will appear in the Connections panel displaying data transfer information in real time.


## TCPRelay Console Instructions

Simply run `TCPRelayC.exe` to start a TCP relay server listening to port 1935 and targeting `live.twitch.tv:1935`, which should work for most people.
If you wish to change the target server, run `TCPRelayC.exe` with the parameter `-th:<server host name>` (eg. for Twitch.tv New York, use `-th:live-jfk.twitch.tv`).
You can also pass in the RTMP URL such as `rtmp://live.twitch.tv/app`; the target host and port will be set based on it.

Create a shortcut to `TCPRelayC.exe` with the desired parameters if you want to use custom settings without using the command line.

If you wish to list all available Twitch.tv ingest servers, run `TCPRelayC -ttv` from the command line.

Run `TCPRelayC -?` from your command prompt to get more help about the parameters.


## XSplit Instructions

1.  Make sure the TCP Relay server is up and running and pointing to the desired server.
2.  Open XSplit
3.  On the main window, go to *Broadcast* > *Edit channels...*
4.  On the User Settings window, click *Add...* > *Custom RTMP*
5.  Fill the fields as follows:
    -  **Name:** anything, this will show up in the Broadcast menu.
    -  **Description:** anything, optional.
    -  **RTMP URL:** rtmp://localhost/app   (See note below)
    -  **Stream name:** your stream key.
    -  **Share link:** link to your stream page, this will go into the clipboard once you start streaming.
    -  **User Agent:** pick XSplit/?.? (whichever version is available) or leave it blank. Shouldn't make any difference.
    -   **Video and Audio Encoding:** whatever you want. Go ahead and try increasing that VBV Max Bitrate! :)
    -   **Automatically record broadcast:** check this if you want to record the stream to your hard disk.
    -   **Interleave audio and video in one RTMP channel:** don't know what this does, just leave it unchecked and it should work fine.

Note: This is the URL for Twitch.tv, and assumes you're using the default server port (1935). If the relay server is listening to a different port, you must pass it in the URL, like `rtmp://localhost:port/app`. If you're streaming to another service, such as own3d.tv, you should also pass in the correct URL path (basically anything after the first single slash). For example, one of own3d.tv's RTMP URLs is `rtmp://live.use.own3d.tv/live`. In this case you should use `rtmp://localhost:port/live`.

Now test the bandwidth. You should no longer be artificially capped now (hopefully)! :)

Remember: whenever you want to stream to this Custom RTMP, make sure the TCP Relay server is running before starting the broadcast.

Now go ahead and stream your gaming sessions in high quality! :)


## Notes

Please note that this program cannot make bandwidth spawn out of nowhere; your internet connection is the limit, and as such I cannot guarantee improvements if the route from your home to the stream servers is slow. Make sure you pick the server closest to you for best results.

Also, it cannot help you if your CPU is overloaded. Lower your quality settings then try again.

Lastly, this program should not have any impact on the CPU whatsoever. All it does is copy some bytes from one place to the other. That's not exactly rocket science for current CPUs.

If something bad happens, run `tcprelay -debug -your-parameters` until you run into the issue and send me the resulting output.


## Troubleshooting

**Problem:** XSplit won't stream with TCPRelay. The bandwidth test shows this:

    Initializing connection to server...
    Trying to stream maximum data rate of #### kbps...
    Initializing transmission to server... (100%)
    Unable to connect to server.
    Aborted.

and trying to actually stream results in dropped frames every few seconds and nothing else.

**Solution:** First, make sure the TCPRelay is in the firewall exceptions. If the stream still doesn't work, try replacing "localhost" with your computer name or "127.0.0.1". The console version TCPRelay will try to get your computer name and display it at initialization, in a line like this:

    Server up at <machine-name>:1935

You can also find it in DxDiag, in the computer properties (WinKey + Pause) or by typing `hostname` in the Windows Command Prompt.

If you set the target address manually, make sure it is correct. TCPRelay will try to lookup the host name to ensure it is valid but will make no attempt to connect to it until you start a stream.

---

**Problem:** I'm using TCPRelay with XSplit and it improved my bandwidth, but I'm still getting yellow results.

**Solution:** Don't worry too much about these yellow results unless you are getting dropped frames. The XSplit Bandwidth Tester will show the amount of dropped frames at the end of the test. If it stays low (ideally zero) you're fine and TCPRelay is working as intended. [See the XSplit guide on the Bandwidth Tester](http://www.xsplit.com/broadcaster/help/index.html?bandwidth_tester_guide.htm):

*"If your video bitrate setting is generally low (<500 kbps) then you will often find that the bandwidth tester overshoots the target by a small margin and if your bitrate setting is equally high (>2000 kbps) then you may find that the tester slightly undershoots the target of MBR + ABR (don't ask why)."*

---

**Problem:** I cannot type anything in the TCPRelay console window.

**Solution:** The TCPRelay console window does not accept input. If you wish to pass parameters to it, you have to open a command prompt, navigate to its folder and then run TCPRelay with the desired parameters.

---

**Problem:** When I run TCPRelay, a command prompt window appears and immediately closes.

**Solution:** Try running TCPRelay from the command prompt (*Start* > *Run* > `cmd` > `cd` to the TCPRelay folder > `TCPRelayC.exe`). This should tell you what's going on.
Chances are some other application is using the listen port TCPRelay wants to use (by default 1935). TCPRelay will display the following message in this situation:
  
    Port <port> already in use. Check if TCPRelay is already running.

To solve this, open a command prompt and type `netstat -ano | find ":<port>"` (where <port> is 1935 or the listen port you configured). This command may or may not display lines like this:

    TCP    192.168.1.1:1935       12.34.56.78:12345      ESTABLISHED     4420

If such a line is displayed, check the second column. If it ends in :<port>, copy the last number in that line (in this case, 4420). Now type `tasklist | find "<the above number>"` and the process that is using the port should appear like this:

    some-process.exe            4420 Console                    1     12.788 K

Now you may decide whether to open the application and change its settings so that the port is no longer in use, close it, kill the process, or if any of these options are viable, change TCPRelay's listen port with the `-port:<port>` parameter and change the RTMP URL in XSplit to use the new port as explained above.

If nothing is displayed for the netstat command, you should try running it with elevated privileges (i.e. as an administrator), and if even then nothing shows up, try running TCPRelay again. Reboot your computer if the error persists.

Have fun streaming! :)

# TCPRelay v0.4 alpha 2

## Introduction

Many people have been plagued by XSplit Broadcaster's arbitrary upload bandwidth cap which severely limited the quality of their streams on many services, most commonly Twitch.tv. I was one of them.

Even though we were reporting success with other streaming methods, such as FMLE, there was no response from the XSplit team regarding this problem other than the limitation being on our side.

After tweaking a lot of settings on XSplit to see if I could squeeze a bit more bandwidth out of it, I decided to give up and try something else.

Just out of curiosity I wrote a quick'n'dirty program to serve as a relay between XSplit Broadcaster and tested it against Twitch.tv. The little program did nothing but transfer bytes from one side to the other. To my surprise, it worked on the first try. See for yourself:

Streaming directly to Twitch.tv with XSplit:
![Streaming directly to Twitch.tv with XSplit](http://i.imgur.com/Qlgv7.png)

Streaming to Twitch.tv through the relay:
![Streaming to Twitch.tv through the relay](http://i.imgur.com/lNh3Z.png)

I decided to improve upon the program and created a command-line tool out of it. Here it is!


## What's new in v0.4 alpha 2

- **FIXED:** -ttv now lists the default Twitch.tv server.
- **General**
    - **NEW:** partial localization support for:
        - [es-AR] Spanish (Argentina)  (thanks to Nicol?s Sigal)
        - [nl-NL] Dutch (The Netherlands)  (thanks to TalbotEv)
    Send me an email (ivan.rober@gmail.com) if you wish to add your language.
    NOTE: only the GUI version has support for localization for now.
- **GUI**
    - **FIXED:** status tooltip did not clear when TCPRelay was started sucessfully after an error.
    - **FIXED:** added localization support for several hard-coded strings.
    - **FIXED:** component layout updated manually for localization.
    - **FIXED:** old Twitch.tv ingest server list was shut down. Updated to the new Kraken REST API. Fixes an error when starting TCPRelay.

See the history.txt file for earlier versions.


## What's new in v0.1.1 beta

- **NEW:** You may now list all available Twitch.tv ingestion servers. Simply run `TCPRelay -ttv`. from the command line.
- **IMPROVED:** When providing a RTMP URL, TCPRelay will now display the custom RTMP URL you should use in XSplit. Useful for other services such as own3d.tv.


## Requirements

Windows
- [.NET Framework 4](http://www.microsoft.com/en-us/download/details.aspx?id=17851)

Linux
- Coming soon... hopefully! :)


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

**Solution:** First, make sure the Java Virtual Machine is in the firewall exceptions. If the stream still doesn't work, try replacing "localhost" with your computer name or "127.0.0.1". TCPRelay will try to get your computer name and display it at initialization, in a line like this:

    Server up at <machine-name>:1935

You can also find it in DxDiag or the computer properties (WinKey + Pause).

If you set the target address manually, make sure it is correct. TCPRelay will try to lookup the host name to ensure it is valid but will make no attempt to connect to it until you start a stream.

---

**Problem:** I'm using TCPRelay with XSplit and it improved my bandwidth, but I'm still getting yellow results.

**Solution:** Don't worry too much about these yellow results unless you are getting dropped frames. The XSplit Bandwidth Tester will show the amount of dropped frames at the end of the test. If it stays low (ideally zero) you're fine and TCPRelay is working as intended. [See the XSplit guide on the Bandwidth Tester](http://www.xsplit.com/broadcaster/help/index.html?bandwidth_tester_guide.htm):

*"If your video bitrate setting is generally low (<500 kbps) then you will often find that the bandwidth tester overshoots the target by a small margin and if your bitrate setting is equally high (>2000 kbps) then you may find that the tester slightly undershoots the target of MBR + ABR (don't ask why)."*

---

**Problem:** When I run TCPRelay, a command prompt window appears and immediately closes. If I try to run it from the command line, it says that the command was not recognized.

**Solution:** You do not have Java installed or it's not properly configured.
First, download the latest JRE (Java Runtime Environment) from [Oracle](http://www.oracle.com/technetwork/java/javase/downloads/jre-7u3-download-1501631.html). Don't forget to accept the license and pick the correct version for your OS.
Follow the installer instructions. When done, open a new command prompt window and type `java -version`. Something like this should come out:

    java version "1.6.0_25"
    Java(TM) SE Runtime Environment (build 1.6.0_25-b06)
    Java HotSpot(TM) 64-Bit Server VM (build 20.0-b11, mixed mode)

If it says that the command was not recognized, you will have to add Java to the PATH variable on your system. To do so, first find the installation path of your JRE (typically C:\Program Files\Java\jre###). Copy it for later.

Open the System Properties window (on Windows 7, right-click the *Computer* option in the Start menu, select *Properties*, then click *Advanced system configurations* on the left bar). Click the *Environment variables* button at the bottom of the window.

Go through the second list and look for the variable named *Path*. Choose it, click *Edit* below the list and append `;<your JRE path>\bin`. Don't forget the semicolon! Now click OK on everything, open a new command prompt and test the `java -version` command again.

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

## Testimonials

From http://www.xsplit.com/forum/viewtopic.php?f=2&t=3701 and http://www.xsplit.com/forum/viewtopic.php?f=2&t=7159


akitaneru:
> Just downloaded it and tested it today, and it worked a treat for me.
> Went from a fluctuating bandwidth of 1200-2700 to a constant 4000. Needed a constant 3k, so this has solved my streaming problems quite handily.

MirrorR:
> Thanks man!!!! This is really helpful, I was capped at 1600kbps before, this solution let me have no capped now, I can full upload (like 4000kbps) right now. good job!

operasaikyo:
> I am a Japanese. I live in Japan. Tested the proxy program from my own connection(210mbdown/190mbup) located in Osaka Japan.
> I tried test stream to the 'Asia Backup Server'. Has become such a great result. Thanks for the help!
>
> His results:
> http://i.imgur.com/6zUCB.png
> http://i.imgur.com/pBiAY.png

Lillsjon (via PM on the XSplit forums):
> Thank you very much, It went from red to green and works fine with 2500 + 320 with audio on own3d now, excellent fix :D

JESUSatWork:
> Confirmed working for me. Tested 5192 Kbps earlier, and with TCP relay (set it to my regular server instead of SF) I could pull the full bandwidth, and dropped 0 frames during the bandwidth test (though I did get a yellow rating). Then tested straight TTV bandwidth stream, same settings and everything. Could only pull 4400 Kbps and I got a red rating with 140 frames dropped. Great work :D

jun-fu-wu:
> im from Taiwan i use asia backup server , Last week my max bitrate about 3000kbps but this week i just can 1300kbps
> then i used this program,I was able to use 3000kbps
> thank you :D

... and many more at the XSplit forums!

## Contact

- [E-mail](mailto:ivan.rober@gmail.com)
- [Twitter (@StrikerX3)](https://twitter.com/StrikerX3)
- [XSplit forums](http://www.xsplit.com/forum) ([StrikerX3](https://support.xsplit.com/forum/memberlist.php?mode=viewprofile&u=51690)) ([Official topic](http://www.xsplit.com/forum/viewtopic.php?f=2&t=7159))
- [My blog](http://strikerx3.blogspot.com/)
- [Twitch.tv](http://www.twitch.tv/strikerx3) (shameless plug! :D)

Have fun streaming! :)

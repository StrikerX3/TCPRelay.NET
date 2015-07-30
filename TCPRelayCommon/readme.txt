TCPRelay v0.4 beta 3
  by Ivan "StrikerX3" Oliveira


Introduction
------------
TCPRelay's main purpose is to serve as an intermediator between streaming
programs and the actual streaming servers. For some reason certain streaming
programs have poor network handling code and are unable to push enough data to
the RTMP servers even though users have more than enough upload speed.

TCPRelay started out as a quick'n'dirty program just to test XSplit's upload
bandwidth issues. I was surprised to see it worked. It works by relaying TCP
streams between an application and the destination server, allowing users to
tune the socket parameters for improved performance.

Streaming directly to Justin.tv with XSplit: http://i.imgur.com/Qlgv7.png
Streaming to Justin.tv through the relay: http://i.imgur.com/lNh3Z.png


What's new in v0.4 beta 3
-------------------------
- General
  - NEW: partial localization support for:
    - [es-AR] Spanish (Argentina)  (thanks to Nicolás Sigal)
    - [nl-NL] Dutch (The Netherlands)  (thanks to TalbotEv)
    Send me an email (ivan.rober@gmail.com) if you wish to add your language.
    NOTE: only the GUI version has support for localization for now.
  - NEW: [a3] socket send buffer size can be tweaked from the console and GUI
    - Console: use the -sbs:## parameter
    - GUI: use the newly added Send Buffer field to adjust the buffer size
    - The default size is 8 KB
    - Increasing this might help reduce or eliminate dropped frames, especially
      on connections with high latency to the server
  - FIXED: [a4] application no longer crashes on startup if there's no BindIP
      address in the Registry
- GUI
  - FIXED: [a1] status tooltip did not clear when TCPRelay was started
    sucessfully after an error.
  - FIXED: [a1] added localization support for several hard-coded strings.
  - FIXED: [a1] component layout updated manually for localization. Components
    should no longer overlap.
  - FIXED: [a2] old Twitch.tv ingest server list was shut down. Updated to the
    new Kraken REST API. Fixes an error when starting TCPRelay.
  - NEW: [a4] added a new Advanced Settings window for tuning socket parameters
    - Socket Send Buffer Size moved to this window
    - Added Receive Buffer Size
    - Added No Delay
    - Added Connection Timeout
    - Can set up parameters for both the application- and the remote-facing
      sockets
  - NEW: [b1] added an option to allow binding to a different IP address in the
    Advanced Settings window
  - IMPROVED: [b2] cosmetic improvements to the Advanced Settings window
  - FIXED: [b2] Advanced Settings window would fail to open again if closed by
    clicking the X button

See the history.txt file for earlier versions.


Requirements
------------
Windows
- .NET Framework 4: http://www.microsoft.com/en-us/download/details.aspx?id=17851

Linux
- coming soon... hopefully! :)


TCPRelay GUI Instructions
-------------------------
Run TCPRelay.exe to open the GUI. The Target RTMP URL comes with the default
target server and is populated with a list of all Twitch.tv servers as soon as
possible. Click "Load Twitch.tv servers" if you wish to refresh the list.
The listen port is the port XSplit will have to connect to. Most people won't
need to change this.
Click the Start button to start the relay. As soon XSplit connects to TCPRelay,
a new connection will appear in the Connections panel displaying data transfer
information in real time.


TCPRelay Console Instructions
-----------------------------
Simply run TCPRelayC.exe to start a TCP relay server listening to port 1935 and
targeting live.justin.tv:1935, which should work for most people.
If you wish to change the target server, run TCPRelayC.exe with the parameter
-th:<server host name> (eg. for Justin.tv New York, use -th:live-jfk.justin.tv).
You can also pass in the RTMP URL such as rtmp://live.justin.tv/app; the target
host and port will be set based on it.

Create a shortcut to TCPRelayC.exe with the desired parameters if you want to
use custom settings without using the command line.

If you wish to list all available Twitch.tv ingest servers, run
  TCPRelayC -ttv
from the command line.

Run TCPRelayC -? from your command prompt to get more help about the parameters.


XSplit Instructions
-------------------
First, make sure the TCP Relay server is up and running and pointing to the
desired server.

Open XSplit.
On the main window, go to Broadcast > Edit channels...
On the User Settings window, click Add... > Custom RTMP.
Fill the fields as follows:
- Name: anything, this will show up in the Broadcast menu.
- Description: anything, optional.
- RTMP URL: rtmp://localhost/app   (See note below)
- Stream name: your stream key.
- Share link: link to your stream page, this will go into the clipboard once you
    start streaming.
- User Agent: pick XSplit/?.? (whichever version is available) or leave it
    blank. Shouldn't make any difference.
- Video and Audio Encoding: whatever you want. Go ahead and try increasing that
    VBV Max Bitrate! :)
- Automatically record broadcast: check this if you want to record the stream to
    your hard disk.
- Interleave audio and video in one RTMP channel: don't know what this does,
    just leave it unchecked and it should work fine.

Note: This is the URL for Justin.tv, and assumes you're using the default server
      port (1935). If the relay server is listening to a different port, you
      must pass it in the URL, like this:
        rtmp://localhost:port/app
      If you're streaming to another service, such as own3d.tv, you should also
      pass in the correct URL path (basically anything after the first single
      slash). For example, one of own3d.tv's RTMP URLs is
        rtmp://live.use.own3d.tv/live
      In this case you should use this:
        rtmp://localhost:port/live

Now test the bandwidth. You should no longer be artificially capped now
(hopefully)! :)

Remember: whenever you want to stream to this Custom RTMP, make sure the TCP
Relay server is running before starting the broadcast.

Now go ahead and stream your gaming sessions in high quality! :)


Notes
-----
Please note that this program cannot make bandwidth spawn out of nowhere; your
internet connection is the limit, and as such I cannot guarantee improvements if
the route from your home to the stream servers is slow. Make sure you pick the
server closest to you for best results.

Also, it cannot help you if your CPU is overloaded. Lower your quality settings
then try again.

Lastly, this program should not have any impact on the CPU whatsoever. All it
does is copy some bytes from one place to the other. That's not exactly rocket
science for current CPUs.

If something bad happens, run "TCPRelayC -debug -your-parameters" until you run
into the issue and send me the resulting output.


Troubleshooting
---------------
Problem: XSplit won't stream with TCPRelay. The bandwidth test shows this:

    Initializing connection to server...
    Trying to stream maximum data rate of #### kbps...
    Initializing transmission to server... (100%)
    Unable to connect to server.
    Aborted.

  and trying to actually stream results in dropped frames every few seconds and
  nothing else.

Solution: first, make sure TCPRelay is in the firewall exceptions.
  If the stream still doesn't work, try replacing "localhost" with your computer
  name or "127.0.0.1".
  TCPRelay will try to get your computer name and display it at initialization,
  in a line like this:

    Server up at <machine-name>:1935

  You can also find it in DxDiag, in the computer properties (WinKey + Pause) or
  by typing "hostname" in the Windows Command Prompt.

  If you set the target address manually, make sure it is correct. TCPRelay will
  try to lookup the host name to ensure it is valid but will make no attempt to
  connect to it until you start a stream.


Problem: I'm using TCPRelay with XSplit and it improved my bandwidth, but I'm
  still getting yellow results.

Solution: Don't worry too much about these yellow results unless you are getting
  dropped frames. The XSplit Bandwidth Tester will show the amount of dropped
  frames at the end of the test. If it stays low (ideally zero) you're fine and
  TCPRelay is working as intended. See the XSplit guide on the Bandwidth Tester:

  http://www.xsplit.com/broadcaster/help/index.html?bandwidth_tester_guide.htm

   "If your video bitrate setting is generally low (<500 kbps) then you will
    often find that the bandwidth tester overshoots the target by a small
    margin and if your bitrate setting is equally high (>2000 kbps) then you
    may find that the tester slightly undershoots the target of MBR + ABR
    (don't ask why)."


Solution: Don't worry too much about these yellow results unless you are getting
  dropped frames. The XSplit Bandwidth Tester will show the amount of dropped
  frames at the end of the test. If it stays low (ideally zero) you're fine and
  TCPRelay is working as intended. See the XSplit guide on the Bandwidth Tester:

  http://www.xsplit.com/broadcaster/help/index.html?bandwidth_tester_guide.htm

   "If your video bitrate setting is generally low (<500 kbps) then you will
    often find that the bandwidth tester overshoots the target by a small
    margin and if your bitrate setting is equally high (>2000 kbps) then you
    may find that the tester slightly undershoots the target of MBR + ABR
    (don't ask why)."


Problem: I cannot type anything in the TCPRelay console window.

Solution: The TCPRelay console window does not accept input. If you wish to
  pass parameters to it, you have to open a command prompt, navigate to its
  folder and then run TCPRelay with the desired parameters.


Problem: When I run TCPRelay, a command prompt window appears and immediately
  closes.

Solution: Try running TCPRelay from the command prompt (Run > cmd > cd to the
  TCPRelay folder > TCPRelayC.exe). This should tell you what's going on.
  
  Chances are some other application is using the listen port TCPRelay wants
  to use (by default 1935). TCPRelay will display the following message in
  this situation:
  
    Port <port> already in use. Check if TCPRelay is already running.

  To solve this, open a command prompt and type:

    netstat -ano | find ":<port>"
  (where <port> is 1935 or the listen port you configured)

  This command may or may not display lines like this:

    TCP    192.168.1.1:1935       12.34.56.78:12345      ESTABLISHED     4420

  If such a line is displayed, check the second column. If it ends in :<port>,
  copy the last number in that line (in this case, 4420). Now type:

    tasklist | find "<the above number>"

  and the process that is using the port should appear like this:

    some-process.exe            4420 Console                    1     12.788 K

  Now you may decide whether to open the application and change its settings
  so that the port is no longer in use, close it, kill the process, or if any
  of these options are viable, change TCPRelay's listen port with the -port:
  parameter and change the RTMP URL in XSplit to use the new port as explained
  above.

  If nothing is displayed for the netstat command, you should try running it
  with elevated privileges (i.e. as an administrator), and if even then
  nothing shows up, try running TCPRelay again. Reboot your computer if the
  error persists.
  

Testimonials
------------
From http://www.xsplit.com/forum/viewtopic.php?f=2&t=3701
 and http://www.xsplit.com/forum/viewtopic.php?f=2&t=7159


akitaneru:
   "Just downloaded it and tested it today, and it worked a treat for me.
    Went from a fluctuating bandwidth of 1200-2700 to a constant 4000. Needed
    a constant 3k, so this has solved my streaming problems quite handily."


MirrorR:
   "Thanks man!!!! This is really helpful, I was capped at 1600kbps before, this
    solution let me have no capped now, I can full upload (like 4000kbps) right
    now. good job!"


operasaikyo:
   "I am a Japanese. I live in Japan.
    Tested the proxy program from my own connection(210mbdown/190mbup) located
    in Osaka Japan.

    I tried test stream to the 'Asia Backup Server'.
    Has become such a great result.
    Thanks for the help!"

   His results:
     http://i.imgur.com/6zUCB.png
     http://i.imgur.com/pBiAY.png


Lillsjon (via PM on the XSplit forums):
   "Thank you very much,
    It went from red to green and works fine with 2500 + 320 with audio on own3d
    now, excellent fix :D"


JESUSatWork:
   "Confirmed working for me. Tested 5192 Kbps earlier, and with TCP relay (set
    it to my regular server instead of SF) I could pull the full bandwidth, and
    dropped 0 frames during the bandwidth test (though I did get a yellow
    rating). Then tested straight TTV bandwidth stream, same settings and
    everything. Could only pull 4400 Kbps and I got a red rating with 140 frames
    dropped. Great work :D"


jun-fu-wu:
   "im from Taiwan
    i use asia backup server , Last week my max bitrate about 3000kbps
    but this week i just can 1300kbps

    then i used this program,I was able to use 3000kbps

    thank you :D"


... and many more at the XSplit forums!


Contact
-------
Contact me by e-mail: ivan.rober@gmail.com
  or on Twitter: @StrikerX3
  or on the XSplit forums at http://www.xsplit.com/forum (StrikerX3)
    Official topic: http://www.xsplit.com/forum/viewtopic.php?f=2&t=7159
  or on my blog: http://strikerx3.blogspot.com/
  or on Twitch.tv (shameless plug! :D): http://www.twitch.tv/strikerx3
  or on YouTube (more shameless plug!): https://www.youtube.com/c/StrikerX3

Have fun streaming! :)

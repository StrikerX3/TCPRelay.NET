md out
md out\x86
md out\x64

copy TCPRelay\bin\Release\TCPRelay.exe out\x86
copy TCPRelay\bin\Release\Newtonsoft.Json.dll out\x86
copy TCPRelay\bin\Release\TCPRelayControls.dll out\x86
copy TCPRelay\bin\Release\TCPRelayCommon.dll out\x86
xcopy TCPRelay\bin\Release\pt-BR out\x86\pt-BR /y /i
xcopy TCPRelay\bin\Release\es-AR out\x86\es-AR /y /i
xcopy TCPRelay\bin\Release\nl-NL out\x86\nl-NL /y /i
copy TCPRelayCommon\readme.txt out\x86
copy TCPRelayCommon\history.txt out\x86
copy TCPRelayConsole\bin\Release\TCPRelayC.exe out\x86

copy TCPRelay\bin\x64\Release\TCPRelay.exe out\x64
copy TCPRelay\bin\x64\Release\Newtonsoft.Json.dll out\x64
copy TCPRelay\bin\x64\Release\TCPRelayControls.dll out\x64
copy TCPRelay\bin\x64\Release\TCPRelayCommon.dll out\x64
xcopy TCPRelay\bin\x64\Release\pt-BR out\x64\pt-BR /y /i
xcopy TCPRelay\bin\x64\Release\es-AR out\x64\es-AR /y /i
xcopy TCPRelay\bin\x64\Release\nl-NL out\x64\nl-NL /y /i
copy TCPRelayCommon\readme.txt out\x64
copy TCPRelayCommon\history.txt out\x64
copy TCPRelayConsole\bin\x64\Release\TCPRelayC.exe out\x64

Set WshShell = WScript.CreateObject("WScript.Shell")
If WScript.Arguments.length = 0 Then
Set ObjShell = CreateObject("Shell.Application")
ObjShell.ShellExecute "wscript.exe", """" & _
WScript.ScriptFullName & """" &_
 " RunAsAdministrator", , "runas", 1
Else
Set objFSO = CreateObject("Scripting.FileSystemObject")
dim fso: set fso = CreateObject("Scripting.FileSystemObject")
strUser = CreateObject("WScript.Network").UserName
If objFSO.FolderExists("C:\Users\" & strUser & "\Documents\RubikCube") Then
else
set objFldr = objFSO.CreateFolder("C:\Users\" & strUser & "\Documents\RubikCube")
end if
Path = fso.BuildPath("C:\Users\User\Documents\RubikCube", "save.xml")
If (objFSO.FileExists(Path)) Then
Else
Set objFile = objFSO.CreateTextFile(Path)
objFile.Write("<?xml version=")
objFile.Write(chr(34))
objFile.Write("1.0")
objFile.Write(chr(34))
objFile.Write(" encoding=""")
objFile.Write("utf-8")
objFile.Write(chr(34))
objFile.WriteLine("?>")
objFile.WriteLine("<root>")
objFile.WriteLine("</root>")
objFile.Close()
end if
End if
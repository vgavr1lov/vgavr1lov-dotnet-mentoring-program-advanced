Set fso = CreateObject("Scripting.FileSystemObject")

repoRoot = fso.GetParentFolderName(fso.GetParentFolderName(WScript.ScriptFullName))

src = repoRoot & "\scripts\pre-push"
dst = repoRoot & "\.git\hooks\pre-push"

fso.CopyFile src, dst

WScript.Echo "Git hooks installed."

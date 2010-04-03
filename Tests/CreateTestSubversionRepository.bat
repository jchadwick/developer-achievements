@echo off
RMDIR /S /Q project
RMDIR /S /Q C:\Temp\repos
RMDIR /S /Q C:\Temp\repos_temp
MKDIR C:\Temp
MKDIR project
ECHO "This is a test file" > project\test.txt
..\References\svn\svnadmin create C:\Temp\repos
..\References\svn\svn import project "file:///C:/Temp/repos/" -m "Creating test repository"
RMDIR /S /Q project
ECHO "Finished"
PAUSE
@echo off

SET reposDir=C:\Temp\repos
SET reposUri=file:///C:/Temp/repos

RMDIR /S /Q project 2>NUL
RMDIR /S /Q %reposDir% 2>NUL

MKDIR %reposDir%
MKDIR project
ECHO "This is a test file" > project\test.txt
..\References\svn\svnadmin create %reposDir%
..\References\svn\svn import project "%reposUri%" -m "Creating test repository"
RMDIR /S /Q project

ECHO Finished - created repository at %reposDir%
PAUSE

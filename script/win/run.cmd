:: Script for run an application for Windows

echo off

call killprocess.cmd

cpe --name="proxy.127.0.0.1.5000" --ip="127.0.0.1" --port="5000" --cu="user" --cp="password"

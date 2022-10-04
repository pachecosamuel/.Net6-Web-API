// See https://aka.ms/new-console-template for more information
using FluentColorConsole;

Console.WriteLine("Hello, World!");

ShowMessage showMessage = new();

showMessage.ShowMessageConsole();

var textMessage = ColorConsole.WithRedText;
textMessage.WriteLine("Using my first nuGet package");



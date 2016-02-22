namespace OpenTraceViewer

module Main = 
    open System
    open Gtk
    
    [<EntryPoint>]
    let Main(args) = 
        Application.Init()

        let win = new MainWindow.MainWindow()

        win.Show()
        Application.Run()
        0


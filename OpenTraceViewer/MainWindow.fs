namespace OpenTraceViewer

module MainWindow = 
    open System
    open System.Collections
    open System.IO
    open Gtk
    open OxyPlot
    open OxyPlot.Series
    open OxyPlot.GtkSharp
    open OpenTraceViewer.Data
    
    type MainWindow() as this = 
        inherit Window("Open Trace Viewer")

        let ds = new DataStore()
        let plot = new Plotter()
        let list = new ListView()

        do this.Initialize

        member this.DataStore = ds
        member this.Plotter = plot
        member this.ListView = list
       
        member this.Open() =
            let fileDialog = new FileChooserDialog("Choose a log file to open", 
                                                   this, 
                                                   FileChooserAction.Open, 
                                                   "Cancel", ResponseType.Cancel,
                                                   "Open",ResponseType.Accept)
            
            if fileDialog.Run() = (int)ResponseType.Accept then
                let file = new FileInfo(fileDialog.Filename)
                this.DataStore.AddFile(file)

                let data = this.DataStore.GetData(0, 1, 1)

                this.Plotter.AddDataToPlot(data)
                this.ListView.AddValues(data)

            fileDialog.Destroy()

        member private this.CreateMenuBar() =
            let navbar = new MenuBar()

            let fileMenu = new Menu()
            let fileItem = new MenuItem("File");
            fileItem.Submenu <- fileMenu

            let connectItem = new MenuItem("Open File")
            connectItem.Activated.Add(fun eventArgs -> this.Open())
            fileMenu.Append(connectItem)

            let exitItem = new MenuItem("Exit")
            exitItem.Activated.Add(fun evArgs -> this.OnDeleteEvent(null, null))
            fileMenu.Append(exitItem)

            navbar.Append(fileItem)
            navbar       

        member this.Initialize =
            this.SetDefaultSize(800, 600)

            let boxLayout = new VBox()

            // Create and attach menu bar
            let navbar = this.CreateMenuBar()
            boxLayout.PackStart(navbar, false, false, (uint32 0))

            //Create and attach plot
            boxLayout.Add(this.Plotter.Initialize())

            // Create and attach list view
            boxLayout.Add(this.ListView.Initialize())

            this.Add(boxLayout)              

            this.DeleteEvent.AddHandler(fun o e -> this.OnDeleteEvent(o, e))
            this.ShowAll()

        member this.OnDeleteEvent(o, e : DeleteEventArgs) = 
            Application.Quit()
            e.RetVal <- true
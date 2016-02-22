namespace OpenTraceViewer

module MainWindow = 
    open System
    open System.Collections
    open System.IO
    open Gtk
    open OxyPlot
    open OxyPlot.Series
    open OxyPlot.GtkSharp

    type MainWindow() as this = 
        inherit Window("Open Trace Viewer")

        do this.Initialize

        member this.Open() =
            let fileDialog = new FileChooserDialog("Choose a log file to open", 
                                                   this, 
                                                   FileChooserAction.Open, 
                                                   "Cancel", ResponseType.Cancel,
                                                   "Open",ResponseType.Accept)
            
            if fileDialog.Run() = (int)ResponseType.Accept then
                let file = File.OpenRead(fileDialog.Filename)
                file.Close()
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

        member private this.CreatePlot() =
            let view = new OxyPlot.GtkSharp.PlotView()
            let model = new PlotModel(Title = "Logs", Subtitle = "Graph view of log entrys - by category")

            let traceSeries = new LineSeries()
            traceSeries.Title <- "Trace"
            traceSeries.CanTrackerInterpolatePoints <- false
            traceSeries.Color <- OxyColors.ForestGreen
            traceSeries.MarkerType <- MarkerType.Circle
            traceSeries.MarkerSize <- 1.0
            traceSeries.MarkerStroke <-OxyColors.White
            traceSeries.MarkerFill <- OxyColors.ForestGreen
            traceSeries.MarkerStrokeThickness <- 1.0

            let infoSeries = new LineSeries()
            infoSeries.Title <- "Info"
            infoSeries.CanTrackerInterpolatePoints <- false
            infoSeries.Color <- OxyColors.SkyBlue
            infoSeries.MarkerType <- MarkerType.Circle
            infoSeries.MarkerSize <- 1.0
            infoSeries.MarkerStroke <-OxyColors.White
            infoSeries.MarkerFill <- OxyColors.SkyBlue
            infoSeries.MarkerStrokeThickness <- 1.0

            let warnSeries = new LineSeries()
            warnSeries.Title <- "Warn"
            warnSeries.CanTrackerInterpolatePoints <- false
            warnSeries.Color <- OxyColors.LightGoldenrodYellow
            warnSeries.MarkerType <- MarkerType.Circle
            warnSeries.MarkerSize <- 1.0
            warnSeries.MarkerStroke <-OxyColors.White
            warnSeries.MarkerFill <- OxyColors.LightGoldenrodYellow
            warnSeries.MarkerStrokeThickness <- 1.0

            let errorSeries = new LineSeries()
            errorSeries.Title <- "Error"
            errorSeries.CanTrackerInterpolatePoints <- false
            errorSeries.Color <- OxyColors.IndianRed
            errorSeries.MarkerType <- MarkerType.Circle
            errorSeries.MarkerSize <- 1.0
            errorSeries.MarkerStroke <-OxyColors.White
            errorSeries.MarkerFill <- OxyColors.IndianRed
            errorSeries.MarkerStrokeThickness <- 1.0

            [for x in 0.0 .. 100.0 -> (x, x*x)] |> Seq.iter(fun (x,y) -> traceSeries.Points.Add(new DataPoint(x, y)))
            [for x in 0.0 .. 80.0 -> (x, x*x)] |> Seq.iter(fun (x,y) -> infoSeries.Points.Add(new DataPoint(x, y)))
            [for x in 0.0 .. 50.0 -> (x, x*x)] |> Seq.iter(fun (x,y) -> warnSeries.Points.Add(new DataPoint(x, y)))
            [for x in 0.0 .. 10.0 -> (x, x*x)] |> Seq.iter(fun (x,y) -> errorSeries.Points.Add(new DataPoint(x, y)))

            model.Series.Add(traceSeries)
            model.Series.Add(infoSeries)
            model.Series.Add(warnSeries)
            model.Series.Add(errorSeries)

            view.Model <- model
            view

        member private this.CreateList() =
            let view = new TreeView()

            let dateCol = new TreeViewColumn()
            let dateCell = new CellRendererText()
            dateCol.Title <- "Date"
            dateCol.PackStart(dateCell, true)
            dateCol.AddAttribute(dateCell, "text", 0)
            dateCol.SetCellDataFunc(dateCell, 
                (fun (coll:TreeViewColumn)(cell:CellRenderer)(model:TreeModel)(iter:TreeIter) -> (cell:?>CellRendererText).Text <- (model.GetValue(iter, 0).ToString())))

            let levelCol = new TreeViewColumn()
            let levelCell = new CellRendererText()
            levelCol.Title <- "Level"
            levelCol.PackStart(levelCell, true)
            levelCol.AddAttribute(levelCell, "text", 1)

            let messageCol = new TreeViewColumn()
            let messageCell = new CellRendererText()
            messageCol.Title <- "Message"
            messageCol.PackStart(messageCell, true)
            messageCol.AddAttribute(messageCell, "text", 2)

            view.AppendColumn(dateCol) |> ignore
            view.AppendColumn(levelCol) |> ignore
            view.AppendColumn(messageCol) |> ignore

            let store = new ListStore(typeof<DateTime>, typeof<string>, typeof<string>)
            view.Model <- store

            store.AppendValues(new DateTime(2016, 2, 20), "Trace", "Trace Message") |> ignore

            view

        member this.Initialize =
            this.SetDefaultSize(800, 600)

            let boxLayout = new VBox()

            // Create and attach menu bar
            let navbar = this.CreateMenuBar()
            boxLayout.PackStart(navbar, false, false, (uint32 0))

            //Create and attach plot
            let plot = this.CreatePlot()
            boxLayout.Add(plot)

            // Create and attach list view
            let list = this.CreateList()
            boxLayout.Add(list)

            this.Add(boxLayout)              

            this.DeleteEvent.AddHandler(fun o e -> this.OnDeleteEvent(o, e))
            this.ShowAll()

        member this.OnDeleteEvent(o, e : DeleteEventArgs) = 
            Application.Quit()
            e.RetVal <- true

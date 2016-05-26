namespace OpenTraceViewer

open System
open System.Collections
open OxyPlot
open OxyPlot.GtkSharp
open OxyPlot.Series
open OxyPlot.Axes
open NLog

type Plotter() as this = 
    let view = new PlotView()
    let model = new OxyPlot.PlotModel(Title = "Logs", Subtitle = "Graph view of log entrys - by category")
    let trace = new LineSeries()
    let info = new LineSeries()
    let warn = new LineSeries()
    let error = new LineSeries()

    do this.Initialize

    member private this.View = view
    member private this.Model = model
    member private this.TraceSeries = trace
    member private this.InfoSeries = info
    member private this.WarnSeries = warn
    member private this.ErrorSeries = error
    member private this.Logger = LogManager.GetCurrentClassLogger()

    member this.GetView = 
        this.View

    member private this.Initialize =
            this.TraceSeries.Title <- "Trace"
            this.TraceSeries.CanTrackerInterpolatePoints <- false
            this.TraceSeries.Color <- OxyPlot.OxyColors.ForestGreen
            this.TraceSeries.MarkerType <- OxyPlot.MarkerType.Circle
            this.TraceSeries.MarkerSize <- 3.0
            this.TraceSeries.MarkerStroke <-OxyPlot.OxyColors.White
            this.TraceSeries.MarkerFill <- OxyPlot.OxyColors.ForestGreen
            this.TraceSeries.MarkerStrokeThickness <- 1.0

            this.InfoSeries.Title <- "Info"
            this.InfoSeries.CanTrackerInterpolatePoints <- false
            this.InfoSeries.Color <- OxyPlot.OxyColors.SkyBlue
            this.InfoSeries.MarkerType <- OxyPlot.MarkerType.Circle
            this.InfoSeries.MarkerSize <- 3.0
            this.InfoSeries.MarkerStroke <-OxyPlot.OxyColors.White
            this.InfoSeries.MarkerFill <- OxyPlot.OxyColors.SkyBlue
            this.InfoSeries.MarkerStrokeThickness <- 1.0

            this.WarnSeries.Title <- "Warn"
            this.WarnSeries.CanTrackerInterpolatePoints <- false
            this.WarnSeries.Color <- OxyPlot.OxyColors.LightGoldenrodYellow
            this.WarnSeries.MarkerType <- OxyPlot.MarkerType.Circle
            this.WarnSeries.MarkerSize <- 3.0
            this.WarnSeries.MarkerStroke <-OxyPlot.OxyColors.White
            this.WarnSeries.MarkerFill <- OxyPlot.OxyColors.LightGoldenrodYellow
            this.WarnSeries.MarkerStrokeThickness <- 1.0

            this.ErrorSeries.Title <- "Error"
            this.ErrorSeries.CanTrackerInterpolatePoints <- false
            this.ErrorSeries.Color <- OxyPlot.OxyColors.IndianRed
            this.ErrorSeries.MarkerType <- OxyPlot.MarkerType.Circle
            this.ErrorSeries.MarkerSize <- 3.0
            this.ErrorSeries.MarkerStroke <-OxyPlot.OxyColors.White
            this.ErrorSeries.MarkerFill <- OxyPlot.OxyColors.IndianRed
            this.ErrorSeries.MarkerStrokeThickness <- 1.0

            this.Model.Series.Add(this.TraceSeries)
            this.Model.Series.Add(this.InfoSeries)
            this.Model.Series.Add(this.WarnSeries)
            this.Model.Series.Add(this.ErrorSeries)

            this.Model.Axes.Add(new DateTimeAxis(Position = AxisPosition.Bottom, Title = "Log Date", Minimum = 0.0))
            this.Model.Axes.Add(new LinearAxis(Position = AxisPosition.Left, Title = "Log Level Entry Count", Minimum = 0.0))

            this.View.Model <- model

    member public this.AddDataToPlot(rows:seq<DataRow>) =
        let firstRow = Seq.head rows
        let lastRow = Seq.last rows

        this.Model.DefaultXAxis.Minimum <- DateTimeAxis.ToDouble firstRow.Date
        this.Model.DefaultXAxis.Maximum <- DateTimeAxis.ToDouble lastRow.Date

        this.View.InvalidatePlot true
        this.Model.InvalidatePlot true

        rows 
        |> Seq.iter (fun row ->
            match row.TraceLevel with
            | Trace -> 
                this.Logger.Trace(sprintf "Adding row with tracelevel Trace to plot")
                this.TraceSeries.Points.Add(DateTimeAxis.CreateDataPoint(row.Date, (float)this.TraceSeries.Points.Count + 1.0))
            | Info -> 
                this.Logger.Trace(sprintf "Adding row with tracelevel Info to plot")
                this.InfoSeries.Points.Add(DateTimeAxis.CreateDataPoint(row.Date, (float)this.InfoSeries.Points.Count + 1.0))
            | Warn -> 
                this.Logger.Trace(sprintf "Adding row with tracelevel Warn to plot")
                this.WarnSeries.Points.Add(DateTimeAxis.CreateDataPoint(row.Date, (float)this.WarnSeries.Points.Count + 1.0))
            | Error -> 
                this.Logger.Trace(sprintf "Adding row with tracelevel Error to plot")
                this.ErrorSeries.Points.Add(DateTimeAxis.CreateDataPoint(row.Date, (float)this.ErrorSeries.Points.Count + 1.0))
            )
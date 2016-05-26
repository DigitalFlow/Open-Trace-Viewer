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
            this.TraceSeries.Color <- OxyColors.ForestGreen
            this.TraceSeries.MarkerType <- MarkerType.Circle
            this.TraceSeries.MarkerSize <- 3.0
            this.TraceSeries.MarkerStroke <-OxyColors.White
            this.TraceSeries.MarkerFill <- OxyColors.ForestGreen
            this.TraceSeries.MarkerStrokeThickness <- 1.0

            this.InfoSeries.Title <- "Info"
            this.InfoSeries.CanTrackerInterpolatePoints <- false
            this.InfoSeries.Color <- OxyColors.SkyBlue
            this.InfoSeries.MarkerType <- MarkerType.Circle
            this.InfoSeries.MarkerSize <- 3.0
            this.InfoSeries.MarkerStroke <-OxyColors.White
            this.InfoSeries.MarkerFill <- OxyColors.SkyBlue
            this.InfoSeries.MarkerStrokeThickness <- 1.0

            this.WarnSeries.Title <- "Warn"
            this.WarnSeries.CanTrackerInterpolatePoints <- false
            this.WarnSeries.Color <- OxyColors.LightGoldenrodYellow
            this.WarnSeries.MarkerType <- MarkerType.Circle
            this.WarnSeries.MarkerSize <- 3.0
            this.WarnSeries.MarkerStroke <-OxyColors.White
            this.WarnSeries.MarkerFill <- OxyColors.LightGoldenrodYellow
            this.WarnSeries.MarkerStrokeThickness <- 1.0

            this.ErrorSeries.Title <- "Error"
            this.ErrorSeries.CanTrackerInterpolatePoints <- false
            this.ErrorSeries.Color <- OxyColors.IndianRed
            this.ErrorSeries.MarkerType <- MarkerType.Circle
            this.ErrorSeries.MarkerSize <- 3.0
            this.ErrorSeries.MarkerStroke <-OxyColors.White
            this.ErrorSeries.MarkerFill <- OxyColors.IndianRed
            this.ErrorSeries.MarkerStrokeThickness <- 1.0

            this.Model.Series.Add(this.TraceSeries)
            this.Model.Series.Add(this.InfoSeries)
            this.Model.Series.Add(this.WarnSeries)
            this.Model.Series.Add(this.ErrorSeries)

            this.Model.Axes.Add(new DateTimeAxis(Position = AxisPosition.Bottom, Title = "Log Date", Minimum = 0.0, Maximum = 100.0))
            this.Model.Axes.Add(new LinearAxis(Position = AxisPosition.Left, Title = "Log Level Entry Count", Minimum = 0.0, Maximum = 100.0))

            this.View.Model <- model

        member public this.AddDataToPlot(rows:seq<DataRow>) =
            let firstRow = rows |> Seq.head

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
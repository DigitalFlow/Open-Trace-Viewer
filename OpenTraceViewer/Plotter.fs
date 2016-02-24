namespace OpenTraceViewer

open System
open OxyPlot
open OxyPlot.GtkSharp
open OxyPlot.Series

type Plotter() = 
    member this.TraceSeries = new LineSeries()
    member this.InfoSeries = new LineSeries()
    member this.WarnSeries = new LineSeries()
    member this.ErrorSeries = new LineSeries()

    member this.CreatePlot() =
            let view = new OxyPlot.GtkSharp.PlotView()
            let model = new PlotModel(Title = "Logs", Subtitle = "Graph view of log entrys - by category")

            this.TraceSeries.Title <- "Trace"
            this.TraceSeries.CanTrackerInterpolatePoints <- false
            this.TraceSeries.Color <- OxyColors.ForestGreen
            this.TraceSeries.MarkerType <- MarkerType.Circle
            this.TraceSeries.MarkerSize <- 1.0
            this.TraceSeries.MarkerStroke <-OxyColors.White
            this.TraceSeries.MarkerFill <- OxyColors.ForestGreen
            this.TraceSeries.MarkerStrokeThickness <- 1.0

            this.InfoSeries.Title <- "Info"
            this.InfoSeries.CanTrackerInterpolatePoints <- false
            this.InfoSeries.Color <- OxyColors.SkyBlue
            this.InfoSeries.MarkerType <- MarkerType.Circle
            this.InfoSeries.MarkerSize <- 1.0
            this.InfoSeries.MarkerStroke <-OxyColors.White
            this.InfoSeries.MarkerFill <- OxyColors.SkyBlue
            this.InfoSeries.MarkerStrokeThickness <- 1.0

            this.WarnSeries.Title <- "Warn"
            this.WarnSeries.CanTrackerInterpolatePoints <- false
            this.WarnSeries.Color <- OxyColors.LightGoldenrodYellow
            this.WarnSeries.MarkerType <- MarkerType.Circle
            this.WarnSeries.MarkerSize <- 1.0
            this.WarnSeries.MarkerStroke <-OxyColors.White
            this.WarnSeries.MarkerFill <- OxyColors.LightGoldenrodYellow
            this.WarnSeries.MarkerStrokeThickness <- 1.0

            this.ErrorSeries.Title <- "Error"
            this.ErrorSeries.CanTrackerInterpolatePoints <- false
            this.ErrorSeries.Color <- OxyColors.IndianRed
            this.ErrorSeries.MarkerType <- MarkerType.Circle
            this.ErrorSeries.MarkerSize <- 1.0
            this.ErrorSeries.MarkerStroke <-OxyColors.White
            this.ErrorSeries.MarkerFill <- OxyColors.IndianRed
            this.ErrorSeries.MarkerStrokeThickness <- 1.0

            model.Series.Add(this.TraceSeries)
            model.Series.Add(this.InfoSeries)
            model.Series.Add(this.WarnSeries)
            model.Series.Add(this.ErrorSeries)

            view.Model <- model
            view

        member public this.AddDataToPlot(row:DataRow) =
            match row.TraceLevel with
            | Trace -> this.TraceSeries.Points.Add(new DataPoint(10.0, 10.0))
            | Info -> this.InfoSeries.Points.Add(new DataPoint((float)this.InfoSeries.Points.Count + 1.0, 1.0))
            | Warn -> this.WarnSeries.Points.Add(new DataPoint((float)this.WarnSeries.Points.Count + 1.0, 1.0))
            | Error -> this.ErrorSeries.Points.Add(new DataPoint((float)this.ErrorSeries.Points.Count + 1.0, 1.0))
namespace OpenTraceViewer.Data

open System
open OpenTraceViewer
open System.Collections.Generic

type DataStore() = 

    member this.Subscribers = new List<IObserver<DataStore>>()
    member private this.Store = new ResizeArray<DataRow>()

    member this.ParseLevel (level:string) =
        match level.ToLowerInvariant() with
        | "trace" -> Trace
        | "info" -> Info
        | "warn" -> Warn
        | "error" -> Error
        | _ -> Trace

    member this.AddData (data:string, plot:Plotter) =
        let s = data.Split('|')
        let row = new DataRow(DateTime.Parse(s.[0]), (this.ParseLevel(s.[1])), s.[2])
        ()
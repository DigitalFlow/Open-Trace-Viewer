namespace OpenTraceViewer
open System

type DataRow(date:DateTime, level:TraceLevel, message:string) = 
    member this.TraceLevel = level
    member this.Date = date
    member this.Message = message
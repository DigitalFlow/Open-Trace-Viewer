namespace OpenTraceViewer
open System

type TraceLevel =
    | Trace
    | Info
    | Warn
    | Error

    override tL.ToString() =
        match tL with
        | Trace -> "Trace"
        | Info -> "Info"
        | Warn -> "Warn"
        | Error -> "Error"
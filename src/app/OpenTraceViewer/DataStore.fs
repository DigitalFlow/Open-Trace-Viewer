namespace OpenTraceViewer.Data

open System
open System.IO
open OpenTraceViewer
open System.Collections.Generic
open NLog

type public DataStore() = 
    let store = new List<DataRow>()
    let fileList = new List<FileInfo>()

    member this.Store = store
    member this.Files = fileList
    member private this.logger = LogManager.GetCurrentClassLogger()

    member this.ParseLevel (level:string) =
        let levelLowerCase = level.ToLowerInvariant().Trim()
        match levelLowerCase with
        | "trace" -> Trace
        | "info" -> Info
        | "warn" -> Warn
        | "error" -> Error
        | _ -> Trace

    member this.ParseRow (row : string) =
        let s = row.Split('|')
        new DataRow(DateTime.Parse(s.[0]), (this.ParseLevel(s.[1])), s.[2])

    member this.ParseRows (file:FileInfo) =
        File.ReadAllLines(file.FullName)
        |> Seq.iter (fun item -> this.Store.Add(this.ParseRow(item)))

    member this.ReturnFromStore(page, pageSize) =
        let beginning = page * pageSize

        if beginning > store.Count then
            new List<DataRow>()
        else if pageSize < this.Store.Count - beginning then
            this.Store.GetRange(page * pageSize, pageSize)
        else
            this.Store.GetRange(page * pageSize, this.Store.Count - beginning)

    member public this.GetData(page : int, pageSize : int) =
        if page * pageSize < this.Store.Count then
            this.ReturnFromStore(page, pageSize)
        else
            this.ParseRows(this.Files.[0])
            this.ReturnFromStore(page, pageSize)

    member public this.AddFile (file:FileInfo) =
        this.logger.Trace(sprintf "Adding %s to data store" file.FullName)
        this.Files.Add(file)
        this.logger.Trace(sprintf "Added %s to data store, new file count is %i" file.FullName this.Files.Count)
        this.ParseRows(file)
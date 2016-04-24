

namespace OpenTraceViewer
open System
open Gtk

type ListView() as this = 

    let view = new TreeView()
    let store = new ListStore(typeof<DateTime>, typeof<string>, typeof<string>)

    do this.Initialize

    member private this.View = view
    member private this.Store = store
   
    member this.GetView =
        this.View

    member private this.Initialize =
            let dateCol = new TreeViewColumn()
            let dateCell = new CellRendererText()
            dateCol.Title <- "Date"
            dateCol.PackStart(dateCell, true)
            dateCol.AddAttribute(dateCell, "text", 0)
            dateCol.SetCellDataFunc(dateCell, 
                (fun (col:TreeViewColumn)(cell:CellRenderer)(model:ITreeModel)(iter:TreeIter) -> 
                    (cell:?>CellRendererText).Text <- (model.GetValue(iter, 0).ToString())))

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

            this.View.AppendColumn(dateCol) |> ignore
            this.View.AppendColumn(levelCol) |> ignore
            this.View.AppendColumn(messageCol) |> ignore

            this.View.Model <- this.Store

    member this.AddValue(row : DataRow) =
        this.Store.AppendValues(row.Date, row.TraceLevel.ToString(), row.Message)

    member this.AddValues(rows : seq<DataRow>) =
        rows
        |> Seq.iter (fun row -> this.AddValue(row) |> ignore)
﻿

namespace OpenTraceViewer
open System
open Gtk

type ListView() = 
    member this.CreateList() =
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
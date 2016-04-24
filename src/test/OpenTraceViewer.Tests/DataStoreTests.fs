module DataStoreTests =
    open Xunit
    open NLog
    open System.IO
    open OpenTraceViewer.Data

    [<Fact>]
    let Store_Should_Save_Inserted_Data() =
        let dataStore = new DataStore()
        dataStore.AddFile(new FileInfo("Test"))
<Query Kind="FSharpProgram">
  <NuGetReference>Insight.Database</NuGetReference>
</Query>

open System
open Insight.Database
open System.Data.SqlClient
open System.Data

type Child = {
    ParentId: int;
    ChildNameId: int;
    ChildName: string;
    SecondChildName: string;
}

type Parent = {
    ParentId:int;
    Name: string;
    SecondName: string;
    Childs: List<Child>;
}

let connection = new SqlConnection(MyExtensions.SQLConnectionString)
SqlInsightDbProvider.RegisterProvider()

let Parents = connection.QuerySql<Parent>(@"SELECT 1 ParentID, 'Jaxel' Name, 'Rojas' SecondName UNION SELECT 2, 'Karla', 'Estrada';")

let Parent = connection.SingleSql<Parent>(@"SELECT 1 ParentID, 'Jaxel' Name, 'Rojas' SecondName ")

let Parents2 =
    connection.Query(
        @"SELECT 1 ParentId, 'Jaxel' Name, 'Rojas' SecondName;
          SELECT 1 ParentId, 3 ChildNameId, 'Aidan' ChildName, 'Rojas' SecondChildName UNION
          SELECT 1 ParentId, 4 ChildNameId, 'Sebastian' ChildName, 'Rojas' SecondChildName;",
          Parameters.Empty, Query.Returns<Parent>().ThenChildren(Some<Child>.Records), CommandType.Text)

Parents |> Seq.iter(fun y-> (y.Dump("Parents")))

let r = Parent

r.Dump("First Parent")

let mr = Parents2
mr |> Seq.iter (fun y-> y.Dump("Entire Parent Structure"))

for z in mr do
     z.Childs |> Seq.iter (fun a -> a.Dump("Children"))
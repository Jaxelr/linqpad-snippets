<Query Kind="FSharpProgram">
  <Connection>
    <ID>b8f1c7ca-2805-4049-940e-3663c608c694</ID>
    <Persist>true</Persist>
    <Server>localhost\brief</Server>
    <Database>master</Database>
    <ShowServer>true</ShowServer>
  </Connection>
  <NuGetReference>Insight.Database</NuGetReference>
</Query>

let dc = new TypedDataContext()

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

[<Literal>]
let connectionString = @"Server=localhost\brief;database=master;Trusted_Connection=true;MultipleActiveResultSets=true;"

let connection = new SqlConnection(connectionString)
SqlInsightDbProvider.RegisterProvider()

let Parents = connection.QuerySql<Parent>(@"SELECT 1 ParentID, 'Jaxel' Name, 'Rojas' SecondName UNION SELECT 2, 'Karla', 'Estrada';")

let Parent = connection.SingleSql<Parent>(@"SELECT 1 ParentID, 'Jaxel' Name, 'Rojas' SecondName ")

let Parents2 =
	connection.Query(
		@"SELECT 1 ParentId, 'Jaxel' Name, 'Rojas' SecondName;
          SELECT 1 ParentId, 3 ChildNameId, 'Aidan' ChildName, 'Rojas' SecondChildName UNION
          SELECT 1 ParentId, 4 ChildNameId, 'Sebastian' ChildName, 'Rojas' SecondChildName;",
		  Parameters.Empty, Query.Returns<Parent>().ThenChildren(Some<Child>.Records), CommandType.Text)

Parents |> Seq.iter(fun y-> (printfn "Name: %s  Second Name: %s     Id: %i" y.Name y.SecondName y.ParentId))

let r = Parent

printfn "Name: %s Second Name: %s Id: %i" r.Name r.SecondName r.ParentId

let mr = Parents2
mr |> Seq.iter (fun y-> printfn "%s %s %i" y.Name y.SecondName y.ParentId)

for z in mr do
     z.Childs |> Seq.iter (fun a -> printfn "%i %s %s" a.ChildNameId a.ChildName a.SecondChildName)


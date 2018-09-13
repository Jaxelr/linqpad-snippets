<Query Kind="Statements">
  <NuGetReference>Nancy</NuGetReference>
</Query>

//Create string
string input = $"Hello World, my custom id is: {Guid.NewGuid()}";
input.Dump("My input");

//Cast from string into byte array
var datum = Encoding.ASCII.GetBytes(input);

//Cast from byte array into stream
var stream = new MemoryStream(datum);

//Cast from byte array into string
string output = Encoding.ASCII.GetString(datum);

output.Dump("My output from byte[]");

//Cast from string into MemoryStream
StreamReader reader = new StreamReader(stream);
string output2 = reader.ReadToEnd();

output2.Dump("My output from stream");
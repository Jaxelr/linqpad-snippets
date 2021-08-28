<Query Kind="Statements">
  <IncludeUncapsulator>false</IncludeUncapsulator>
</Query>

//Create string
string input = $"Hello World, my custom id is: {Guid.NewGuid()}";
input.Dump("My input");

//Cast from string into byte array
byte[] datum = Encoding.UTF8.GetBytes(input);

//Cast from byte array into stream
var stream = new MemoryStream(datum);

//Cast from byte array into string
string output = Encoding.UTF8.GetString(datum);

output.Dump("My output from byte[]");

//Cast from string into MemoryStream
StreamReader reader = new StreamReader(stream);
string output2 = reader.ReadToEnd();

output2.Dump("My output from stream");
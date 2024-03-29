<Query Kind="Statements" />

// Copied from: https://github.com/aspnet/Logging/blob/master/src/Microsoft.Extensions.Logging.Console/Internal/ConsoleLoggerProcessor.cs
// On date: 2020-04-30

// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Concurrent;

public class ConsoleLoggerProcessor : IDisposable
{
	private const int _maxQueuedMessages = 1024;

	private readonly BlockingCollection<LogMessageEntry> _messageQueue = new BlockingCollection<LogMessageEntry>(_maxQueuedMessages);
	private readonly Thread _outputThread;

	public IConsole Console;
	public IConsole ErrorConsole;

	public ConsoleLoggerProcessor()
	{
		// Start Console message queue processor
		_outputThread = new Thread(ProcessLogQueue)
		{
			IsBackground = true,
			Name = "Console logger queue processing thread"
		};
		_outputThread.Start();
	}

	public virtual void EnqueueMessage(LogMessageEntry message)
	{
		if (!_messageQueue.IsAddingCompleted)
		{
			try
			{
				_messageQueue.Add(message);
				return;
			}
			catch (InvalidOperationException) { }
		}

		// Adding is completed so just log the message
		WriteMessage(message);
	}

	// for testing
	internal virtual void WriteMessage(LogMessageEntry message)
	{
		var console = message.LogAsError ? ErrorConsole : Console;

		if (message.TimeStamp != null)
		{
			console.Write(message.TimeStamp, message.MessageColor, message.MessageColor);
		}

		if (message.LevelString != null)
		{
			console.Write(message.LevelString, message.LevelBackground, message.LevelForeground);
		}

		console.Write(message.Message, message.MessageColor, message.MessageColor);
		console.Flush();
	}

	private void ProcessLogQueue()
	{
		try
		{
			foreach (var message in _messageQueue.GetConsumingEnumerable())
			{
				WriteMessage(message);
			}
		}
		catch
		{
			try
			{
				_messageQueue.CompleteAdding();
			}
			catch { }
		}
	}

	public void Dispose()
	{
		_messageQueue.CompleteAdding();

		try
		{
			_outputThread.Join(1500); // with timeout in-case Console is locked by user input
		}
		catch (ThreadStateException) { }
	}
}

public struct LogMessageEntry
{
	public string TimeStamp;
	public string LevelString;
	public ConsoleColor? LevelBackground;
	public ConsoleColor? LevelForeground;
	public ConsoleColor? MessageColor;
	public string Message;
	public bool LogAsError;
}

public interface IConsole
{
	void Write(string message, ConsoleColor? background, ConsoleColor? foreground);
	void WriteLine(string message, ConsoleColor? background, ConsoleColor? foreground);
	void Flush();
}

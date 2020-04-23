<Query Kind="Expression">
  <Namespace>System.Security.Cryptography</Namespace>
  <RemoveNamespace>System.Collections</RemoveNamespace>
  <RemoveNamespace>System.Collections.Generic</RemoveNamespace>
  <RemoveNamespace>System.Data</RemoveNamespace>
  <RemoveNamespace>System.Diagnostics</RemoveNamespace>
  <RemoveNamespace>System.IO</RemoveNamespace>
  <RemoveNamespace>System.Linq</RemoveNamespace>
  <RemoveNamespace>System.Linq.Expressions</RemoveNamespace>
  <RemoveNamespace>System.Reflection</RemoveNamespace>
  <RemoveNamespace>System.Text.RegularExpressions</RemoveNamespace>
  <RemoveNamespace>System.Threading</RemoveNamespace>
  <RemoveNamespace>System.Transactions</RemoveNamespace>
  <RemoveNamespace>System.Xml</RemoveNamespace>
  <RemoveNamespace>System.Xml.Linq</RemoveNamespace>
  <RemoveNamespace>System.Xml.XPath</RemoveNamespace>
</Query>

// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.
public static class StringExtensions
{
	/// <summary>
	/// Creates a SHA256 hash of the specified input.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>A hash</returns>
	public static string ToSha256(this string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return string.Empty;
		}

		using var sha = SHA256.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] hash = sha.ComputeHash(bytes);

		return Convert.ToBase64String(hash);
	}

	/// <summary>
	/// Creates a SHA512 hash of the specified input.
	/// </summary>
	/// <param name="input">The input.</param>
	/// <returns>A hash</returns>
	public static string ToSha512(this string input)
	{
		if (string.IsNullOrEmpty(input))
		{
			return string.Empty;
		}

		using var sha = SHA512.Create();
		byte[] bytes = Encoding.UTF8.GetBytes(input);
		byte[] hash = sha.ComputeHash(bytes);

		return Convert.ToBase64String(hash);
	}
# Welcome to the Package Analyzer Project

This repository contains the source code for:

* GraphBuilder - constructs a directed graph from xml files. Identifies cycles in the graph.
* PackageHasher - hashes contents of folders to be used in identifying changes.
* PackageBuilder - uses Graph and Hasher to analyze and packages for build.
* ConsoleTest - a console application that pulls together builder, hasher and graph.

## Project Build Status

Project|Build Status
---|---
PackageAnalyzer|![Build Status](https://github.com/hlotyaks/PackageAnalyzer/workflows/.NET%20Core/badge.svg)

## How to build

The console test can be built and published with the following command:
`dotnet publish --self-contained -r win-x64 -c Release -o .\publish .\ConsoleTest.sln`

## Interesting things learned in this project

You can copy test cases to the output directory by using the following in your test project file

```html
<ItemGroup>
    <None Update="testcases\**\*.*" CopyToOutputDirectory="PreserveNewest" />
</ItemGroup>
```

## Running the GraphBuilder

<!-- MARKDOWN-AUTO-DOCS:START (CODE:src-./ConsoleTest/src/Program.cs) -->

<!-- MARKDOWN-AUTO-DOCS:END -->
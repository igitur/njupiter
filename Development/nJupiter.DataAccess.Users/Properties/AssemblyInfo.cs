using System.Reflection;
using System.Runtime.CompilerServices;

[assembly: AssemblyDescription("General User Access Component")]

// Increment AssemblyVersion only on major and minor releases
[assembly: AssemblyVersion("4.0.1.308")]
// Increment AssemblyFileVersion on every release
[assembly: AssemblyFileVersion("4.2.0.516")]

#if DEBUG
[assembly: InternalsVisibleTo("nJupiter.DataAccess.Users.Tests.Unit")]
#endif

//This product includes software developed by Sebastien Ros and contributors (https://github.com/sebastienros/esprima-dotnet).

//Esprima.NET is licensed under the BSD 2-Clause License:

//Copyright(c) 2014, Sebastien Ros
//All rights reserved.

//Redistribution and use in source and binary forms, with or without
//modification, are permitted provided that the following conditions are met:

//1.Redistributions of source code must retain the above copyright notice,
//   this list of conditions and the following disclaimer.

//2. Redistributions in binary form must reproduce the above copyright
//   notice, this list of conditions and the following disclaimer in the
//   documentation and/or other materials provided with the distribution.

//THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
//AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
//IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
//ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
//LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
//CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
//SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
//INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
//CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
//ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
//POSSIBILITY OF SUCH DAMAGE.


using Esprima;
using Esprima.Ast;
using System.Collections.Generic;

namespace cloudscribe.SimpleContent.Web.Services
{
    public class JsSecuritySanitizer 
    {
        private static readonly HashSet<string> DangerousCalls = new()
        {
            "eval",                // Executes arbitrary strings as code
            "Function",            // Creates functions from strings (like eval)
            "setTimeout",          // Often used with string eval
            "setInterval",         // Same
            "execScript",          // IE-specific eval alternative
            // "alert",               // Common in test payloads (may remove if just for dev)
            "confirm",             // Can be used to trick users
            "prompt",              // Same
            "fetch",               // Sends data out
            "XMLHttpRequest",      // Same
            "open",                // Opens new windows/tabs
            "showModalDialog",     // Obsolete, but sometimes used for phishing
            "postMessage",         // Can be abused for cross-origin data leaks
            "webkitRequestFullscreen", // Abused for phishing
            "requestFullscreen",   // Tricking users
            "requestPointerLock",  // Locking pointer as part of scams
        };

        private static readonly HashSet<string> DangerousProperties = new()
        {
            "document.cookie",        // Accesses user's cookies
            "window.location",        // Can redirect user or steal location
            "location.href",          // Commonly set to redirect
            "document.location",      // Same
            "window.name",            // Used to pass data between domains
            "localStorage",           // Persistent local storage
            "sessionStorage",         // Session-scoped storage
            "indexedDB",              // DB access
            "navigator.geolocation",  // Gets user location
            "navigator.clipboard",    // Read/write clipboard
            "navigator.sendBeacon",   // Sends data after page unload
            "navigator.mediaDevices", // Webcam/mic access
            "window.parent",          // Cross-frame manipulation
            "window.top",             // Can break out of iframes
            "window.opener",          // Can hijack the opener page
            "window.history",         // Modify browser history
        };

        public bool IsSafe(string script, out List<string> issues)
        {
            issues = new List<string>();

            try
            {
                var parser = new JavaScriptParser();
                var program = parser.ParseScript(script);

                TraverseNode(program, issues);
                return issues.Count == 0;
            }
            catch (ParserException ex)
            {
                issues.Add($"Parsing error: {ex.Message}");
                return false;
            }
        }

        private void TraverseNode(Node node, List<string> issues)
        {
            if (node == null) return;

            // Check for dangerous function calls
            if (node is CallExpression callExpr)
            {
                if (callExpr.Callee is Identifier ident &&
                    DangerousCalls.Contains(ident.Name))
                {
                    issues.Add($"Call to disallowed function: {ident.Name}");
                }
            }

            // Check for dangerous property access like window.location or document.cookie
            if (node is MemberExpression memberExpr &&
                memberExpr.Object is Identifier obj &&
                memberExpr.Property is Identifier prop)
            {
                string fullAccess = $"{obj.Name}.{prop.Name}";
                if (DangerousProperties.Contains(fullAccess))
                {
                    issues.Add($"Access to disallowed property: {fullAccess}");
                }
            }

            // Recursively walk all child nodes
            foreach (var child in node.ChildNodes)
            {
                TraverseNode(child, issues);
            }
        }
    }
}

namespace WebSharper.JQueryTerminal.Extension

open WebSharper
open WebSharper.InterfaceGenerator

module Definition =
    let Terminal = Class "Terminal"

    let callbackSA = T<string[]> ^-> T<unit>
    let callbackS = T<string> ^-> T<unit>
    let interpreterFunction = Terminal -* T<string> ^-> T<unit>
    let greetingsCallback = callbackS ^-> T<unit>

    let Options =
        Pattern.Config "Options"{
            Required=[]
            Optional=
            [
                "checkArity", T<bool>
                "clear", T<bool>
                "clickTimeout", T<int>
                "completion", (T<string> * callbackSA + T<string[]> + T<bool>) ^-> T<unit>
                "convertLinks", T<bool>
                "describe", T<string> + T<bool>
                "echoCommand", T<bool>
                "enabled", T<bool>
                "exceptionHandler", callbackS
                "exechash", T<bool>
                "exit", T<bool>
                "extra", T<obj>
                "greetings", T<string> // + greetingsCallback
                "history", T<bool>
                "historyFilter", T<string> ^-> T<bool>
                "historySize", T<int>
                "historyState", T<bool>
                "importHistory", T<bool>
                //"keydown", T<>
                "keymap", T<obj>
                //"keypress", T<>
                "linksNoReferer", T<bool>
                "linksNoReferrer", T<bool>
                //"login", T<> nope
                "maskChar", T<bool> + T<string>
                "memory", T<bool>
                "name", T<string>
                "numChars", T<int>
                "numRows", T<int>
                "onAfterCommand", T<string> ^-> T<unit>
                //"onAfterLogout", T<>
                //"onAjaxError"
                "onBeforeCommand", T<string> ^-> T<unit>
                //"onBeforeLogin"
                //"onBeforeLogout"
                //!!"onBlur" -- !! -- !!
                //"onClear"
                //"onCommandChange"
                //"onCommandNotFound"
                //"onExit"
                //"onExport"
                //"onImport"
                //"onFocus"
                //"onInt"
                "onPause", T<unit> ^-> T<unit>
                "onPop", T<unit> ^-> T<unit>
                "onPush", T<unit> ^-> T<unit>
                "onRPCError", T<string> ^-> T<unit>
                "onResize", T<unit> ^-> T<unit>
                "onResume", T<unit> ^-> T<unit>
                //"onTerminalChange"
                "outputLimit", T<int>
                "pauseEvents", T<bool>
                "processArguments", T<bool> + (T<string> ^-> T<string[]>)
                "processRCPResponse", T<obj> ^-> T<unit>
                "prompt", T<string> //, T<>
                //"request", T<>
                //"response", T<>
                "scrollBottomOffset", T<int>
                "scrollOnEcho", T<bool>
                "softPause", T<bool>
                "wordAutocomplete", T<bool>
                "wrap", T<bool>
            ]
        }
    
    let EchoOptions =
        Pattern.Config "EchoOptions"{
            Required = []
            Optional =
            [
                "raw", T<bool>
                "flush", T<bool>
                "keepWords", T<bool>
            ]
        }

    Terminal
        |+> Static[
            Constructor(T<string>?target * interpreterFunction?interpreter * Options?options)
            |>WithInline("$($target).terminal($interpreter, $options);")
        ]
        |+> Instance[
            "clear" => T<unit> ^-> T<unit>
            "destroy" => T<unit> ^-> T<unit>
            "echo" => (T<string> + (T<string> ^-> T<string>)) * !? EchoOptions ^-> T<unit>
            "enable" => T<unit> ^-> T<unit>
            "disable" => T<unit> ^-> T<unit>
            "flush" => T<unit> ^-> T<unit>
            "settings" => T<unit> ^-> Options
        ]|>ignore

    let Assembly =
        Assembly [
            Namespace "WebSharper.JQueryTerminal.Resources" [
                Resource "Js" "https://cdnjs.cloudflare.com/ajax/libs/jquery.terminal/1.5.3/js/jquery.terminal.min.js"
                |> RequiresExternal [T<WebSharper.JQuery.Resources.JQuery>]
                |> AssemblyWide
                Resource "Css" "https://cdnjs.cloudflare.com/ajax/libs/jquery.terminal/1.5.3/css/jquery.terminal.min.css"
                |> AssemblyWide
            ]
            Namespace "WebSharper.JQueryTerminal" [
                Terminal
                Options
                EchoOptions
            ]
        ]


[<Sealed>]
type Extension() =
    interface IExtension with
        member x.Assembly = Definition.Assembly

[<assembly: Extension(typeof<Extension>)>]
do ()
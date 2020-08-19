// $begin{copyright}
//
// This file is part of WebSharper
//
// Copyright (c) 2008-2018 IntelliFactory
//
// Licensed under the Apache License, Version 2.0 (the "License"); you
// may not use this file except in compliance with the License.  You may
// obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or
// implied.  See the License for the specific language governing
// permissions and limitations under the License.
//
// $end{copyright}
namespace WebSharperTerminalSample

open WebSharper
open WebSharper.JavaScript
open WebSharper.JQuery
open WebSharper.UI
open WebSharper.UI.Html
open WebSharper.UI.Client
open WebSharper.UI.Templating
open WebSharper.JQuery
open SMApp.JQueryTerminal

[<JavaScript>]
module Client =
    
    type FormTemplate = Templating.Template<"./template.html">

    let intOpt =
        InterpreterOptions(
            Name = "No2",
            Prompt = "+ "
        )
    let i2 = (fun (this: Terminal) command ->
            match command with
            | "asd" -> this.Echo "kek"
            | "switch" -> this.Pop()
            | _ -> this.Echo "..."
        )
    

    let (|Help|_|) (command: string) =
        if command = "help" then
            Some ()
        else
            None

    let (|Clear|_|) (command: string) =
        if command = "clear" then
            Some()
        else
            None

    let (|Template|_|) (command: string) =
        if command = "template" then
            Some()
        else
            None
    let (|Switch|_|) (command: string) =
        if command = "switch" then
            Some()
        else
            None
    
    let (|Blank|_|) (command: string) =
        if command = "" then
            Some()
        else
            None
    let mutable numOfTemplates = 0
    let interpreter =
        FuncWithThis<Terminal, string->Unit>(fun this command ->
            match command with
            | Help -> this.Echo "Commands: help, clear, template"
            | Template ->
                numOfTemplates <- numOfTemplates + 1
                let id = "template" + string numOfTemplates
                let rvProjName = Var.Create ""
                let html = 
 (*                   Doc.Concat[
                        Doc.Input [on.keyPress (fun _ ev -> ev.StopPropagation()); on.keyDown (fun _ ev -> ev.StopPropagation()); on.keyUp (fun _ ev -> ev.StopPropagation()); on.input (fun _ ev -> ev.StopPropagation())] rvInputList.[numOfTemplates]
                    ]*)
                    FormTemplate.Form()
                        .ProjName(rvProjName)
                        .EventProp([on.keyPress (fun _ ev -> ev.StopPropagation()); on.keyDown (fun _ ev -> ev.StopPropagation()); on.keyUp (fun _ ev -> ev.StopPropagation()); on.input (fun _ ev -> ev.StopPropagation())])
                        .Download(fun _ -> JS.Alert(rvProjName.Value))
                        .Doc()
                this.EchoHtml ("<div id=\"" + id + "\"></div>")
                html
                |> Doc.RunById id
  (*              rvInput.View
                |> View.Map (fun _ -> "it works")
                |> Doc.TextView
                |> Doc.RunById "my"*)
            | Clear -> this.Clear()
            | Switch -> this.Push (i2, intOpt)
            | Blank -> this.Echo ""
            | _ -> this.EchoHtml("Unknown command")
        )
            
    let Opt =
        Options(
            Name = "Terminal1",
            Prompt = "> ",
            Greetings = "Welcome to the Terminal Test Page! See 'help' for the list of commands.",
            OnInit = (fun (t:Terminal) -> t.Enable(); t.Echo("Hey Dood, it's workin'!"))
        )
    
    [<SPAEntryPoint>]
    let Main =
        Terminal("#body", interpreter, Opt)

//----------------------------------------------------------------------------
//
// Copyright (c) 2002-2012 Microsoft Corporation. 
//
// This source code is subject to terms and conditions of the Apache License, Version 2.0. A 
// copy of the license can be found in the License.html file at the root of this distribution. 
// By using this source code in any fashion, you are agreeing to be bound 
// by the terms of the Apache License, Version 2.0.
//
// You must not remove this notice, or any other, from this software.
//----------------------------------------------------------------------------

module (*internal*) Microsoft.FSharp.Compiler.ErrorLogger

open Microsoft.FSharp.Text.Lexing
open System

//------------------------------------------------------------------------
// General error recovery mechanism
//-----------------------------------------------------------------------

/// Thrown when want to add some Position information to some .NET exception
exception WrappedError of exn * Position

/// Thrown when immediate, local error recovery is not possible. This indicates
/// we've reported an error but need to make a non-local transfer of control.
/// Error recovery may catch this and continue (see 'errorRecovery')
///
/// The exception that caused the report is carried as data because in some
/// situations (LazyWithContext) we may need to re-report the original error
/// when a lazy thunk is re-evaluated.
exception ReportedError of exn option with
    override this.Message =
        match this :> exn with
        | ReportedError (Some exn) -> exn.Message
        | _ -> "ReportedError"

let rec findOriginalException err = 
    match err with 
    | ReportedError (Some err) -> err
    | WrappedError(err,_)  -> findOriginalException err
    | _ -> err


/// Thrown when we stop processing the F# Interactive interactive entry or #load.
exception StopProcessing


(* common error kinds *)
exception NumberedError of (int * string) * Position with   // int is e.g. 191 in FS0191
    override this.Message =
        match this :> exn with
        | NumberedError((_,msg),_) -> msg
        | _ -> "impossible"
exception Error of (int * string) * Position with   // int is e.g. 191 in FS0191  // eventually remove this type, it is a transitional artifact of the old unnumbered error style
    override this.Message =
        match this :> exn with
        | Error((_,msg),_) -> msg
        | _ -> "impossible"
exception InternalError of string * Position
exception UserCompilerMessage of string * int * Position
exception LibraryUseOnly of Position
exception Deprecated of string * Position
exception Experimental of string * Position
exception PossibleUnverifiableCode of Position

// Range\NoRange Duals
exception UnresolvedReferenceNoRange of (*assemblyname*) string 
exception UnresolvedReferenceError of (*assemblyname*) string * Position
exception UnresolvedPathReferenceNoRange of (*assemblyname*) string * (*path*) string
exception UnresolvedPathReference of (*assemblyname*) string * (*path*) string * Position


let inline protectAssemblyExploration dflt f = 
    try 
       f()
     with 
        | UnresolvedPathReferenceNoRange _ -> dflt
        | _ -> reraise()

let inline protectAssemblyExplorationNoReraise dflt1 dflt2 f  = 
    try 
       f()
     with 
        | UnresolvedPathReferenceNoRange _ -> dflt1
        | _ -> dflt2

// Attach a Position if this is a Position dual exception.
let rec AttachRange m (exn:exn) = 
    if m = Position.Empty then exn
    else 
        match exn with
        // Strip TargetInvocationException wrappers
        | :? System.Reflection.TargetInvocationException -> AttachRange m exn.InnerException
        | UnresolvedReferenceNoRange(a) -> UnresolvedReferenceError(a,m)
        | UnresolvedPathReferenceNoRange(a,p) -> UnresolvedPathReference(a,p,m)
        | Failure(msg) -> InternalError(msg^" (Failure)",m)
        | :? System.ArgumentException as exn -> InternalError(exn.Message + " (ArgumentException)",m)
        | notARangeDual -> notARangeDual

//----------------------------------------------------------------------------
// Error logger interface

type Exiter = 
    abstract Exit : int -> 'T 

let QuitProcessExiter = 
    { new Exiter with 
        member x.Exit(n) = 
#if SILVERLIGHT
#else                    
            try 
              System.Environment.Exit(n)
            with _ -> 
              ()
#endif              
            failwithf "%s" <| FSComp.SR.elSysEnvExitDidntExit() }

/// Closed enumeration of build phases.
type BuildPhase =
    | DefaultPhase 
    | Compile 
    |  Parameter | Parse | TypeCheck 
    | CodeGen 
    |  Optimize |  IlxGen |  IlGen |  Output 
    | Interactive // An error seen during interactive execution
    
/// Literal build phase subcategory strings.
module BuildPhaseSubcategory =
    [<Literal>] 
    let DefaultPhase = ""
    [<Literal>] 
    let Compile = "compile"
    [<Literal>] 
    let Parameter = "parameter"
    [<Literal>] 
    let Parse = "parse"
    [<Literal>] 
    let TypeCheck = "typecheck"
    [<Literal>] 
    let CodeGen = "codegen"
    [<Literal>] 
    let Optimize = "optimize"
    [<Literal>] 
    let IlxGen = "ilxgen"
    [<Literal>] 
    let IlGen = "ilgen"        
    [<Literal>] 
    let Output = "output"        
    [<Literal>] 
    let Interactive = "interactive"        
    [<Literal>] 
    let Internal = "internal"          // Compiler ICE

[<System.Diagnostics.DebuggerDisplay("{DebugDisplay()}")>]
type PhasedError = { Exception:exn; Phase:BuildPhase } with
    /// Construct a phased error
    static member Create(exn:exn,phase:BuildPhase) : PhasedError =
        System.Diagnostics.Debug.Assert(phase<>BuildPhase.DefaultPhase, sprintf "Compile error seen with no phase to attribute it to.%A %s %s" phase exn.Message exn.StackTrace )        
        {Exception = exn; Phase=phase}
    member this.DebugDisplay() =
        sprintf "%s: %s" (this.Subcategory()) this.Exception.Message
    /// This is the textual subcategory to display in error and warning messages (shows only under --vserrors):
    ///
    ///     file1.fs(72): subcategory warning FS0072: This is a warning message
    ///
    member pe.Subcategory() =
        match pe.Phase with
        | DefaultPhase -> BuildPhaseSubcategory.DefaultPhase
        | Compile -> BuildPhaseSubcategory.Compile
        | Parameter -> BuildPhaseSubcategory.Parameter
        | Parse -> BuildPhaseSubcategory.Parse
        | TypeCheck -> BuildPhaseSubcategory.TypeCheck
        | CodeGen -> BuildPhaseSubcategory.CodeGen
        | Optimize -> BuildPhaseSubcategory.Optimize
        | IlxGen -> BuildPhaseSubcategory.IlxGen
        | IlGen -> BuildPhaseSubcategory.IlGen
        | Output -> BuildPhaseSubcategory.Output
        | Interactive -> BuildPhaseSubcategory.Interactive
    /// Return true if the textual phase given is from the compile part of the build process.
    /// This set needs to be equal to the set of subcategories that the language service can produce. 
    static member IsSubcategoryOfCompile(subcategory:string) =
        // Beware: This code logic is duplicated in DocumentTask.cs in the language service
        match subcategory with 
        | BuildPhaseSubcategory.Compile 
        | BuildPhaseSubcategory.Parameter 
        | BuildPhaseSubcategory.Parse 
        | BuildPhaseSubcategory.TypeCheck -> true
        | null 
        | BuildPhaseSubcategory.DefaultPhase 
        | BuildPhaseSubcategory.CodeGen 
        | BuildPhaseSubcategory.Optimize 
        | BuildPhaseSubcategory.IlxGen 
        | BuildPhaseSubcategory.IlGen 
        | BuildPhaseSubcategory.Output 
        | BuildPhaseSubcategory.Interactive -> false
        | BuildPhaseSubcategory.Internal 
            // Getting here means the compiler has ICE-d. Let's not pile on by showing the unknownSubcategory assert below.
            // Just treat as an unknown-to-LanguageService error.
            -> false
        | unknownSubcategory -> 
            System.Diagnostics.Debug.Assert(false, sprintf "Subcategory '%s' could not be correlated with a build phase." unknownSubcategory)
            // Recovery is to treat this as a 'build' error. Downstream, the project system and language service will treat this as
            // if it came from the build and not the language service.
            false
    /// Return true if this phase is one that's known to be part of the 'compile'. This is the initial phase of the entire compilation that
    /// the language service knows about.                
    member pe.IsPhaseInCompile() = 
        let isPhaseInCompile = 
            match pe.Phase with
            | Compile | Parameter | Parse | TypeCheck -> true
            | _ -> false
        // Sanity check ensures that Phase matches Subcategory            
#if DEBUG
        if isPhaseInCompile then 
            System.Diagnostics.Debug.Assert(PhasedError.IsSubcategoryOfCompile(pe.Subcategory()), "Subcategory did not match isPhaesInCompile=true")
        else
            System.Diagnostics.Debug.Assert(not(PhasedError.IsSubcategoryOfCompile(pe.Subcategory())), "Subcategory did not match isPhaseInCompile=false")
#endif            
        isPhaseInCompile

[<AbstractClass>]
[<System.Diagnostics.DebuggerDisplay("{DebugDisplay()}")>]
type ErrorLogger(nameForDebugging:string) = 
    abstract ErrorCount: int
    // the purpose of the 'Impl' factoring is so that you can put a breakpoint on the non-Impl code just below, and get a breakpoint for all implementations of error loggers
    abstract WarnSinkImpl: PhasedError -> unit
    abstract ErrorSinkRichImpl: (PhasedError * string) -> unit
    abstract ErrorSinkImpl: PhasedError -> unit
    member this.WarnSink err = 
        this.WarnSinkImpl err
    member this.ErrorSink err =
        this.ErrorSinkImpl err
    member this.ErrorSinkRich err =
        this.ErrorSinkRichImpl err
    member this.DebugDisplay() = sprintf "ErrorLogger(%s)" nameForDebugging

let DiscardErrorsLogger = 
    { new ErrorLogger("DiscardErrorsLogger") with 
            member x.ErrorSinkRichImpl(e) =
                ()
            member x.WarnSinkImpl(e) = 
                ()
            member x.ErrorSinkImpl(e) = 
                ()
            member x.ErrorCount = 
                0 }

let AssertFalseErrorLogger =
    { new ErrorLogger("AssertFalseErrorLogger") with 
            member x.ErrorSinkRichImpl(e) =
                assert false; ()
            member x.WarnSinkImpl(e) = 
                assert false; ()
            member x.ErrorSinkImpl(e) = 
                assert false; ()
            member x.ErrorCount = 
                assert false; 0 }

/// When no errorLogger is installed (on the thread) use this one.
let uninitializedErrorLoggerFallback = ref AssertFalseErrorLogger

/// Type holds thread-static globals for use by the compile
type (*internal*) CompileThreadStatic =
    [<System.ThreadStatic;DefaultValue>]
    static val mutable private buildPhase  : BuildPhase
    
    [<System.ThreadStatic;DefaultValue>]
    static val mutable private errorLogger : ErrorLogger

    static member BuildPhaseUnchecked with get() = CompileThreadStatic.buildPhase (* This can be a null value *)
    static member BuildPhase
        with get() = BuildPhase.Parse // if box CompileThreadStatic.buildPhase <> null then CompileThreadStatic.buildPhase else (assert false; BuildPhase.DefaultPhase)
        and set v = CompileThreadStatic.buildPhase <- v
            
    static member ErrorLogger
        with get() = if box CompileThreadStatic.errorLogger <> null then CompileThreadStatic.errorLogger else !uninitializedErrorLoggerFallback
        and set v = CompileThreadStatic.errorLogger <- v


[<AutoOpen>]
module ErrorLoggerExtensions = 
    open System.Reflection

#if SILVERLIGHT
#else
    // Instruct the exception not to reset itself when thrown again.
    // Why don?t we just not catch these in the first place? Because we made the design choice to ask the user to send mail to fsbugs@microsoft.com. 
    // To achieve this, we need to catch the exception, report the email address and stack trace, and then reraise. 
    let PreserveStackTrace(exn) =
        try 
            let preserveStackTrace = typeof<System.Exception>.GetMethod("InternalPreserveStackTrace", BindingFlags.Instance ||| BindingFlags.NonPublic)
            preserveStackTrace.Invoke(exn, null) |> ignore
        with e->
           // This is probably only the mono case.
           System.Diagnostics.Debug.Assert(false, "Could not preserve stack trace for watson exception.")
           ()


    // Reraise an exception if it is one we want to report to Watson.
    let ReraiseIfWatsonable(exn:exn) =
        match  exn with 
        // These few SystemExceptions which we don't report to Watson are because we handle these in some way in Build.fs
        | :? System.Reflection.TargetInvocationException -> ()
        | :? System.NotSupportedException  -> ()
        | :? System.IO.IOException -> () // This covers FileNotFoundException and DirectoryNotFoundException
        | :? System.UnauthorizedAccessException -> ()
        | Failure _ // This gives reports for compiler INTERNAL ERRORs
        | :? System.SystemException -> 
            PreserveStackTrace(exn)
            raise exn
        | _ -> ()
#endif

    type ErrorLogger with  
        member x.ErrorR  exn = match exn with StopProcessing | ReportedError _ -> raise exn | _ -> x.ErrorSink(PhasedError.Create(exn,CompileThreadStatic.BuildPhase))
        member x.ErrorRrich  (exn, error_string) = 
          
          match exn with StopProcessing | ReportedError _ -> raise exn | _ -> x.ErrorSinkRich(PhasedError.Create(exn,CompileThreadStatic.BuildPhase), error_string)
        member x.Warning exn = match exn with StopProcessing | ReportedError _ -> raise exn | _ -> x.WarnSink(PhasedError.Create(exn,CompileThreadStatic.BuildPhase))
        member x.Error   exn = x.ErrorR exn; raise (ReportedError (Some exn))
        member x.PhasedError   (ph:PhasedError) = 
            x.ErrorSink ph
            raise (ReportedError (Some ph.Exception))
        member x.ErrorRecovery (exn:exn) (m:Position) =
            // Never throws ReportedError.
            // Throws StopProcessing and exceptions raised by the ErrorSink(exn) handler.
            match exn with
            (* Don't send ThreadAbortException down the error channel *)
            | :? System.Threading.ThreadAbortException | WrappedError((:? System.Threading.ThreadAbortException),_) ->  ()
            | ReportedError _  | WrappedError(ReportedError _,_)  -> ()
            | StopProcessing | WrappedError(StopProcessing,_) -> raise exn
            | _ ->
                try  
                    x.ErrorR (AttachRange m exn) // may raise exceptions, e.g. an fsi error sink raises StopProcessing.
#if SILVERLIGHT
#else
                    ReraiseIfWatsonable(exn)
#endif
                with
                  | ReportedError _ | WrappedError(ReportedError _,_)  -> ()
        member x.StopProcessingRecovery (exn:exn) (m:Position) =
            // Do standard error recovery.
            // Additionally ignore/catch StopProcessing. [This is the only catch handler for StopProcessing].
            // Additionally ignore/catch ReportedError.
            // Can throw other exceptions raised by the ErrorSink(exn) handler.         
            match exn with
            | StopProcessing | WrappedError(StopProcessing,_) -> () // suppress, so skip error recovery.
            | _ ->
                try  x.ErrorRecovery exn m
                with
                  | StopProcessing | WrappedError(StopProcessing,_) -> () // catch, e.g. raised by ErrorSink.
                  | ReportedError _ | WrappedError(ReportedError _,_)  -> () // catch, but not expected unless ErrorRecovery is changed.
        member x.ErrorRecoveryNoRange (exn:exn) =
            x.ErrorRecovery exn Position.Empty

/// NOTE: The change will be undone when the returned "unwind" object disposes
let PushThreadBuildPhaseUntilUnwind (phase:BuildPhase) =
    let oldBuildPhase = CompileThreadStatic.BuildPhaseUnchecked
    CompileThreadStatic.BuildPhase <- phase
    { new System.IDisposable with 
         member x.Dispose() = CompileThreadStatic.BuildPhase <- oldBuildPhase (* maybe null *) }

/// NOTE: The change will be undone when the returned "unwind" object disposes
let PushErrorLoggerPhaseUntilUnwind(errorLoggerTransformer : ErrorLogger -> #ErrorLogger) =
    let oldErrorLogger = CompileThreadStatic.ErrorLogger
    let newErrorLogger = errorLoggerTransformer oldErrorLogger
    let newInstalled = ref true
    let newIsInstalled() = if !newInstalled then () else (assert false; (); (*failwith "error logger used after unwind"*)) // REVIEW: ok to throw?
    let chkErrorLogger = { new ErrorLogger("PushErrorLoggerPhaseUntilUnwind") with
                             member x.WarnSinkImpl(e)  = newIsInstalled(); newErrorLogger.WarnSink(e)
                             member x.ErrorSinkImpl(e) = newIsInstalled(); newErrorLogger.ErrorSink(e)
                             member x.ErrorSinkRichImpl(e) = newIsInstalled(); newErrorLogger.ErrorSinkRich(e)
                             member x.ErrorCount   = newIsInstalled(); newErrorLogger.ErrorCount }
    CompileThreadStatic.ErrorLogger <- chkErrorLogger
    { new System.IDisposable with 
         member x.Dispose() =       
            CompileThreadStatic.ErrorLogger <- oldErrorLogger
            newInstalled := false }

let SetThreadBuildPhaseNoUnwind(phase:BuildPhase) = CompileThreadStatic.BuildPhase <- phase
let SetThreadErrorLoggerNoUnwind(errorLogger)     = CompileThreadStatic.ErrorLogger <- errorLogger
let SetUninitializedErrorLoggerFallback errLogger = uninitializedErrorLoggerFallback := errLogger

// Global functions are still used by parser and TAST ops
let errorR  exn = CompileThreadStatic.ErrorLogger.ErrorR exn
let errorRrich  exn = CompileThreadStatic.ErrorLogger.ErrorRrich exn

let warning exn = CompileThreadStatic.ErrorLogger.Warning exn
let error   exn = CompileThreadStatic.ErrorLogger.Error exn
// for test only
let phasedError (p : PhasedError) = CompileThreadStatic.ErrorLogger.PhasedError p

let errorSink pe = CompileThreadStatic.ErrorLogger.ErrorSink pe
let warnSink pe = CompileThreadStatic.ErrorLogger.WarnSink pe
let errorRecovery exn m = CompileThreadStatic.ErrorLogger.ErrorRecovery exn m
let stopProcessingRecovery exn m = CompileThreadStatic.ErrorLogger.StopProcessingRecovery exn m
let errorRecoveryNoRange exn = CompileThreadStatic.ErrorLogger.ErrorRecoveryNoRange exn


let report f = 
    f() 

let deprecatedWithError s m = errorR(Deprecated(s,m))

// Note: global state, but only for compiling FSHarp.Core.dll
let mutable reportLibraryOnlyFeatures = true
let libraryOnlyError m = if reportLibraryOnlyFeatures then errorR(LibraryUseOnly(m))
let libraryOnlyWarning m = if reportLibraryOnlyFeatures then warning(LibraryUseOnly(m))
let deprecatedOperator m = deprecatedWithError (FSComp.SR.elDeprecatedOperator()) m
let mlCompatWarning s m = warning(UserCompilerMessage(FSComp.SR.mlCompatMessage s, 62, m))

let suppressErrorReporting f =
    let errorLogger = CompileThreadStatic.ErrorLogger
    try
        let errorLogger = 
            { new ErrorLogger("suppressErrorReporting") with 
                member x.WarnSinkImpl(_exn) = ()
                member x.ErrorSinkImpl(_exn) = ()
                member x.ErrorSinkRichImpl(_exn) = ()
                member x.ErrorCount = 0 }
        SetThreadErrorLoggerNoUnwind(errorLogger)
        f()
    finally
        SetThreadErrorLoggerNoUnwind(errorLogger)

let conditionallySuppressErrorReporting cond f = if cond then suppressErrorReporting f else f()

//------------------------------------------------------------------------
// Errors as data: Sometimes we have to reify errors as data, e.g. if backtracking 
//
// REVIEW: consider using F# computation expressions here

[<NoEquality; NoComparison>]
type OperationResult<'T> = 
    | OkResult of (* warnings: *) exn list * 'T
    | ErrorResult of (* warnings: *) exn list * exn
    
type ImperativeOperationResult = OperationResult<unit>

let ReportWarnings warns = 
    match warns with 
    | [] -> () // shortcut in common case
    | _ -> List.iter warning warns

let CommitOperationResult res = 
    match res with 
    | OkResult (warns,res) -> ReportWarnings warns; res
    | ErrorResult (warns,err) -> ReportWarnings warns; error err

let RaiseOperationResult res : unit = CommitOperationResult res

let ErrorD err = ErrorResult([],err)
let WarnD err = OkResult([err],())
let CompleteD = OkResult([],())
let ResultD x = OkResult([],x)
let CheckNoErrorsAndGetWarnings res  = match res with OkResult (warns,_) -> Some warns | ErrorResult _ -> None 

/// The bind in the monad. Stop on first error. Accumulate warnings and continue. 
let (++) res f = 
    match res with 
    | OkResult([],res) -> (* tailcall *) f res 
    | OkResult(warns,res) -> 
        begin match f res with 
        | OkResult(warns2,res2) -> OkResult(warns@warns2, res2)
        | ErrorResult(warns2,err) -> ErrorResult(warns@warns2, err)
        end
    | ErrorResult(warns,err) -> 
        ErrorResult(warns,err)
        
/// Stop on first error. Accumulate warnings and continue. 
let rec IterateD f xs = match xs with [] -> CompleteD | h :: t -> f h ++ (fun () -> IterateD f t)
let rec WhileD gd body = if gd() then body() ++ (fun () -> WhileD gd body) else CompleteD
let MapD f xs = let rec loop acc xs = match xs with [] -> ResultD (List.rev acc) | h :: t -> f h ++ (fun x -> loop (x::acc) t) in loop [] xs

type TrackErrorsBuilder() =
    member x.Bind(res,k) = res ++ k
    member x.Return(res) = ResultD(res)
    member x.ReturnFrom(res) = res
    member x.For(seq,k) = IterateD k seq
    member x.While(gd,k) = WhileD gd k
    member x.Zero()  = CompleteD

let trackErrors = TrackErrorsBuilder()
    
/// Stop on first error. Accumulate warnings and continue. 
let OptionD f xs = match xs with None -> CompleteD | Some(h) -> f h 

/// Stop on first error. Report index 
let IterateIdxD f xs = 
    let rec loop xs i = match xs with [] -> CompleteD | h :: t -> f i h ++ (fun () -> loop t (i+1))
    loop xs 0

/// Stop on first error. Accumulate warnings and continue. 
let rec Iterate2D f xs ys = 
    match xs,ys with 
    | [],[] -> CompleteD 
    | h1 :: t1, h2::t2 -> f h1 h2 ++ (fun () -> Iterate2D f t1 t2) 
    | _ -> failwith "Iterate2D"

let TryD f g = 
    match f() with
    | ErrorResult(warns,err) ->  (OkResult(warns,())) ++ (fun () -> g err)
    | res -> res

let rec RepeatWhileD ndeep body = body ndeep ++ (function true -> RepeatWhileD (ndeep+1) body | false -> CompleteD) 
let AtLeastOneD f l = MapD f l ++ (fun res -> ResultD (List.exists id res))


// Code below is for --flaterrors flag that is only used by the IDE

let stringThatIsAProxyForANewlineInFlatErrors = new System.String[|char 29 |]

let NewlineifyErrorString (message:string) = message.Replace(stringThatIsAProxyForANewlineInFlatErrors, Environment.NewLine)

/// fixes given string by replacing all control chars with spaces.
/// NOTE: newlines are recognized and replaced with stringThatIsAProxyForANewlineInFlatErrors (ASCII 29, the 'group separator'), 
/// which is decoded by the IDE with 'NewlineifyErrorString' back into newlines, so that multi-line errors can be displayed in QuickInfo
let NormalizeErrorString (text : string) =    
    if text = null then nullArg "text"
    let text = text.Trim()

    let buf = System.Text.StringBuilder()
    let mutable i = 0
    while i < text.Length do
        let delta = 
            match text.[i] with
            | '\r' when i + 1 < text.Length && text.[i + 1] = '\n' ->
                // handle \r\n sequence - replace it with one single space
                buf.Append(stringThatIsAProxyForANewlineInFlatErrors) |> ignore
                2
            | '\n' ->
                buf.Append(stringThatIsAProxyForANewlineInFlatErrors) |> ignore
                1
            | c ->
                // handle remaining chars: control - replace with space, others - keep unchanged
                let c = if Char.IsControl(c) then ' ' else c
                buf.Append(c) |> ignore
                1
        i <- i + delta
    buf.ToString()

let ErrorLogger () = 

    let errors = ref 0

    { new ErrorLogger("ErrorLoggerThatQuitsAfterMaxErrors") with 
            member x.ErrorSinkImpl(err) = 
//                if !errors >= tcConfigB.maxErrors then 
//                    DoWithErrorColor true (fun () -> Printf.eprintfn "%s" (FSComp.SR.fscTooManyErrors()))
//                    exiter.Exit 1
//
//                DoWithErrorColor false (fun () -> 
//                    (writeViaBufferWithEnvironmentNewLines stderr (OutputErrorOrWarning (tcConfigB.implicitIncludeDir,tcConfigB.showFullPaths,tcConfigB.flatErrors,tcConfigB.errorStyle,false)) err;  stderr.WriteLine()));
//
//                incr errors
//
//                match err.Exception with 
//                | InternalError _ 
//                | Failure _ 
//                | :? KeyNotFoundException -> 
//                    match tcConfigB.simulateException with
//                    | Some _ -> () // Don't show an assert for simulateException case so that unittests can run without an assert dialog.                     
//                    | None -> System.Diagnostics.Debug.Assert(false,sprintf "Bug seen in compiler: %s" (err.ToString()))
//                | _ -> 
                    ()
            member x.ErrorSinkRichImpl(err) = 
//                if !errors >= tcConfigB.maxErrors then 
//                    DoWithErrorColor true (fun () -> Printf.eprintfn "%s" (FSComp.SR.fscTooManyErrors()))
//                    exiter.Exit 1
//
//                DoWithErrorColor false (fun () -> 
//                    (writeViaBufferWithEnvironmentNewLines stderr (OutputErrorOrWarning (tcConfigB.implicitIncludeDir,tcConfigB.showFullPaths,tcConfigB.flatErrors,tcConfigB.errorStyle,false)) (fst err);  stderr.WriteLine()));
//
//                incr errors
//
//                match (fst err).Exception with 
//                | InternalError _ 
//                | Failure _ 
//                | :? KeyNotFoundException -> 
//                    match tcConfigB.simulateException with
//                    | Some _ -> () // Don't show an assert for simulateException case so that unittests can run without an assert dialog.                     
//                    | None -> System.Diagnostics.Debug.Assert(false,sprintf "Bug seen in compiler: %s" (err.ToString()))
//                | _ -> 
                    ()
            member x.WarnSinkImpl(err) =  ()
//                DoWithErrorColor true (fun () -> 
//                    if (ReportWarningAsError tcConfigB.globalWarnLevel tcConfigB.specificWarnOff tcConfigB.specificWarnOn tcConfigB.specificWarnAsError tcConfigB.specificWarnAsWarn tcConfigB.globalWarnAsError err) then 
//                        x.ErrorSink(err)
//                    elif ReportWarning tcConfigB.globalWarnLevel tcConfigB.specificWarnOff tcConfigB.specificWarnOn err then 
//                        writeViaBufferWithEnvironmentNewLines stderr (OutputErrorOrWarning (tcConfigB.implicitIncludeDir,tcConfigB.showFullPaths,tcConfigB.flatErrors,tcConfigB.errorStyle,true)) err;  
//                        stderr.WriteLine())
            member x.ErrorCount = !errors  }


/// Build an ErrorLogger that delegates to another ErrorLogger but filters warnings turned off by the given pragma declarations
type DelayedErrorLogger(errorLogger:ErrorLogger) =
    inherit ErrorLogger("DelayedErrorLogger")
    let delayed = new ResizeArray<_>()
    override x.ErrorSinkImpl err = delayed.Add (err, "",true)
    override x.ErrorSinkRichImpl (err) = 
      delayed.Add (fst err, snd err,true)
    override x.ErrorCount = [for a,b,c in delayed do if c then yield c] |> Seq.length
    override x.WarnSinkImpl err = delayed.Add(err,"",false)
    member x.CommitDelayedErrorsAndWarnings() = 
        // Eagerly grab all the errors and warnings from the mutable collection
        let errors = delayed |> Seq.toList
        // Now report them
        for (err,_,isError) in errors do
            if isError then errorLogger.ErrorSink err else errorLogger.WarnSink err
    member x.FilterLexerErrors() =
      let res =
        [for phased_error,_, bool in delayed do
//          if phased_error.Exception.Message <> "Exception of type 'Microsoft.FSharp.Compiler.ErrorLogger+PhasedError' was thrown." then
            yield phased_error,"", bool]
      let syntax_error =
        [for phased_error,str, _ in delayed do
//          if phased_error.Exception.Message = "Exception of type 'Microsoft.FSharp.Compiler.ErrorLogger+PhasedError' was thrown." then
            yield str]
      delayed.Clear()
      delayed.AddRange(res)
      syntax_error



type ErrorLoggerFilteringByScopedPragmas (checkFile,scopedPragmas,errorLogger:ErrorLogger) =
    inherit ErrorLogger("ErrorLoggerFilteringByScopedPragmas")
    let mutable scopedPragmas = scopedPragmas
    member x.ScopedPragmas with set v = scopedPragmas <- v
    override x.ErrorSinkImpl err = errorLogger.ErrorSink err
    override x.ErrorSinkRichImpl err = errorLogger.ErrorSinkRich err
    override x.ErrorCount = errorLogger.ErrorCount
    override x.WarnSinkImpl err = 
//        let report = 
//            let warningNum = GetErrorNumber err
//            match RangeOfError err with 
//            | Some m -> 
//                not (scopedPragmas |> List.exists (fun pragma ->
//                    match pragma with 
//                    | ScopedPragma.WarningOff(pragmaRange,warningNumFromPragma) -> 
//                        warningNum = warningNumFromPragma && 
//                        (not checkFile || m.FileIndex = pragmaRange.FileIndex) &&
//                        Range.posGeq m.Start pragmaRange.Start))  
//            | None -> true
//        if report then 
        errorLogger.WarnSink(err); 
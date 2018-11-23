Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Reflection
Imports System.ComponentModel
Imports DevExpress.Persistent.Base
Imports DevExpress.XtraSpellChecker
Imports DevExpress.ExpressApp.Actions

Namespace DevExpress.ExpressApp.SpellChecker
    Public MustInherit Class SpellCheckerWindowController
        Inherits WindowController
        Private Shared lockObject As Object = New Object()
        Private Const ActiveKeyCanCheckSpelling As String = "CanCheckSpelling"
        Public Sub New()
            CheckSpellingAction = New SimpleAction(Me, "CheckSpelling", PredefinedCategory.RecordEdit)
            CheckSpellingAction.Caption = "Check Spelling"
            CheckSpellingAction.ToolTip = "Check the spelling and grammar of text in the form."
            CheckSpellingAction.Category = PredefinedCategory.RecordEdit.ToString()
            CheckSpellingAction.ImageName = "Action_SpellChecker"
            CheckSpellingAction.TargetViewType = ViewType.DetailView
            AddHandler CheckSpellingAction.Execute, AddressOf checkSpellingAction_Execute
        End Sub
        Private Sub checkSpellingAction_Execute(ByVal sender As Object, ByVal e As SimpleActionExecuteEventArgs)
            CheckSpelling()
        End Sub
        Protected Overrides Sub OnActivated()
            MyBase.OnActivated()
            AddHandler Window.TemplateChanged, AddressOf Window_TemplateChanged
        End Sub
        Protected Overrides Sub OnDeactivated()
            RemoveHandler Window.TemplateChanged, AddressOf Window_TemplateChanged
            ReleaseSpellChecker()
            MyBase.OnDeactivated()
        End Sub
        Private Sub ReleaseSpellChecker()
            If TypeOf SpellCheckerComponent Is IDisposable Then
                CType(SpellCheckerComponent, IDisposable).Dispose()
                SpellCheckerComponent = Nothing
            End If
        End Sub
        Private Sub Window_TemplateChanged(ByVal sender As Object, ByVal e As EventArgs)
            Dim window As Window = CType(sender, Window)
            If window.Template Is Nothing Then
                ReleaseSpellChecker()
            End If
            UpdateCheckSpellingAction(window)
        End Sub
        Protected Overridable Sub UpdateCheckSpellingAction(ByVal window As Window)
            Dim isInitialized As Boolean = InitializeSpellChecker(window)
            CheckSpellingAction.Active(ActiveKeyCanCheckSpelling) = isInitialized
        End Sub
        ''' <summary>
        ''' Creates and initializes a spell checker component for the specified parent container.
        ''' </summary>
        ''' <param name="template"></param>
        Private Function InitializeSpellChecker(ByVal window As Window) As Boolean
            Dim success As Boolean = False
            If (SpellCheckerComponent Is Nothing) AndAlso CanCheckSpelling(window.Context, window.Template) Then
                SpellCheckerComponent = CreateSpellCheckerComponent(window.Template)
                If SpellCheckerComponent IsNot Nothing Then
                    EnsureDictionaries()
                    ActivateDictionaries()
                    OnSpellCheckerCreated(New SpellCheckerCreatedEventArgs(SpellCheckerComponent))
                    success = True
                End If
            End If
            Return success
        End Function
        ''' <summary>
        ''' Determines whether the specified form template is supposed to be spell checked.
        ''' </summary>
        ''' <param name="context"></param>
        ''' <param name="template"></param>
        ''' <returns></returns>
        Public Overridable Function CanCheckSpelling(ByVal context As TemplateContext, ByVal template As Object) As Boolean
            Dim args As New QueryCanCheckSpellingEventArgs(context, template)
            Dim isTargetTemplate As Boolean = (template IsNot Nothing) AndAlso (context = TemplateContext.ApplicationWindow OrElse context = TemplateContext.PopupWindow OrElse context = TemplateContext.View)
            args.Cancel = Not (SpellCheckerOptions.Enabled AndAlso isTargetTemplate)
            OnQueryCanCheckSpelling(args)
            Return Not args.Cancel
        End Function
        ''' <summary>
        ''' Initializes a default dictionary depending on the platform.
        ''' </summary>
        ''' <returns>SpellCheckerISpellDictionary</returns>
        Protected MustOverride Function CreateDefaultDictionaryCore() As SpellCheckerISpellDictionary
        ''' <summary>
        ''' Initializes a custom dictionary depending on the platform.
        ''' </summary>
        ''' <returns>SpellCheckerCustomDictionary</returns>
        Protected MustOverride Function CreateCustomDictionaryCore() As SpellCheckerCustomDictionary
        ''' <summary>
        ''' Allows end-users to spell check the current form.
        ''' </summary>
        Public MustOverride Sub CheckSpelling()
        ''' <summary>
        ''' Links dictionaries to the created spell checker component.
        ''' </summary>
        Protected MustOverride Sub ActivateDictionaries()
        ''' <summary>
        ''' Creates and initializes a spell checker component for each platform for the specified parent container.
        ''' </summary>
        ''' <param name="template"></param>
        ''' <returns></returns>
        Protected MustOverride Function CreateSpellCheckerComponent(ByVal template As Object) As Object
        ''' <summary>
        ''' Initializes static default and custom dictionaries for the first time they are accessed in the application.
        ''' </summary>
        Private Sub EnsureDictionaries()
            If DefaultDictionary Is Nothing Then
                SyncLock lockObject
                    If DefaultDictionary Is Nothing Then
                        Dim result As SpellCheckerISpellDictionary = CreateDefaultDictionaryCore()
                        SetupDefaultDictionary(result)
                        Dim args As New DictionaryCreatedEventArgs(result, False)
                        OnDictionaryCreated(args)
                        DefaultDictionary = If(TryCast(args.Dictionary, SpellCheckerISpellDictionary), result)
                    End If
                End SyncLock
            End If
            If CustomDictionary Is Nothing Then
                SyncLock lockObject
                    If CustomDictionary Is Nothing Then
                        Dim result As SpellCheckerCustomDictionary = CreateCustomDictionaryCore()
                        SetupCustomDictionary(result)
                        Dim args As New DictionaryCreatedEventArgs(result, True)
                        OnDictionaryCreated(args)
                        CustomDictionary = If(TryCast(args.Dictionary, SpellCheckerCustomDictionary), result)
                    End If
                End SyncLock
            End If
        End Sub
        ''' <summary>
        ''' Configures the default dictionary settings.
        ''' </summary>
        ''' <param name="dictionary"></param>
        Protected Overridable Sub SetupDefaultDictionary(ByVal dictionary As SpellCheckerISpellDictionary)
            dictionary.Culture = SpellChecker.Culture
            dictionary.Encoding = System.Text.Encoding.UTF8
            If SpellCheckerOptions.PathResolutionMode = FilePathResolutionMode.None Then
                Dim streamInfo As SpellCheckerDictionaryStreamInfo = GetDictionaryStreamInfo(False)
                Try
                    dictionary.LoadFromStream(streamInfo.DictionaryStream, streamInfo.GrammarStream, streamInfo.AlphabetStream)
                Finally
                    If streamInfo.AlphabetStream IsNot Nothing Then
                        streamInfo.AlphabetStream.Dispose()
                        streamInfo.AlphabetStream = Nothing
                    End If
                    If streamInfo.DictionaryStream IsNot Nothing Then
                        streamInfo.DictionaryStream.Dispose()
                        streamInfo.DictionaryStream = Nothing
                    End If
                    If streamInfo.GrammarStream IsNot Nothing Then
                        streamInfo.GrammarStream.Dispose()
                        streamInfo.GrammarStream = Nothing
                    End If
                End Try
            Else
                Dim fileInfo As SpellCheckerDictionaryFileInfo = GetDictionaryFileInfo(False)
                dictionary.AlphabetPath = fileInfo.AlphabetPath
                dictionary.DictionaryPath = fileInfo.DictionaryPath
                dictionary.GrammarPath = fileInfo.GrammarPath
            End If
        End Sub
        ''' <summary>
        ''' Configures the custom dictionary settings.
        ''' </summary>
        ''' <param name="dictionary"></param>
        Protected Overridable Sub SetupCustomDictionary(ByVal dictionary As SpellCheckerCustomDictionary)
            dictionary.Encoding = System.Text.Encoding.UTF8
            dictionary.Culture = SpellChecker.Culture
            If SpellCheckerOptions.PathResolutionMode = FilePathResolutionMode.None Then
                Dim streamInfo As SpellCheckerDictionaryStreamInfo = GetDictionaryStreamInfo(True)
                Try
                    dictionary.Load(streamInfo.DictionaryStream, streamInfo.AlphabetStream)
                Finally
                    If streamInfo.AlphabetStream IsNot Nothing Then
                        streamInfo.AlphabetStream.Dispose()
                        streamInfo.AlphabetStream = Nothing
                    End If
                    If streamInfo.DictionaryStream IsNot Nothing Then
                        streamInfo.DictionaryStream.Dispose()
                        streamInfo.DictionaryStream = Nothing
                    End If
                End Try
            Else
                Dim fileInfo As SpellCheckerDictionaryFileInfo = GetDictionaryFileInfo(True)
                dictionary.AlphabetPath = fileInfo.AlphabetPath
                dictionary.DictionaryPath = fileInfo.DictionaryPath
            End If
        End Sub
        Protected Overridable Sub OnDictionaryCreated(ByVal args As DictionaryCreatedEventArgs)
            If DictionaryCreated IsNot Nothing Then
                DictionaryCreated(Me, args)
            End If
        End Sub
        Protected Overridable Sub OnQueryCanCheckSpelling(ByVal args As QueryCanCheckSpellingEventArgs)
            If QueryCanCheckSpelling IsNot Nothing Then
                QueryCanCheckSpelling(Me, args)
            End If
        End Sub
        Protected Overridable Sub OnSpellCheckerCreated(ByVal args As SpellCheckerCreatedEventArgs)
            If SpellCheckerCreated IsNot Nothing Then
                SpellCheckerCreated(Me, args)
            End If
        End Sub

        ''' <summary>
        ''' Provides a structure containing basic dictionary file settings depending on the Options | SpellChecker node values in the Application Model.
        ''' </summary>
        ''' <param name="isCustom"></param>
        ''' <returns></returns>
        Protected Overridable Function GetDictionaryFileInfo(ByVal isCustom As Boolean) As SpellCheckerDictionaryFileInfo
            Dim filePathPrefix As String = String.Empty
            If SpellCheckerOptions.PathResolutionMode = FilePathResolutionMode.RelativeToApplicationFolder Then
                filePathPrefix = PathHelper.GetApplicationFolder()

            End If
            Return New SpellCheckerDictionaryFileInfo(filePathPrefix & SpellCheckerOptions.AlphabetPath, filePathPrefix & (If(isCustom, SpellCheckerOptions.CustomDictionaryPath, SpellCheckerOptions.DefaultDictionaryPath)), If(isCustom, String.Empty, (filePathPrefix & SpellCheckerOptions.GrammarPath)))
        End Function

        ''' <summary>
        ''' Provides a structure containing basic dictionary stream settings.
        ''' </summary>
        ''' <param name="isCustom"></param>
        ''' <returns></returns>
        'TODO Dennis: Consider re-using the DictionaryHelper.CreateEnglishDictionary method instead.
        <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
        Protected Overridable Function GetDictionaryStreamInfo(ByVal isCustom As Boolean) As SpellCheckerDictionaryStreamInfo
            Dim resourceAssembly As System.Reflection.Assembly = GetType(SpellCheckerBase).Assembly
            Dim resourcePrefix As String = resourceAssembly.GetName().Name
            Try
                Dim alphabeth As String = "ABCDEFGHIJKLMNOPQRSTUVWXYZ"
                Dim alphabethStream As Stream = New MemoryStream(System.Text.Encoding.Unicode.GetBytes(alphabeth))
                Dim dictionaryStream As Stream = resourceAssembly.GetManifestResourceStream("DevExpress.XtraSpellChecker.Core.Dictionary.american.xlg")
                Dim grammarStream As Stream = resourceAssembly.GetManifestResourceStream("DevExpress.XtraSpellChecker.Core.Dictionary.english.aff")
                Return New SpellCheckerDictionaryStreamInfo(alphabethStream, If(isCustom, New FileStream(PathHelper.GetApplicationFolder() + SpellCheckerOptions.CustomDictionaryPath, FileMode.OpenOrCreate, FileAccess.ReadWrite), dictionaryStream), If(isCustom, Nothing, grammarStream))
            Catch ex As Exception
                Throw New ArgumentException(String.Format("Cannot load a dictionary from the {0} assembly by the specified name. Make sure that the dictionary file's BuildAction is set to EmbeddedResource and its name includes the file extension.", resourceAssembly.GetName().Name), ex)
            End Try
        End Function
        ''' <summary>
        ''' Provides access to the default dictionary.
        ''' </summary>
        Public Shared Property DefaultDictionary As SpellCheckerISpellDictionary
        ''' <summary>
        ''' Provides access to the custom dictionary.
        ''' </summary>
        Public Shared Property CustomDictionary As SpellCheckerCustomDictionary

        ''' <summary>
        ''' Provides access to the spell checker component.
        ''' </summary>
        Private privateSpellCheckerComponent As Object
        Public Property SpellCheckerComponent() As Object
            Get
                Return privateSpellCheckerComponent
            End Get
            Protected Set(ByVal value As Object)
                privateSpellCheckerComponent = value
            End Set
        End Property
        ''' <summary>
        ''' Provides access to the underlying SpellCheckerBase object, the core of the spell checker component.
        ''' </summary>
        <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)>
        Protected MustOverride ReadOnly Property SpellChecker() As SpellCheckerBase
        ''' <summary>
        ''' Provides access to the Options | SpellChecker node values in the Application Model.
        ''' </summary>
        Public ReadOnly Property SpellCheckerOptions() As IModelSpellChecker
            Get
                Return (CType(Application.Model.Options, IModelOptionsSpellChecker)).SpellChecker
            End Get
        End Property
        ''' <summary>
        ''' Provides access to the Action that allows end-users to spell check the current form.
        ''' </summary>
        Private privateCheckSpellingAction As SimpleAction
        Public Property CheckSpellingAction() As SimpleAction
            Get
                Return privateCheckSpellingAction
            End Get
            Private Set(ByVal value As SimpleAction)
                privateCheckSpellingAction = value
            End Set
        End Property
        ''' <summary>
        ''' Allows to choose whether spell checking should be available for the specified form template.
        ''' </summary>
        Public QueryCanCheckSpelling As EventHandler(Of QueryCanCheckSpellingEventArgs)
        ''' <summary>
        ''' Allows to customize the spell checker component once it is created and initialized.
        ''' </summary>
        Public SpellCheckerCreated As EventHandler(Of SpellCheckerCreatedEventArgs)
        ''' <summary>
        ''' Provides to customize a created dictionary or provide a fully custom one.
        ''' </summary>
        Public DictionaryCreated As EventHandler(Of DictionaryCreatedEventArgs)
    End Class
    Public Structure SpellCheckerDictionaryFileInfo
        Public Sub New(ByVal alphabetPath As String, ByVal dictionaryPath As String, ByVal grammarPath As String)
            Me.AlphabetPath = alphabetPath
            Me.DictionaryPath = dictionaryPath
            Me.GrammarPath = grammarPath
        End Sub
        Public AlphabetPath As String
        Public DictionaryPath As String
        Public GrammarPath As String
    End Structure
    Public Structure SpellCheckerDictionaryStreamInfo
        Public Sub New(ByVal alphabetStream As Stream, ByVal dictionaryStream As Stream, ByVal grammarStream As Stream)
            Me.AlphabetStream = alphabetStream
            Me.DictionaryStream = dictionaryStream
            Me.GrammarStream = grammarStream
        End Sub
        Public AlphabetStream As Stream
        Public DictionaryStream As Stream
        Public GrammarStream As Stream
    End Structure
End Namespace
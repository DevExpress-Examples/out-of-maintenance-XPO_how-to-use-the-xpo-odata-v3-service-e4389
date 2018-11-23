Imports Microsoft.VisualBasic
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Model

Namespace DevExpress.ExpressApp.SpellChecker
	Public NotInheritable Partial Class SpellCheckerModule
		Inherits ModuleBase
		Public Sub New()
			InitializeComponent()
		End Sub
		Public Overrides Sub ExtendModelInterfaces(ByVal extenders As ModelInterfaceExtenders)
			MyBase.ExtendModelInterfaces(extenders)
			extenders.Add(Of IModelOptions, IModelOptionsSpellChecker)()
		End Sub
	End Class
	Public Interface IModelOptionsSpellChecker
	Inherits IModelNode
		<DXDescription("DevExpress.ExpressApp.SpellChecker.IModelOptionsSpellChecker,SpellChecker")> _
		ReadOnly Property SpellChecker() As IModelSpellChecker
	End Interface
	Public Interface IModelSpellChecker
	Inherits IModelNode
		<DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,Enabled"), Category("Behavior"), DefaultValue(True)> _
		Property Enabled() As Boolean
		<DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,AlphabetPath"), Category("Data"), Localizable(True), DefaultValue("Dictionaries\EnglishAlphabet.txt")> _
		Property AlphabetPath() As String
		<DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,GrammarPath"), Category("Data"), DefaultValue("Dictionaries\English.aff"), Localizable(True)> _
		Property GrammarPath() As String
		<DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,DefaultDictionaryPath"), Category("Data"), DefaultValue("Dictionaries\American.xlg"), Localizable(True)> _
		Property DefaultDictionaryPath() As String
		<DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,CustomDictionaryPath"), Category("Data"), DefaultValue("Dictionaries\Custom.txt"), Localizable(True)> _
		Property CustomDictionaryPath() As String
		<DXDescription("DevExpress.ExpressApp.SpellChecker.IModelSpellChecker,DefaultDictionaryPathResolution"), Category("Behavior"), DefaultValue(FilePathResolutionMode.RelativeToApplicationFolder)> _
		Property PathResolutionMode() As FilePathResolutionMode
	End Interface
	Public Enum FilePathResolutionMode
		'TODO Dennis: Consider introducing an option allowing English spell checking without doing anything by developers.
		<Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
		None
		Absolute
		RelativeToApplicationFolder
	End Enum
End Namespace

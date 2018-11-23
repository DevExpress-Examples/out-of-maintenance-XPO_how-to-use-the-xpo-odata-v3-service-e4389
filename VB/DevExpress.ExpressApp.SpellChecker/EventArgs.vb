Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports DevExpress.XtraSpellChecker

Namespace DevExpress.ExpressApp.SpellChecker
	''' <summary>
	''' Event arguments for the SpellCheckerWindowController.QueryCanCheckSpelling event.
	''' </summary>
	Public Class QueryCanCheckSpellingEventArgs
		Inherits CancelEventArgs
		Public Sub New(ByVal context As TemplateContext, ByVal template As Object)
			Me.Context = context
			Me.Template = template
		End Sub
		Private privateContext As TemplateContext
		Public Property Context() As TemplateContext
			Get
				Return privateContext
			End Get
			Private Set(ByVal value As TemplateContext)
				privateContext = value
			End Set
		End Property
		Private privateTemplate As Object
		Public Property Template() As Object
			Get
				Return privateTemplate
			End Get
			Private Set(ByVal value As Object)
				privateTemplate = value
			End Set
		End Property
	End Class
	''' <summary>
	''' Event arguments for the SpellCheckerWindowController.SpellCheckerCreated event.
	''' </summary>
	Public Class SpellCheckerCreatedEventArgs
		Inherits EventArgs
		Public Sub New(ByVal spellChecker As Object)
			Me.SpellChecker = spellChecker
		End Sub
		Private privateSpellChecker As Object
		Public Property SpellChecker() As Object
			Get
				Return privateSpellChecker
			End Get
			Private Set(ByVal value As Object)
				privateSpellChecker = value
			End Set
		End Property
	End Class
	''' <summary>
	''' Event arguments for the SpellCheckerWindowController.DictionaryCreated event.
	''' </summary>
	Public Class DictionaryCreatedEventArgs
		Inherits EventArgs
		Public Sub New(ByVal dictionary As ISpellCheckerDictionary, ByVal isCustom As Boolean)
			Me.Dictionary = dictionary
			Me.IsCustom = isCustom
		End Sub
		Private privateDictionary As ISpellCheckerDictionary
		Public Property Dictionary() As ISpellCheckerDictionary
			Get
				Return privateDictionary
			End Get
			Set(ByVal value As ISpellCheckerDictionary)
				privateDictionary = value
			End Set
		End Property
		Private privateIsCustom As Boolean
		Public Property IsCustom() As Boolean
			Get
				Return privateIsCustom
			End Get
			Private Set(ByVal value As Boolean)
				privateIsCustom = value
			End Set
		End Property
	End Class

End Namespace

Imports Microsoft.VisualBasic
Imports System.Globalization
Imports System.Windows.Forms
Imports DevExpress.XtraSpellChecker

Namespace DevExpress.ExpressApp.SpellChecker.Win
	Partial Public Class WinSpellCheckerWindowController
		Inherits SpellCheckerWindowController
		Protected Shared dictionariesStorage As SharedDictionaryStorage
		Protected Overrides Function CreateSpellCheckerComponent(ByVal parentContainer As Object) As Object
			Dim result = New DevExpress.XtraSpellChecker.SpellChecker()
			result.ParentContainer = CType(parentContainer, Form)
			result.SpellCheckMode = SpellCheckMode.AsYouType
			result.CheckAsYouTypeOptions.ShowSpellCheckForm = True
			result.CheckAsYouTypeOptions.CheckControlsInParentContainer = True
			result.Culture = CultureInfo.CurrentUICulture
			result.UseSharedDictionaries = True
			Return result
		End Function
		Public Shadows Property SpellCheckerComponent() As DevExpress.XtraSpellChecker.SpellChecker
			Get
				Return TryCast(MyBase.SpellCheckerComponent, DevExpress.XtraSpellChecker.SpellChecker)
			End Get
			Private Set(ByVal value As DevExpress.XtraSpellChecker.SpellChecker)
				MyBase.SpellCheckerComponent = value
			End Set
		End Property
		Protected Overrides ReadOnly Property SpellChecker() As SpellCheckerBase
			Get
				Return SpellCheckerComponent
			End Get
		End Property
		Public Overrides Sub CheckSpelling()
			If SpellCheckerComponent IsNot Nothing Then
				SpellCheckerComponent.CheckContainer()
			End If
		End Sub
		Protected Overrides Sub UpdateCheckSpellingAction(ByVal window As Window)
			MyBase.UpdateCheckSpellingAction(window)
			CheckSpellingAction.Active("IsMain") = Not window.IsMain
		End Sub
		Protected Overrides Sub ActivateDictionaries()
			If dictionariesStorage Is Nothing Then
				dictionariesStorage = New SharedDictionaryStorage()
				dictionariesStorage.Dictionaries.Add(DefaultDictionary)
				dictionariesStorage.Dictionaries.Add(CustomDictionary)
			End If
			SpellCheckerComponent.Dictionaries.AddRange(dictionariesStorage.Dictionaries)
		End Sub
		Protected Overrides Function CreateDefaultDictionaryCore() As SpellCheckerISpellDictionary
			Return New SpellCheckerISpellDictionary()
		End Function
		Protected Overrides Function CreateCustomDictionaryCore() As SpellCheckerCustomDictionary
			Return New SpellCheckerCustomDictionary()
		End Function
		'protected override void OnWindowViewChanged(View view) {
		'    base.OnWindowViewChanged(view);
		'    if(view is ListView) {
		'        CheckSpellingAction.Active[ActiveKeyWindowView] = false;
		'    }
		'}
		'protected override void OnQueryCanCheckSpelling(QueryCanCheckSpellingEventArgs args) {
		'    base.OnQueryCanCheckSpelling(args);
		'    Form form = args.Template as Form;
		'    if(form != null) {
		'        args.Cancel &= form.IsMdiChild;
		'    }
		'}
	End Class
End Namespace
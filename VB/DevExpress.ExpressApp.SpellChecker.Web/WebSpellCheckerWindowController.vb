Imports Microsoft.VisualBasic
Imports System
Imports System.Linq
Imports System.Web.UI
Imports System.Globalization
Imports DevExpress.ExpressApp.Web
Imports System.Collections.Generic
Imports DevExpress.XtraSpellChecker
Imports DevExpress.ExpressApp.Templates
Imports DevExpress.Web.ASPxSpellChecker
Imports DevExpress.ExpressApp.Web.Controls

Namespace DevExpress.ExpressApp.SpellChecker.Web
	Partial Public Class WebSpellCheckerWindowController
		Inherits SpellCheckerWindowController
		Private Const ActiveKeyDetailViewEditMode As String = "DetailView.ViewEditMode is Edit"
		Protected Shared spellCheckerToWebDictionaryMap As New Dictionary(Of ISpellCheckerDictionary, WebDictionary)()
		Public Const SpellCheckerClientInstanceName As String = "xaf_spellChecker"
		Protected Overrides Function CreateSpellCheckerComponent(ByVal template As Object) As Object
			Dim result As DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker = Nothing
			Dim page As Page = TryCast(template, Page)
			If page IsNot Nothing Then
				Dim controlId As String = SpellCheckerClientInstanceName
				result = TryCast(page.FindControl(controlId), DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker)
				If result Is Nothing Then
					result = New DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker()
					result.ClientInstanceName = controlId
					result.ID = result.ClientInstanceName
					result.ShowLoadingPanel = False
					result.Culture = CultureInfo.CurrentUICulture
					'Dennis: The Page.Form property is not yet initialized at this moment, while the ASPxSpellChecker must be added into the Form element only.
					Dim form As Control = WebWindow.GetForm(page)
					If form IsNot Nothing Then
						form.Controls.Add(result)
					End If
				End If
			End If
			Return result
		End Function
		Public Shadows Property SpellCheckerComponent() As DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker
			Get
				Return TryCast(MyBase.SpellCheckerComponent, DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker)
			End Get
			Private Set(ByVal value As DevExpress.Web.ASPxSpellChecker.ASPxSpellChecker)
				MyBase.SpellCheckerComponent = value
			End Set
		End Property
		'TODO Dennis: Ask the team to make the ASPxSpellChecker.SpellChecker property public in the next version.
		Protected Overrides ReadOnly Property SpellChecker() As SpellCheckerBase
			Get
				Return TryCast(SpellCheckerComponent.GetType().GetProperty("SpellChecker", System.Reflection.BindingFlags.NonPublic Or System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.GetProperty).GetValue(SpellCheckerComponent, Nothing), SpellCheckerBase)
			End Get
		End Property
		Public Overrides Sub CheckSpelling()
			Dim page As IViewSiteTemplate = TryCast(Frame.Template, IViewSiteTemplate)
			If page IsNot Nothing Then
				WebWindow.CurrentRequestWindow.RegisterStartupScript(Me.GetType().Name, String.Format("{0}.CheckElementsInContainer({1});", SpellCheckerClientInstanceName,If(TypeOf page.ViewSiteControl Is ViewSiteControl, String.Format("window.document.getElementById('{0}')", (CType(page.ViewSiteControl, ViewSiteControl)).Control.ClientID), "window.document")))
			End If
		End Sub
		Protected Overrides Sub ActivateDictionaries()
			SpellCheckerComponent.Dictionaries.Add(spellCheckerToWebDictionaryMap(DefaultDictionary))
			SpellCheckerComponent.Dictionaries.Add(spellCheckerToWebDictionaryMap(CustomDictionary))
		End Sub
		Protected Overrides Function CreateDefaultDictionaryCore() As SpellCheckerISpellDictionary
			Dim defaultWebDictionary As New ASPxSpellCheckerISpellDictionary()
			defaultWebDictionary.CacheKey = defaultWebDictionary.GetType().Name
			spellCheckerToWebDictionaryMap.Add(defaultWebDictionary.Dictionary, defaultWebDictionary)
			Return defaultWebDictionary.Dictionary
		End Function
		Protected Overrides Function CreateCustomDictionaryCore() As SpellCheckerCustomDictionary
			Dim customWebDictionary As New ASPxSpellCheckerCustomDictionary()
			customWebDictionary.CacheKey = customWebDictionary.GetType().Name
			spellCheckerToWebDictionaryMap.Add(customWebDictionary.Dictionary, customWebDictionary)
			Return customWebDictionary.Dictionary
		End Function
		'protected override void OnWindowViewChanged(View view) {
		'    base.OnWindowViewChanged(view);
		'    DetailView dv = Window.View as DetailView;
		'    if((dv != null) && (dv.ViewEditMode == Editors.ViewEditMode.View)) {
		'        CheckSpellingAction.Active[ActiveKeyDetailViewEditMode] = false;
		'    }
		'}
	End Class
End Namespace
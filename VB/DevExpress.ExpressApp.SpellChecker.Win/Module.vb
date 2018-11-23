Imports Microsoft.VisualBasic
Imports System
Imports System.IO
Imports System.Reflection
Imports System.Windows.Forms
Imports DevExpress.ExpressApp
Imports DevExpress.XtraEditors
Imports DevExpress.XtraSpellChecker
Imports DevExpress.ExpressApp.Win.Editors
Imports DevExpress.XtraSpellChecker.Native
Imports DevExpress.Persistent.Base
Imports System.Threading.Tasks

Namespace DevExpress.ExpressApp.SpellChecker.Win
	Public NotInheritable Partial Class SpellCheckerWindowsFormsModule
		Inherits ModuleBase
		Public Sub New()
			InitializeComponent()
			RegisterSpellCheckerControlFinders()
		End Sub
		Private Shared Sub RegisterSpellCheckerControlFinders()
			SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(GetType(StringEdit), GetType(StringEditTextBoxFinder))
			SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(GetType(LargeStringEdit), GetType(LargeStringEditTextBoxFinder))

			SpellCheckTextControllersManager.Default.RegisterClass(GetType(StringEdit), GetType(SimpleTextEditTextController))
			SpellCheckTextControllersManager.Default.RegisterClass(GetType(LargeStringEdit), GetType(SimpleTextEditTextController))
			SpellCheckTextControllersManager.Default.RegisterClass(GetType(MemoEdit), GetType(SimpleTextEditTextController))
			'Q418588
			'SpellCheckTextControllersManager.Default.RegisterClass(typeof(RichEditUserControl), typeof(RichEditSpellCheckController));
			'SpellCheckTextBoxBaseFinderManager.Default.RegisterClass(typeof(RichEditUserControl), typeof(RichTextBoxFinder));
		End Sub
	End Class
	Public Class LargeStringEditTextBoxFinder
		Inherits MemoEditTextBoxFinder
		Public Overrides Function GetTextBoxInstance(ByVal editControl As Control) As TextBoxBase
			If TypeOf editControl Is LargeStringEdit Then
				Return MyBase.GetTextBoxInstance(CType(editControl, MemoEdit))
			End If
			Return Nothing
		End Function
	End Class
	Public Class StringEditTextBoxFinder
		Inherits TextEditTextBoxFinder
		Public Overrides Function GetTextBoxInstance(ByVal editControl As Control) As TextBoxBase
			If TypeOf editControl Is StringEdit Then
				Return MyBase.GetTextBoxInstance(CType(editControl, TextEdit))
			End If
			Return Nothing
		End Function
	End Class
End Namespace

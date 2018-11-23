Imports Microsoft.VisualBasic
Imports DevExpress.ExpressApp
Imports DevExpress.ExpressApp.Xpo
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Win

Namespace Demo.Win
	Partial Public Class DemoWindowsFormsApplication
		Inherits WinApplication
		Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
			args.ObjectSpaceProvider = New XPObjectSpaceProvider(args.ConnectionString, args.Connection)
		End Sub
		Public Sub New()
			InitializeComponent()
		End Sub
		Private Sub DemoWindowsFormsApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
			e.Updater.Update()
			e.Handled = True
		End Sub
	End Class
End Namespace

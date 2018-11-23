Imports Microsoft.VisualBasic
Imports System
Imports DevExpress.ExpressApp
Imports System.ComponentModel
Imports DevExpress.ExpressApp.Xpo
Imports DevExpress.ExpressApp.Web
Imports System.Collections.Generic
'using DevExpress.ExpressApp.Security;

Namespace Demo.Web
	' You can override various virtual methods and handle corresponding events to manage various aspects of your XAF application UI and behavior.
	Partial Public Class DemoAspNetApplication ' http://documentation.devexpress.com/#Xaf/DevExpressExpressAppWebWebApplicationMembersTopicAll
		Inherits WebApplication
		Private module1 As DevExpress.ExpressApp.SystemModule.SystemModule
		Private module2 As DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule
		Private module3 As Demo.Module.DemoModule
		Private spellCheckerAspNetModule1 As DevExpress.ExpressApp.SpellChecker.Web.SpellCheckerAspNetModule
		Private spellCheckerModule1 As DevExpress.ExpressApp.SpellChecker.SpellCheckerModule
		Private sqlConnection1 As System.Data.SqlClient.SqlConnection

		Public Sub New()
			InitializeComponent()
		End Sub

		' Override to execute custom code after a logon has been performed, the SecuritySystem object is initialized, logon parameters have been saved and user model differences are loaded.
		Protected Overrides Sub OnLoggedOn(ByVal args As LogonEventArgs) ' http://documentation.devexpress.com/#Xaf/DevExpressExpressAppXafApplication_LoggedOntopic
			MyBase.OnLoggedOn(args)
		End Sub

		' Override to execute custom code after a user has logged off.
		Protected Overrides Sub OnLoggedOff() ' http://documentation.devexpress.com/#Xaf/DevExpressExpressAppXafApplication_LoggedOfftopic
			MyBase.OnLoggedOff()
		End Sub

		Protected Overrides Sub CreateDefaultObjectSpaceProvider(ByVal args As CreateCustomObjectSpaceProviderEventArgs)
			args.ObjectSpaceProvider = New XPObjectSpaceProvider(args.ConnectionString, args.Connection, True)
		End Sub

		Private Sub DemoAspNetApplication_DatabaseVersionMismatch(ByVal sender As Object, ByVal e As DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs) Handles MyBase.DatabaseVersionMismatch
#If EASYTEST Then
			e.Updater.Update()
			e.Handled = True
#Else
			If System.Diagnostics.Debugger.IsAttached Then
				e.Updater.Update()
				e.Handled = True
			Else
				Dim message As String = "The application cannot connect to the specified database, because the latter doesn't exist or its version is older than that of the application." & Constants.vbCrLf & "This error occurred  because the automatic database update was disabled when the application was started without debugging." & Constants.vbCrLf & "To avoid this error, you should either start the application under Visual Studio in debug mode, or modify the " & "source code of the 'DatabaseVersionMismatch' event handler to enable automatic database update, " & "or manually create a database using the 'DBUpdater' tool." & Constants.vbCrLf & "Anyway, refer to the following help topics for more detailed information:" & Constants.vbCrLf & "'Update Application and Database Versions' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument2795.htm" & Constants.vbCrLf & "'Database Security References' at http://www.devexpress.com/Help/?document=ExpressApp/CustomDocument3237.htm" & Constants.vbCrLf & "If this doesn't help, please contact our Support Team at http://www.devexpress.com/Support/Center/"

				If e.CompatibilityError IsNot Nothing AndAlso e.CompatibilityError.Exception IsNot Nothing Then
					message &= Constants.vbCrLf & Constants.vbCrLf & "Inner exception: " & e.CompatibilityError.Exception.Message
				End If
				Throw New InvalidOperationException(message)
			End If
#End If
		End Sub

		Private Sub InitializeComponent()
			Me.module1 = New DevExpress.ExpressApp.SystemModule.SystemModule()
			Me.module2 = New DevExpress.ExpressApp.Web.SystemModule.SystemAspNetModule()
			Me.module3 = New Demo.Module.DemoModule()
			Me.sqlConnection1 = New System.Data.SqlClient.SqlConnection()
			Me.spellCheckerAspNetModule1 = New DevExpress.ExpressApp.SpellChecker.Web.SpellCheckerAspNetModule()
			Me.spellCheckerModule1 = New DevExpress.ExpressApp.SpellChecker.SpellCheckerModule()
			CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
			' 
			' sqlConnection1
			' 
			Me.sqlConnection1.ConnectionString = "Integrated Security=SSPI;Pooling=false;Data Source=.\SQLEXPRESS;Initial Catalog=W" & "inSolution"
			Me.sqlConnection1.FireInfoMessageEventOnUserErrors = False
			' 
			' DemoAspNetApplication
			' 
			Me.ApplicationName = "Demo"
			Me.Connection = Me.sqlConnection1
			Me.Modules.Add(Me.module1)
			Me.Modules.Add(Me.module2)
			Me.Modules.Add(Me.module3)
			Me.Modules.Add(Me.spellCheckerModule1)
			Me.Modules.Add(Me.spellCheckerAspNetModule1)
'			Me.DatabaseVersionMismatch += New System.EventHandler(Of DevExpress.ExpressApp.DatabaseVersionMismatchEventArgs)(Me.DemoAspNetApplication_DatabaseVersionMismatch);
			CType(Me, System.ComponentModel.ISupportInitialize).EndInit()

		End Sub
	End Class
End Namespace
